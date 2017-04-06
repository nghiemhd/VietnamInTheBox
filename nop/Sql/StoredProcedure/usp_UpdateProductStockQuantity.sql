SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-07-09
-- Description:	Update product stock quantity 
--				after update IVM_Product_Ref
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateProductStockQuantity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateProductStockQuantity]
GO

CREATE PROCEDURE usp_UpdateProductStockQuantity 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    merge Product as dest
	using (
		select ProductId, SUM(Qty) as Qty
		from IVM_Product_Ref
		group by ProductId
		having SUM(Qty) <= 0
	) as src
	on dest.Id = src.ProductId
	when matched then 
		update set ManageInventoryMethodId = 1 --Track inventory for product
				  ,StockQuantity = 0;

	merge Product as dest
	using (
		select ProductId, SUM(Qty) as Qty
		from IVM_Product_Ref
		group by ProductId
		having SUM(Qty) > 0
	) as src
	on dest.Id = src.ProductId
	when matched then 
		update set ManageInventoryMethodId = 0; --Don't track inventory
				  
END
GO
