using Confluent.Kafka;
using Kafka.Debezium.Models.Entities;
using Kafka.Debezium.Models.Key;
using Kafka.Debezium.Models.Value;
using System.Text.Json;

namespace Kafka.Debezium
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _host;
        private readonly string _topic;

        public Worker(ILogger<Worker> logger,
            IConfiguration config)
        {
            _logger = logger;
            _host = config.GetSection("KafkaConfigurations:Host").Value ?? string.Empty;
            _topic = config.GetSection("KafkaConfigurations:Topic").Value ?? string.Empty;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _host,
                GroupId = $"{_topic}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false,
            };

            using (var consumer = new ConsumerBuilder<string, string>(config)
                .SetLogHandler((_, logMessage) =>
                {
                    _logger.LogInformation($"Kafka log: {logMessage.Message}");
                })
                .SetErrorHandler((_, error) =>
                {
                    if (error.IsFatal)
                    {
                        _logger.LogError($"Kafka fatal error: {error.Reason}");
                        throw new KafkaException(error);
                    }
                    _logger.LogWarning($"Kafka error: {error.Reason}");
                })
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    _logger.LogInformation($"Kafka log: Assigned partitions: [{string.Join(", ", partitions)}]");
                })
                .Build())
            {
                consumer.Subscribe(_topic);
                //consumer.Subscribe("dbserver.KafkaDebezium.dbo.Products");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Kafka consumer loop started at: {time}", DateTimeOffset.Now);
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult.IsPartitionEOF) continue;
                        
                        _logger.LogInformation($"Key: {consumeResult.Message.Key} | Value: {consumeResult.Message.Value}");

                        ModelValue<Customer>? customer = null;
                        if (consumeResult.Message.Value is not null)
                            customer = JsonSerializer.Deserialize<ModelValue<Customer>>(consumeResult.Message.Value);

                        ModelValue<Product>? product = null;
                        //if (consumeResult.Message.Value is not null)
                        //    product = JsonSerializer.Deserialize<ModelValue<Product>>(consumeResult.Message.Value);

                        var key = JsonSerializer.Deserialize<ModelKey>(consumeResult.Message.Key);
                        var offset = consumeResult.TopicPartitionOffset;

                        switch (customer?.Payload?.Op)
                        {
                            case "c":
                                Console.WriteLine($"INSERT");

                                AfterProduct(product);
                                Console.Write("\n");
                                AfterCustomer(customer);
                                break;

                            case "d":
                                Console.WriteLine($"DELETE");

                                BeforeProduct(product);
                                Console.Write("\n");
                                BeforeCustomer(customer);
                                break;

                            case "u":
                                Console.WriteLine($"UPDATE");

                                BeforeProduct(product);
                                Console.Write("\n");
                                AfterProduct(product);

                                Console.Write("\n");
                                BeforeCustomer(customer);
                                Console.Write("\n");
                                AfterCustomer(customer);
                                break;

                            case "r":
                                Console.WriteLine($"READ");

                                AfterProduct(product);
                                Console.Write("\n");
                                AfterCustomer(customer);
                                break;

                            default:
                                Console.WriteLine($"DEFAULT");

                                if (customer is null)
                                {
                                    Console.WriteLine($"DELETADO - {key?.Payload?.Id}");
                                }

                                break;
                        }

                        _logger.LogInformation("Consumer worker finished at: {time}", DateTimeOffset.Now);
                        Commit(consumer, consumeResult);
                    }
                    catch (OperationCanceledException oce)
                    {
                        _logger.LogError(oce, $"Consume canceled: {oce.Message}");
                        continue;
                    }
                    catch (ConsumeException e)
                    {
                        // Consumer errors should generally be ignored (or logged) unless fatal.
                        _logger.LogError(e, $"Consume error: {e.Error.Reason}");

                        if (e.Error.IsFatal)
                        {
                            // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Kafka consumer will be restarted due to non-retryable exception: {ex.Message}");
                        throw;
                    }
                }
            }

            await Task.CompletedTask;
        }

        private void AfterCustomer(ModelValue<Customer> message)
        {
            Console.WriteLine("Customer");
            //Console.WriteLine($"After - Id: {message?.Payload?.After?.Id}\nNome:{message?.Payload?.After?.Name}\nE-mail:{message?.Payload?.After?.Email}\nAniversário:{message?.Payload?.After?.BirthDate}");
            Console.WriteLine($"After - Id: {message?.Payload?.After?.Id}\nNome:{message?.Payload?.After?.Name}\nE-mail:{message?.Payload?.After?.Email}");
        }

        private void BeforeCustomer(ModelValue<Customer> message)
        {
            Console.WriteLine("Customer");
            //Console.WriteLine($"Before - Id: {message?.Payload?.Before?.Id}\nNome:{message?.Payload?.Before?.Name}\nE-mail:{message?.Payload?.Before?.Email}\nAniversário:{message?.Payload?.Before?.BirthDate}");
            Console.WriteLine($"Before - Id: {message?.Payload?.Before?.Id}\nNome:{message?.Payload?.Before?.Name}\nE-mail:{message?.Payload?.Before?.Email}");
        }

        private void AfterProduct(ModelValue<Product> message)
        {
            Console.WriteLine("Product");
            Console.WriteLine($"After - Id: {message?.Payload?.After?.Id}\nNome:{message?.Payload?.After?.Name}\nAtivo:{message?.Payload?.After?.Active}");
        }

        private void BeforeProduct(ModelValue<Product> message)
        {
            Console.WriteLine("Product");
            Console.WriteLine($"Before - Id: {message?.Payload?.Before?.Id}\nNome:{message?.Payload?.Before?.Name}\nAtivo:{message?.Payload?.Before?.Active}");
        }

        private void Commit(IConsumer<string, string> consumer, ConsumeResult<string, string> consumeResult)
        {
            consumer.Commit(consumeResult);
            consumer.StoreOffset(consumeResult.TopicPartitionOffset);
        }
    }
}