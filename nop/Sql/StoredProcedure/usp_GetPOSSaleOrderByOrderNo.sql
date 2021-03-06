SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[usp_GetPOSSaleOrderByOrderNo]
(
	@orderNo varchar(50)
)
AS
Select
	so.Id,
	so.StoreId,
	so.OrderNo,
	so.ChanelId,
	so.[Desc],
	so.Mobile,
	so.CustomerName,
	so.OrderDate,
	so.DiscountPercent,
	so.ShippingFee,
	so.RefNo,
	so.Amount,
	so.SubTotal,
	so.ReceiveMoney,
	so.ReturnMoney,
	so.DiscountAmount,
	so.Discount,
	so.CreatedUser,
	so.CarrierId,
	so.ObjId,
	so.AeroShippingFee,
	so.Status,
	so.Users,
	so.IsMasterCard,
	so.SourceId,
	so.ReturnReasonId,
	so.ShippingCode,
	so.IsPaid,
	so.PaymentDate
From POS_SaleOrder so
where so.OrderNo = @orderNo
