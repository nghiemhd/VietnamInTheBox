SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[usp_SaleOrderSearch]
(
	@fromDate DATETIME,
	@toDate DATETIME,
	@storeId INT,
	@status INT,
	@orderNo VARCHAR(50),
	@refNo VARCHAR(50),
	@customerName NVARCHAR(100),
	@mobile VARCHAR(50),
	@productCode NVARCHAR(400),
	@barcode VARCHAR(50),
	@desc NVARCHAR(max),
	@carrierId INT,
	@chanelId INT,
	@sourceId INT,
	@returnReasonId INT,
	@shippingCode varchar(50),
	@paymentType INT,
	@paymentStatus INT,
	@pageIndex INT,
	@pageSize INT,
	@count INT OUTPUT,
	@sumSubTotal DECIMAL OUTPUT,
	@sumDiscount DECIMAL OUTPUT,
	@sumShippingFee DECIMAL OUTPUT,
	@sumAeroShippingFee DECIMAL OUTPUT,
	@sumAmount DECIMAL OUTPUT,
	@sumCostAmount DECIMAL OUTPUT,
	@sumQty INT OUTPUT
)
AS 

SELECT	@count = COUNT(1), 
		@sumSubTotal = ISNULL(SUM(s.SubTotal),0),
		@sumDiscount = -ISNULL(SUM(s.Discount),0),
		@sumShippingFee = ISNULL(SUM(s.ShippingFee),0),
		@sumAeroShippingFee = ISNULL(SUM(s.AeroShippingFee),0), 
		@sumAmount = ISNULL(SUM(s.Amount),0),
		@sumCostAmount = ISNULL(SUM(-t.CostAmount),0),
		@sumQty = ISNULL(-SUM(t.Qty),0)
FROM POS_SaleOrder s INNER JOIN (SELECT 
				RefId,
				SUM(Qty) AS Qty,
				SUM(CostAmount) AS CostAmount
			FROM dbo.IVM_Transaction WHERE TypeId = 2
			GROUP BY RefId) t ON s.Id = t.RefId
WHERE 
	(@storeId IS NULL OR s.StoreId = @storeId) AND
    (@carrierId IS NULL OR s.CarrierId = @carrierId) AND
	(@fromDate IS NULL OR OrderDate >= @fromDate) AND
    (@toDate IS NULL OR OrderDate <= @toDate) AND
    (@status IS NULL OR [Status] = @status) AND  
    (@chanelId IS NULL OR [ChanelId] = @chanelId) AND  
    (@sourceId IS NULL OR s.SourceId = @sourceId) AND  
    (@returnReasonId IS NULL OR s.ReturnReasonId = @returnReasonId) AND  
	(@orderNo IS NULL OR OrderNo = @orderNo) AND  
	(@refNo IS NULL OR RefNo LIKE ('%' + @refNo + '%')) AND  
	(@customerName IS NULL OR CustomerName LIKE ('%' + @customerName + '%')) AND
	(@mobile IS NULL OR Mobile LIKE ('%' + @mobile + '%')) AND  
	(@desc IS NULL OR [Desc] LIKE ('%' + @desc + '%')) AND  
	(@shippingCode IS NULL OR s.ShippingCode LIKE ('%' + @shippingCode + '%')) AND
	(@PaymentType IS NULL OR s.IsMasterCard = @PaymentType) AND
	(@paymentStatus IS NULL OR s.IsPaid = @paymentStatus) AND
	(@productCode IS NULL OR 
				EXISTS(SELECT TOP 1 dt.Id
					FROM dbo.POS_SaleOrderDetail dt INNER JOIN dbo.IVM_Barcode b ON dt.BarcodeId = b.Id 
					INNER JOIN dbo.Product p ON b.ProductId = p.Id WHERE dt.SaleOrderId = s.Id AND p.SKU = @productCode)) AND
	(@barcode IS NULL OR 
				EXISTS(SELECT TOP 1 dt.Id
					FROM dbo.POS_SaleOrderDetail dt INNER JOIN dbo.IVM_Barcode b ON dt.BarcodeId = b.Id 
					INNER JOIN dbo.Product p ON b.ProductId = p.Id WHERE dt.SaleOrderId = s.Id AND b.Barcode = @barcode))

