USE [KafkaDebezium]
GO

-- Habilita o CDC
EXEC sys.sp_cdc_enable_db;
GO

-- Criar o CDC para a tabela Customers
EXEC sys.sp_cdc_enable_table
@source_schema = N'dbo',
@source_name   = N'Customers',
@role_name     = N'Admin',
@supports_net_changes = 1;
GO

EXEC sys.sp_cdc_enable_table
@source_schema = N'dbo',
@source_name   = N'Products',
@role_name     = N'Admin',
@supports_net_changes = 1;
GO

-- Desabilita o CDC
EXEC sys.sp_cdc_disable_db;
GO

-- Remove o CDC para a tabela Customers
EXEC sys.sp_cdc_disable_table  
@source_schema = N'dbo',  
@source_name   = N'Customers',  
@capture_instance = N'dbo_Customers';
GO

EXEC sys.sp_cdc_disable_table  
@source_schema = N'dbo',  
@source_name   = N'Products',  
@capture_instance = N'dbo_Products';
GO
