SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-24
-- Description:	Update POS_SaleOrder
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateSaleOrder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_UpdateSaleOrder]
GO
CREATE PROCEDURE usp_UpdateSaleOrder 
	@Id int,
	@IsPaid bit,
	@PaymentDate smalldatetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE POS_SaleOrder
	SET IsPaid = @IsPaid
	,PaymentDate = @PaymentDate
	WHERE Id = @Id
END
GO
