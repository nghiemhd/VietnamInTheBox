SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang	
-- Create date: 2014-07-20
-- Description:	Update size of product after update IVM_Product_Ref
--				Don't display size if out of stock
--				Display if stock
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateProductSize]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateProductSize]
GO

CREATE PROCEDURE usp_UpdateProductSize
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	merge ProductVariantAttributeValue as dest
	using (
		select distinct ProductVariantAttributeValueId from(
			select ref.BarcodeId, bd.ProductVariantAttributeValueId, ref.Qty, pav.Name, pav.DisplayOrder 		
			from (
				select BarcodeId, SUM(Qty) as Qty 
				from IVM_Product_Ref
				group by BarcodeId
				having SUM(Qty) > 0
			) ref
			join IVM_BarcodeDetail bd 
			on ref.BarcodeId = bd.BarcodeId
			join ProductVariantAttributeValue pav 
			on bd.ProductVariantAttributeValueId = pav.Id
			join Product_ProductAttribute_Mapping pa
			on pav.ProductVariantAttributeId = pa.Id
			where pa.ProductAttributeId = 1 --color
			or pa.ProductAttributeId = 7 --size
			or pa.ProductAttributeId = 9 --volume
		)T
	) as src
	on dest.Id = src.ProductVariantAttributeValueId
	when matched then
		update set DisplayOrder = 0;

	merge ProductVariantAttributeValue as dest
	using (
		select distinct ProductVariantAttributeValueId from(
			select ref.BarcodeId, bd.ProductVariantAttributeValueId, ref.Qty, pav.Name, pav.DisplayOrder 		
			from (
				select BarcodeId, SUM(Qty) as Qty 
				from IVM_Product_Ref
				group by BarcodeId
				having SUM(Qty) <= 0
			) ref
			join IVM_BarcodeDetail bd 
			on ref.BarcodeId = bd.BarcodeId
			join ProductVariantAttributeValue pav 
			on bd.ProductVariantAttributeValueId = pav.Id
			join Product_ProductAttribute_Mapping pa
			on pav.ProductVariantAttributeId = pa.Id
			where pa.ProductAttributeId = 1 --color
			or pa.ProductAttributeId = 7 --size
			or pa.ProductAttributeId = 9 --volume
		)T
	) as src
	on dest.Id = src.ProductVariantAttributeValueId
	when matched then
		update set DisplayOrder = -1;
END
GO
