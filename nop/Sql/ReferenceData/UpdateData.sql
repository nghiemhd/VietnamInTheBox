-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-07-02
-- Description:	Insert Product status in table
--				LocaleStringResource				
-- =============================================
if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Catalog.Products.List.SearchProductStatus'
			  and LanguageId = 1)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (1, 'Admin.Catalog.Products.List.SearchProductStatus', 'Product status')
end

if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Catalog.Products.List.SearchProductStatus'
			  and LanguageId = 2)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (2, 'Admin.Catalog.Products.List.SearchProductStatus', 'Product status')
end

if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Catalog.Products.List.SearchProductStatus.Hint'
			  and LanguageId = 1)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (1, 'Admin.Catalog.Products.List.SearchProductStatus.Hint', 'Search by a product status.')
end

if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Catalog.Products.List.SearchProductStatus.Hint'
			  and LanguageId = 2)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (2, 'Admin.Catalog.Products.List.SearchProductStatus.Hint', 'Search by a product status.')
end
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-08-30
-- Description:	Add permission Aero87_CheckSOPayment				
-- =============================================
if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'Aero87_CheckSOPayment')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Aero87 Check SO Payment', 'Aero87_CheckSOPayment', 'Configuration')
end

if not exists(select 1 from CustomerRole r
			  where r.Name = 'CheckSOPayment')
begin			  
	insert CustomerRole(Name, FreeShipping, TaxExempt, Active, IsSystemRole, SystemName)
	values('CheckSOPayment', 0, 0, 1, 0, 'CheckSOPayment')
end
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-10-21
-- Description:	Add permission 
--				- EditSystemCat
--				- DeleteProducts
--				- PublishProducts
--				- Aero87_CreateInventoryTransfer
-- =============================================
if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'EditSystemCat')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Admin area. Alow edit system cat','EditSystemCat','Catalog')
end

if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'DeleteProducts')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Admin area. Delete Products','DeleteProducts','Catalog')
end

if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'PublishProducts')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Admin area. Publish Products','PublishProducts','Catalog')
end

if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'Aero87_CreateInventoryTransfer')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Aero87 Create Inventory Transfer','Aero87_CreateInventoryTransfer','Configuration')
end
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2014-10-29
-- Description:	Insert DeleteSelectedConfirmation in table
--				LocaleStringResource				
-- =============================================
if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Common.DeleteSelectedConfirmation'
			  and LanguageId = 1)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (1, 'Admin.Common.DeleteSelectedConfirmation', 'Are you sure you want to delete selected item(s)?')
end

if not exists(select 1 from LocaleStringResource 
			  where ResourceName = 'Admin.Common.DeleteSelectedConfirmation'
			  and LanguageId = 2)
begin
	insert LocaleStringResource(LanguageId, ResourceName, ResourceValue)
	values (2, 'Admin.Common.DeleteSelectedConfirmation', 'Are you sure you want to delete selected item(s)?')
end
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2015-09-08
-- Description:	Add permission Aero87_EditPrice				
-- =============================================
if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'Aero87_EditPrice')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Aero87 Edit Product Price', 'Aero87_EditPrice', 'Configuration')
end

if not exists(select 1 from CustomerRole r
			  where r.Name = 'EditPrice')
begin			  
	insert CustomerRole(Name, FreeShipping, TaxExempt, Active, IsSystemRole, SystemName)
	values('EditPrice', 0, 0, 1, 0, 'EditPrice')
end
GO

-- =============================================
-- Author:		Nghiem Hoang
-- Create date: 2015-09-25
-- Description:	Add permission Aero87_EditProduct				
-- =============================================
if not exists(select 1 from PermissionRecord p
			  where p.SystemName = 'Aero87_EditProduct')
begin			  
	insert PermissionRecord(Name, SystemName, Category)
	values('Aero87 Edit Product', 'Aero87_EditProduct', 'Configuration')
end

if not exists(select 1 from CustomerRole r
			  where r.Name = 'EditProduct')
begin			  
	insert CustomerRole(Name, FreeShipping, TaxExempt, Active, IsSystemRole, SystemName)
	values('EditProduct', 0, 0, 1, 0, 'EditProduct')
end
GO