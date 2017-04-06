SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-07-16
-- Description:	Delete all products by categoryId
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DeleteAllProductsByCategoryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_DeleteAllProductsByCategoryId]
GO

CREATE PROCEDURE usp_DeleteAllProductsByCategoryId
	@categoryId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    delete Product_Category_Mapping 
    where CategoryId = @categoryId
END
GO
