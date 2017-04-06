SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-06-26
-- Description:	Import data into IVM_Product_Ref
--				from IVM_Transaction
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Import_IVM_Product_Ref]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Import_IVM_Product_Ref]
GO

CREATE PROCEDURE usp_Import_IVM_Product_Ref
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @ProcessDate DateTime
	declare @NewProcessDate DateTime

	if not exists(select 1 from IVM_Product_Ref)
	begin
		-- Import all data
		set @ProcessDate = GETDATE()

		insert IVM_Product_Ref(ProductId, BarcodeId, StoreId, Qty, ProcessDate) 
		select ProductId, BarcodeId, StoreId, SUM(Qty) as Qty, @ProcessDate 
		from IVM_Transaction 
		where TransDate <= @ProcessDate
		group by ProductId, BarcodeId, StoreId
	end
	else
	begin
		select top 1 @ProcessDate = ProcessDate from IVM_Product_Ref
		set @NewProcessDate = GETDATE()
		merge IVM_Product_Ref as dest
		using (
			select ProductId, BarcodeId, StoreId, SUM(Qty) as Qty
			from IVM_Transaction 
			where TransDate > @ProcessDate
			group by ProductId, BarcodeId, StoreId
		) as src
		on dest.ProductId = src.ProductId
		and dest.BarcodeId = src.BarcodeId
		and dest.StoreId = src.StoreId
		when matched then 
			update set Qty = dest.Qty + src.Qty
		when not matched by target then
			insert (ProductId, BarcodeId, StoreId, Qty, ProcessDate)
			values(src.ProductId, src.BarcodeId, src.StoreId, src.Qty, @NewProcessDate);

		update IVM_Product_Ref
		set ProcessDate = GETDATE()

		delete IVM_Product_Ref
		where not exists (select 1 from IVM_Transaction
						  where ProductId = IVM_Product_Ref.ProductId
						  and BarcodeId = IVM_Product_Ref.BarcodeId
						  and StoreId = IVM_Product_Ref.StoreId)
	end
	
END
GO
