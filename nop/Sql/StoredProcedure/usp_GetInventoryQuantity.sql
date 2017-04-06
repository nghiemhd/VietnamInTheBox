SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-07-08
-- Description:	Get inventory quantity
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetInventoryQuantity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetInventoryQuantity]
GO

CREATE PROCEDURE usp_GetInventoryQuantity
	@ProductId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @ProcessDate datetime
	set @ProcessDate = (select top 1 ProcessDate from IVM_Product_Ref)
	
    select SUM(Qty) as Quantity
	from (
		select ProductId, SUM(Qty) as Qty
		from IVM_Product_Ref
		where ProductId = @ProductId 
		AND (StoreId = 1 OR StoreId = 2 OR StoreId = 0)
		group by ProductId
		UNION
		select ProductId, SUM(Qty) as Qty
		from IVM_Transaction
		where ProductId = @ProductId 
		AND (StoreId = 1 OR StoreId = 2 OR StoreId = 2)
		AND TransDate > @ProcessDate
		group by ProductId
	) T
	group by ProductId
END
GO
