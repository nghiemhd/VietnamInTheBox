IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'FN' AND name = 'ufn_GetDiscount')
	DROP FUNCTION [dbo].[ufn_GetDiscount]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetDiscount]
(
	@productId int
)
RETURNS int
AS
BEGIN

	declare @discount INT
	DECLARE @now AS DATETIME
	SET @now = GETUTCDATE()  

	set @discount = 0

	select 
		@discount = max(isnull(dc.DiscountPercentage,0))
	from Product p
		inner join Product_Category_Mapping m on p.Id = m.ProductId
		left join Discount_AppliedToCategories d on d.Category_Id = m.CategoryId
		left join Discount dc on d.Discount_Id = dc.Id
		left join Category c on m.CategoryId = c.Id
	where p.id = @productId
		and c.Published = 1 and c.Deleted = 0
		AND (dc.StartDateUtc IS NULL OR dc.StartDateUtc <= @now)
		AND (dc.EndDateUtc IS NULL OR dc.EndDateUtc >= @now)
	
	return isnull(@discount,0)
END
GO