-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-25
-- Description:	Add column [ProductId] in table
--				[IVM_Transaction]				
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'ProductId' AND [object_id] = OBJECT_ID(N'IVM_Transaction')
)
BEGIN
    ALTER TABLE IVM_Transaction 
	ADD ProductId INT NULL
END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-25
-- Description:	Update [ProductId] of [IVM_Transaction] 
--				from [IVM_Barcode]				
-- =============================================
IF EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'ProductId' AND [object_id] = OBJECT_ID(N'IVM_Transaction')
)
BEGIN
	UPDATE IVM_Transaction
	SET ProductId = b.ProductId
	FROM IVM_Barcode b
	WHERE BarcodeId = b.Id
END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-25
-- Description:	Add Foreign Key to column [ProductId] 
--				of table [IVM_Transaction]				
-- =============================================
IF EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'ProductId' AND [object_id] = OBJECT_ID(N'IVM_Transaction')
)
BEGIN
	ALTER TABLE IVM_Transaction ALTER COLUMN ProductId INT NOT NULL
	
	IF EXISTS(SELECT * FROM sys.foreign_keys
	WHERE  name = 'FK_IVM_Transaction_Product')
	BEGIN
		ALTER TABLE [dbo].[IVM_Transaction] DROP CONSTRAINT [FK_IVM_Transaction_Product]
	END	
	
	ALTER TABLE [dbo].[IVM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK_IVM_Transaction_Product] FOREIGN KEY([ProductId])
	REFERENCES [dbo].[Product] ([Id])

	ALTER TABLE [dbo].[IVM_Transaction] CHECK CONSTRAINT [FK_IVM_Transaction_Product]
END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-25
-- Description:	Create Index on column [ProductId] 
--				and [BarcodeId] of table [IVM_Transaction]				
-- =============================================
IF EXISTS (
	SELECT * FROM sys.indexes 
	WHERE name='IX_IVM_Transaction_BarcodeId' AND object_id = OBJECT_ID('IVM_Transaction')
)
DROP INDEX [IX_IVM_Transaction_BarcodeId] ON [dbo].[IVM_Transaction] 
GO
CREATE NONCLUSTERED INDEX [IX_IVM_Transaction_BarcodeId] ON [dbo].[IVM_Transaction] 
(
	BarcodeId ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

IF EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'ProductId' AND [object_id] = OBJECT_ID(N'IVM_Transaction')
)
BEGIN
	IF EXISTS (
		SELECT * FROM sys.indexes 
		WHERE name='IX_IVM_Transaction_ProductId' AND object_id = OBJECT_ID('IVM_Transaction')
	) DROP INDEX [IX_IVM_Transaction_ProductId] ON [dbo].[IVM_Transaction] 
	
	CREATE NONCLUSTERED INDEX [IX_IVM_Transaction_ProductId] ON [dbo].[IVM_Transaction] 
	(
		ProductId ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-26
-- Description:	Create table IVM_Product_Ref stores
--				data calculated daily from IVM_Transaction
-- =============================================
DROP TABLE [dbo].[IVM_Product_Ref]
GO

CREATE TABLE [dbo].[IVM_Product_Ref](
	[ProductId] [int] NOT NULL,
	[BarcodeId] [int] NOT NULL,
	[StoreId] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[ProcessDate] [datetime] NOT NULL,
 CONSTRAINT [PK_IVM_Product_Ref] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[BarcodeId] ASC,
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-10
-- Description:	Add column Note on table Order
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'Note' AND [object_id] = OBJECT_ID(N'Order')
)
BEGIN
    ALTER TABLE [Order] 
	ADD Note nvarchar(1000) NULL
END
GO

IF NOT EXISTS(
	SELECT 1 FROM LocaleStringResource WHERE ResourceName = 'Admin.Orders.Fields.Note'
)
BEGIN
	INSERT INTO LocaleStringResource(LanguageId, ResourceName, ResourceValue) 
	VALUES
	(1, 'Admin.Orders.Fields.Note', 'Note'),
	(2, 'Admin.Orders.Fields.Note', 'Note')
END

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-10
-- Description:	Add column OrderNote on table Customer
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'OrderNote' AND [object_id] = OBJECT_ID(N'Customer')
)
BEGIN
    ALTER TABLE [Customer] 
	ADD OrderNote nvarchar(1000) NULL
END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-13
-- Description:	Add column IsHighlighted on table Category
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE [name] = N'IsHighlighted' AND [object_id] = OBJECT_ID(N'Category')
)
BEGIN
    ALTER TABLE [Category] 
	ADD IsHighlighted bit NOT NULL DEFAULT(0)
END
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-15
-- Description:	Add column IsPaid, PaymentDate
--				on table POS_SaleOrder
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE ([name] = N'IsPaid' OR [name] = N'PaymentDate')
	AND [object_id] = OBJECT_ID(N'POS_SaleOrder')
)
BEGIN
    ALTER TABLE [POS_SaleOrder] 
	ADD 
		IsPaid bit NOT NULL DEFAULT(0),
		PaymentDate smalldatetime NULL
END
GO

UPDATE [POS_SaleOrder]
	SET IsPaid = 1
	,PaymentDate = CreatedDate

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-10-21
-- Description:	Add column IsSystemCat
--				on table Category
-- =============================================
IF NOT EXISTS(
	SELECT * FROM sys.columns 
	WHERE ([name] = N'IsSystemCat')
	AND [object_id] = OBJECT_ID(N'Category')
)
BEGIN
    ALTER TABLE [Category] 
	ADD 
		IsSystemCat bit NOT NULL DEFAULT(0)
END
GO