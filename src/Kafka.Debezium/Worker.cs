using Confluent.Kafka;
using Kafka.Debezium.Models.Entities;
using Kafka.Debezium.Models.Key;
using Kafka.Debezium.Models.Key.Identifications;
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

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Kafka consumer loop started at: {time}", DateTimeOffset.Now);
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult.IsPartitionEOF) continue;

                        _logger.LogInformation($"Key: {consumeResult.Message.Key} | Value: {consumeResult.Message.Value}");

                        //HandleCustomer(consumeResult);
                        HandleProduct(consumeResult);

                        _logger.LogInformation("Consumer worker finished at: {time}", DateTimeOffset.Now);
                        Commit(consumer, consumeResult);
                    }
                    catch (OperationCanceledException oce)
                    {
                        _logger.LogError(oce, $"Consume canceled: {oce.Message}");
                        continue;
                    }
                    catch (ConsumeException ce)
                    {
                        // Consumer errors should generally be ignored (or logged) unless fatal.
                        _logger.LogError(ce, $"Consume error: {ce.Error.Reason}");

                        if (ce.Error.IsFatal)
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

        private void Commit(IConsumer<string, string> consumer, ConsumeResult<string, string> consumeResult)
        {
            consumer.Commit(consumeResult);
            consumer.StoreOffset(consumeResult.TopicPartitionOffset);
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

        private void HandleCustomer(ConsumeResult<string, string> consumeResult)
        {
            ModelValue<Customer>? message = null;
            if (consumeResult.Message.Value is not null)
                message = JsonSerializer.Deserialize<ModelValue<Customer>>(consumeResult.Message.Value);
            var key = JsonSerializer.Deserialize<ModelKey<CustomerKey>>(consumeResult.Message.Key);

            var offset = consumeResult.TopicPartitionOffset;

            switch (message?.Payload?.Op)
            {
                case "r":
                    Console.WriteLine("READ");

                    AfterCustomer(message);
                    break;

                case "c":
                    Console.WriteLine("INSERT");

                    AfterCustomer(message);
                    break;

                case "u":
                    Console.WriteLine("UPDATE");

                    BeforeCustomer(message);
                    Console.Write("\n");
                    AfterCustomer(message);
                    break;

                case "d":
                    Console.WriteLine("DELETE");

                    BeforeCustomer(message);
                    break;

                default:
                    Console.WriteLine("DEFAULT");

                    if (message is null)
                    {
                        Console.WriteLine($"REMOVED - {key?.Payload?.Id}");
                    }

                    break;
            }
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

        private void HandleProduct(ConsumeResult<string, string> consumeResult)
        {
            ModelValue<Product>? message = null;
            if (consumeResult.Message.Value is not null)
                message = JsonSerializer.Deserialize<ModelValue<Product>>(consumeResult.Message.Value);
            var key = JsonSerializer.Deserialize<ModelKey<ProductKey>>(consumeResult.Message.Key);

            var offset = consumeResult.TopicPartitionOffset;

            switch (message?.Payload?.Op)
            {
                case "r":
                    Console.WriteLine("READ");

                    AfterProduct(message);
                    break;

                case "c":
                    Console.WriteLine("INSERT");

                    AfterProduct(message);
                    break;

                case "u":
                    Console.WriteLine("UPDATE");

                    BeforeProduct(message);
                    Console.Write("\n");
                    AfterProduct(message);
                    break;

                case "d":
                    Console.WriteLine("DELETE");

                    BeforeProduct(message);
                    break;

                default:
                    Console.WriteLine("DEFAULT");

                    if (message is null)
                    {
                        Console.WriteLine($"REMOVED - {key?.Payload?.Code}");
                    }

                    break;
            }
        }        
    }
}