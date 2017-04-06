SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-21
-- Description:	Retrieve stores where product is
--				stocking				
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetStoreProductStocking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetStoreProductStocking]
GO

CREATE PROCEDURE usp_GetStoreProductStocking
	@ProductId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @ProcessDate datetime
	set @ProcessDate = (select top 1 ProcessDate from IVM_Product_Ref)
	
    select distinct s.Id, s.Name
	from
	(
		select ProductId, StoreId, SUM(Qty) as Qty
		from (
			select ProductId, StoreId, SUM(Qty) as Qty
			from IVM_Product_Ref
			where ProductId = @ProductId 
			AND (StoreId = 1 OR StoreId = 2)
			group by ProductId, StoreId
			UNION
			select ProductId, StoreId, SUM(Qty) as Qty
			from IVM_Transaction
			where ProductId = @ProductId 
			AND (StoreId = 1 OR StoreId = 2)
			AND TransDate > @ProcessDate
			group by ProductId, StoreId
		) T
		group by ProductId, StoreId
		having SUM(Qty) > 0
	) IVM
	join CF_Store s
	on IVM.StoreId = s.Id
	where s.Id = 1 OR s.Id = 2
END
GO
