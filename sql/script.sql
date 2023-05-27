INSERT INTO [dbo].[Customers]([Id],
	[Name],
	[Email],
	[BirthDate])
    VALUES('8a1d0b00-a451-49d9-832e-5a47388839bf', 
    'Fulano', 
    'fulano@gmail.com',
    GETDATE())
GO

-- UPDATE [dbo].[Customers]
--    SET [Name] = 'Fulano Ferreira',
--        [Email] = 'fulano@gmail.com'
-- WHERE [Id] = '8a1d0b00-a451-49d9-832e-5a47388839bf'

-- DELETE FROM [dbo].[Customers] WHERE ID = '8a1d0b00-a451-49d9-832e-5a47388839bf'

INSERT INTO [dbo].[Products]([Name],
	[Active])
    VALUES('PS5', 
    1)
GO

-- UPDATE [dbo].[Products]
--    SET [Name] = 'PS5',
--        [Active] = 1
-- WHERE [Id] = 1

-- DELETE FROM [dbo].[Products] WHERE ID = 1