;WITH CTE AS
(SELECT
	Row_Number() OVER (ORDER BY s.Id DESC) as rowNum,
	s.Id,
	s.OrderNo,
	s.OrderDate,
	s.[Desc],
	s.CustomerName,
	s.Mobile,
	s.ChanelId,
	s.RefNo,
	s.SubTotal,
	s.ShippingFee,
	s.Discount,
	s.DiscountAmount,
	s.DiscountPercent,
	s.Amount,
	s.ReceiveMoney,
	s.ReturnMoney,
	s.CreatedUser,
	s.CreatedDate,
	s.UpdatedUser,
	s.UpdatedDate,
	s.[Status],
	st.Code AS StoreCode,
	c.Code AS CarrierCode,
	s.ShippingCode,
	s.AeroShippingFee,
	s.PrintCount,
	s.IsMasterCard,
	s.IsPaid,
	s.PaymentDate,
	s.Users,
	t.CostAmount,
	t.Qty,
	ss.Name AS SalesSource,
	rr.Name AS SalesReturnReason
FROM dbo.POS_SaleOrder s INNER JOIN dbo.CF_Store st ON s.StoreId = st.Id
	LEFT JOIN dbo.CF_Carrier c ON s.CarrierId = c.Id
	LEFT JOIN dbo.CF_SaleSource ss ON s.SourceId = ss.Id
	LEFT JOIN dbo.CF_SaleReturnReason rr ON s.ReturnReasonId = rr.Id
	INNER JOIN (SELECT 
				RefId,
				SUM(Qty) AS Qty,
				SUM(CostAmount) AS CostAmount
			FROM dbo.IVM_Transaction WHERE TypeId = 2
			GROUP BY RefId) t ON s.Id = t.RefId
WHERE 
	(@storeId IS NULL OR s.StoreId = @storeId) AND
	(@carrierId IS NULL OR s.CarrierId = @carrierId) AND  
	(@fromDate IS NULL OR OrderDate >= @fromDate) AND
    (@toDate IS NULL OR OrderDate <= @toDate) AND
	(@status IS NULL OR [Status] = @status) AND  
	(@chanelId IS NULL OR [ChanelId] = @chanelId) AND    
	(@sourceId IS NULL OR s.SourceId = @sourceId) AND  
    (@returnReasonId IS NULL OR s.ReturnReasonId = @returnReasonId) AND  
	(@orderNo IS NULL OR OrderNo = @orderNo) AND    
	(@refNo IS NULL OR RefNo LIKE ('%' + @refNo + '%')) AND  
	(@customerName IS NULL OR CustomerName LIKE ('%' + @customerName + '%')) AND
	(@mobile IS NULL OR Mobile LIKE ('%' + @mobile + '%')) AND  
	(@desc IS NULL OR [Desc] LIKE ('%' + @desc + '%')) AND  
	(@shippingCode IS NULL OR s.ShippingCode LIKE ('%' + @shippingCode + '%')) AND
	(@PaymentType IS NULL OR s.IsMasterCard = @PaymentType) AND
	(@paymentStatus IS NULL OR s.IsPaid = @paymentStatus) AND
	(@productCode IS NULL OR 
				EXISTS(SELECT TOP 1 dt.Id
					FROM dbo.POS_SaleOrderDetail dt INNER JOIN dbo.IVM_Barcode b ON dt.BarcodeId = b.Id 
					INNER JOIN dbo.Product p ON b.ProductId = p.Id WHERE dt.SaleOrderId = s.Id AND p.SKU = @productCode)) AND
	(@barcode IS NULL OR 
				EXISTS(SELECT TOP 1 dt.Id
					FROM dbo.POS_SaleOrderDetail dt INNER JOIN dbo.IVM_Barcode b ON dt.BarcodeId = b.Id 
					INNER JOIN dbo.Product p ON b.ProductId = p.Id WHERE dt.SaleOrderId = s.Id AND b.Barcode = @barcode))
)

SELECT 
	TOP (@pageSize)
	Id,
	OrderNo,
	OrderDate,
	[Desc],
	CustomerName,
	Mobile,
	ChanelId,
	RefNo,
	SubTotal,
	ShippingFee,
	Discount,
	DiscountAmount,
	DiscountPercent,
	Amount,
	ReceiveMoney,
	ReturnMoney,
	CreatedUser,
	CreatedDate,
	UpdatedUser,
	UpdatedDate,
	[Status],
	StoreCode,
	CarrierCode,
	ShippingCode,
	AeroShippingFee,
	PrintCount,
	CostAmount,
	Qty,
	Users,
	IsMasterCard,
	SalesSource,
	SalesReturnReason,
	IsPaid,
	PaymentDate
FROM CTE WHERE rowNum > @pageIndex * @pageSize

