using Nop.Admin.Models.Aero87;
using Nop.Core;
using Nop.Core.Domain.Aero87;
using Nop.Core.Infrastructure;
using Nop.Services.Aero87;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.UI.Fluent;

namespace Nop.Admin.Controllers
{
    public static class Utils
    {
        public static DateTime GetMaxOfDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59, 999, datetime.Kind);
        }
        public static DateTime GetMinOfDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0, datetime.Kind);
        }

        public static DropDownListBuilder DropDownListForEnum<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            Type enumType, DropDownItem nullItem = null)
        {
            ModelMetadata modelMeta = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var result = htmlHelper.Telerik().DropDownListFor(expression);
            var list = TranslateEnum(enumType, modelMeta.Model);
            List<DropDownItem> listSource = new List<DropDownItem>();
            if (nullItem != null)
            {
                listSource.Add(nullItem);
            }
            listSource.AddRange(list.Select(m => new DropDownItem()
            {
                Selected = m.Item3,
                Text = m.Item2,
                Value = m.Item1
            }));

            result.BindTo(listSource);

            return result;
        }

        public static DropDownListBuilder DropDownListForEnum(this HtmlHelper htmlHelper,
            Type enumType, object selectedValue)
        {
            var result = htmlHelper.Telerik().DropDownList();
            var list = TranslateEnum(enumType, null);
            List<DropDownItem> listSource = new List<DropDownItem>();

            listSource.AddRange(list.Select(m => new DropDownItem()
            {
                Selected = m.Item1 == selectedValue.ToString(),
                Text = m.Item2,
                Value = m.Item1
            }));

            result.BindTo(listSource);

            return result;
        }

        public static List<Tuple<string, string, bool>> TranslateEnum(Type enumType, object enumValue)
        {
            List<Tuple<string, string, bool>> listEnumField = new List<Tuple<string, string, bool>>();
            Type type = enumType;
            foreach (var evalue in type.GetEnumValues())
            {
                var valueName = type.GetField(evalue.ToString());

                string displayLabel = "";
                DisplayAttribute displayAtt = valueName.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAtt != null)
                    displayLabel = displayAtt.GetName();
                listEnumField.Add(new Tuple<string, string, bool>(evalue.ToString(), displayLabel, (enumValue ?? "").ToString() == evalue.ToString()));
            }
            return listEnumField;
        }

    }

    public class Aero87Controller : BaseNopController
    {
        private readonly ISaleOrderService _saleOrderService;
        private readonly IInventoryService _inventoryService;
        private readonly IPurchaseInvoiceService _purchaseInvoiceService;
        private readonly IGeneralLedgerService _generalLedgerService;
        private readonly ICommonService _commonService;

        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _mediaService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workCtx;

        public Aero87Controller(ISaleOrderService saleOrderService, 
            IInventoryService inventoryService,
            IPurchaseInvoiceService purchaseInvoiceService,
            IGeneralLedgerService generalLedgerService,
            ICommonService commonService,
            IManufacturerService manufacturerService,
            ICategoryService categoryService,
            IPictureService mediaService,
            IPermissionService permissionService,
            IWorkContext workCtx
            )
        {
            this._saleOrderService = saleOrderService;
            this._inventoryService = inventoryService;
            this._purchaseInvoiceService = purchaseInvoiceService;
            this._generalLedgerService = generalLedgerService;
            this._commonService = commonService;
            this._mediaService = mediaService;
            this._permissionService = permissionService;
            this._manufacturerService = manufacturerService;
            this._categoryService = categoryService;
            this._workCtx = workCtx;
        }
        //
        // GET: /Aero87/

        #region Sale Order

        public ActionResult SaleOrders()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            ViewBag.IsAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);
            ViewBag.IsCheckSOPayment = _permissionService.Authorize(StandardPermissionProvider.Aero87CheckSOPayment);

            var model = new SaleOrderSearch() { FromDate = DateTime.Now, ToDate = DateTime.Now };

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả"});
            foreach (var m in _commonService.GetAllStore().Where(m => m.Id != -1))
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            model.Carriers = new List<SelectListItem>();
            model.Carriers.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllCarrier())
                model.Carriers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            model.SalesSources = new List<SelectListItem>();
            model.SalesSources.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllSaleSource())
                model.SalesSources.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            model.ReturnReasons = new List<SelectListItem>();
            model.ReturnReasons.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllSaleReturnReason())
                model.ReturnReasons.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult SaleOrdersList(GridCommand command, SaleOrderSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            model.Desc = nullIfEmpty(model.Desc);
            model.ShippingCode = nullIfEmpty(model.ShippingCode);
            model.CustomerName = nullIfEmpty(model.CustomerName);
            model.CustomerPhone = nullIfEmpty(model.CustomerPhone);
            model.ProductCode = nullIfEmpty(model.ProductCode);
            model.Barcode = nullIfEmpty(model.Barcode);

            if(model.FromDate != null)
                model.FromDate = model.FromDate.Value.GetMinOfDate();

            if (model.ToDate != null)
                model.ToDate = model.ToDate.Value.GetMaxOfDate();

            var gridModel = new GridModel();

            var isAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            var totalRecord = 0;
            decimal SumSubTotal = 0;
            decimal SumDiscount = 0;
            decimal SumShippingFee = 0;
            decimal SumAeroShippingFee = 0;
            decimal SumAmount = 0;
            decimal SumCostAmount = 0;
            int SumQty = 0;

            var searchCondition = new SalesOrderSearchDTO { 
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                StoreId = model.StoreId,
                PSStatus = (int?)model.PSStatus,
                OrderNo = model.OrderNo,
                RefNo = model.RefNo,
                CustomerName = model.CustomerName,
                CustomerPhone = model.CustomerPhone,
                ProductCode = model.ProductCode,
                Barcode = model.Barcode,
                Desc = model.Desc,
                CarrierId = model.CarrierId,
                SaleChanelId = (int?)model.SaleChanel,
                SourceId = model.SourceId,
                ReturnReasonId = model.ReturnReasonId,
                ShippingCode = model.ShippingCode,
                PaymentType = (int?)model.PaymentType,
                PaymentStatus = (int?)model.PaymentStatus
            };
            
            var rs = _saleOrderService.Search(
                searchCondition,
                command.Page - 1,
                command.PageSize, 
                out totalRecord,
                out SumSubTotal,
                out SumDiscount,
                out SumShippingFee,
                out SumAeroShippingFee,
                out SumAmount,
                out SumCostAmount,
                out SumQty
                );

            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            gridModel.Aggregates = new {
                SumSubTotal = SumSubTotal.ToString("N0"),
                SumDiscount = SumDiscount.ToString("N0"),
                SumShippingFee = SumShippingFee.ToString("N0"),
                SumAeroShippingFee = SumAeroShippingFee.ToString("N0"),
                SumAmount = SumAmount.ToString("N0"),
                SumAmountAfterAPVC = (SumAmount - SumAeroShippingFee).ToString("N0"),
                SumCostAmount = isAd?SumCostAmount.ToString("N0"):"0",
                SumGrossAmount = isAd ? ((SumAmount - SumAeroShippingFee - SumCostAmount).ToString("N0")) : "0",
                GrossTR = (isAd && (SumAmount != 0)) ? ((((SumAmount - SumAeroShippingFee - SumCostAmount) / SumAmount) * 100).ToString("N0") + " %") : "0",
                SumQty = SumQty.ToString("N0")
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult SaleOrderGridDetail(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var rs = _saleOrderService.GetSaleOrderDetail(id);

            var isAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            foreach (var p in rs)
            {
                var defaultProductPicture =  _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);

                if (!isAd)
                {
                    p.CostPrice = 0;
                    p.CostAmount = 0;
                }

            }
            gridModel.Data = rs;
            gridModel.Total = rs.Count();
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult SaleOrderGridUpdate(GridCommand command, IEnumerable<SaleOrdersResult> updated)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.Aero87Admin)) || 
                !(_permissionService.Authorize(StandardPermissionProvider.Aero87CheckSOPayment)))
                return AccessDeniedView();

            if(updated != null && updated.Count() > 0)
            {
                foreach(var item in updated)
                {
                    var order = new UpdatedSaleOrderDTO { 
                        Id = item.Id,
                        IsPaid = item.IsPaid,
                        PaymentDate = item.PaymentDate
                    };

                    _saleOrderService.UpdateSaleOrder(order);
                }
            }

            //return no data
            var gridModel = new GridModel();
            gridModel.Data = new List<SaleOrdersResult>();
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost]
        public ActionResult ApproveSaleOrder(SaleOrderApproveModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            _saleOrderService.SaleOrderApprove(model.OrderId, model.DebitAcctId, model.ObjId, model.FeeAcctId, model.FeeAmount, model.Desc, _workCtx.CurrentCustomer.Username);

            return Json(new {IsOke = true});
        }

        [HttpPost]
        public ActionResult UnPostSaleOrder(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            _saleOrderService.SaleOrderUnPost(id,_workCtx.CurrentCustomer.Username);

            return Json(new { IsOke = true });
        }

        private string nullIfEmpty(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            else return str;
        }

        #endregion

        #region WarehouseCount

        public ActionResult WarehouseCount()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            ViewBag.IsAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            var model = new WarehouseCountSearch();

            model.ToEndDate = DateTime.Now;

            model.AvailableManufacturers = new List<SelectListItem>();

            model.AvailableManufacturers.Add(new SelectListItem() { Text = "Tất cả", Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            model.AvailableManufacturers.Add(new SelectListItem() { Text = "Rổng", Value = "-1" });

            model.AvailableCategories = new List<SelectListItem>();
            model.AvailableCategories.Add(new SelectListItem() { Text = "Tất cả", Value = "0" });
            foreach (var c in _categoryService.GetAllCategories(showHidden: true))
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(_categoryService), Value = c.Id.ToString()});

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllStore())
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        protected List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var categoriesIds = new List<int>();
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            foreach (var category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id));
            }
            return categoriesIds;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult WarehouseCountList(GridCommand command, WarehouseCountSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            model.ProductCode = nullIfEmpty(model.ProductCode);

            var categoryIds = new List<int>() { model.SearchCategoryId };

            if (model.SearchCategoryId > 0)
                categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));


            var gridModel = new GridModel();

            int totalRecord = 0;
            int SumQty = 0;
            decimal SumCostAmount = 0;
            var rs = _inventoryService.WarehouseCount(
                model.StoreId,
                model.ToEndDate,
                categoryIds,
                model.SearchManufacturerId,
                model.OrderByAge,
                (int?)model.QtyFilter,
                model.ProductCode,
                command.Page - 1,
                command.PageSize,
                out totalRecord,
                out SumQty,
                out SumCostAmount
                );

            var isAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            foreach (var p in rs)
            {
                var defaultProductPicture = _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);
            }


            gridModel.Data = rs;
            gridModel.Total = totalRecord;

            if(!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
            {
                SumCostAmount = 0;
                foreach (var it in rs)
                {
                    it.CostAmount = 0;
                }
            }

            gridModel.Aggregates = new
            {
                SumQty = SumQty.ToString("N0"),
                SumCostAmount = SumCostAmount.ToString("N0")
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult WarehouseCountGridDetail(GridCommand command,WarehouseCountSearch model,int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var rs = _inventoryService.GetWarehouseCountDetail(id, model.StoreId, model.ToEndDate);
            gridModel.Data = rs;
            gridModel.Total = rs.Count();

            var ad = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            if (!ad)
            {
                foreach (var it in rs)
                {
                    it.CostAmount = 0;
                }
            }
            

            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        #endregion

        #region Inventory Tran

        public ActionResult InventoryTransaction()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            var model = new InventoryTransactionSearch();

            model.FromDate = DateTime.Now.Date;
            model.ToDate = DateTime.Now.Date;

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllStore())
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryTransactionList(GridCommand command, InventoryTransactionSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            model.ProductCode = nullIfEmpty(model.ProductCode);

            var gridModel = new GridModel();

            if (model.ToDate.HasValue)
            {
                model.ToDate = model.ToDate.Value.Date.AddDays(1);
            }

            int SumQty = 0;
            int totalRecord = 0;
            var rs = _inventoryService.GetInventoryTransaction(
                model.FromDate,
                model.ToDate,
                model.TranNo,
                model.ProductCode,
                model.StoreId,
                model.Barcode,
                (int?)model.TypeId,
                command.Page - 1,
                command.PageSize,
                out totalRecord,
                out SumQty
                );

            foreach (var p in rs)
            {
                var defaultProductPicture = _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);
            }

            gridModel.Data = rs;
            gridModel.Total = totalRecord;

            gridModel.Aggregates = new
            {
                SumQty = SumQty.ToString("N0")
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #region Inventory Transfer

        public ActionResult InventoryTransfer()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            ViewBag.CanConfirmInventoryTransfer = _permissionService.Authorize(StandardPermissionProvider.Aero87ConfirmIT);

            var model = new InventoryTransferSearch();

            model.FromDate = DateTime.Now.Date;
            model.ToDate = DateTime.Now.Date;

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllStore().Where(a => a.Id != 0))
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryTransferList(GridCommand command, InventoryTransferSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            model.ProductCode = nullIfEmpty(model.ProductCode);

            var gridModel = new GridModel();

            if (model.ToDate.HasValue)
            {
                model.ToDate = model.ToDate.Value.Date.AddDays(1);
            }

            int SumQty = 0;
            int totalRecord = 0;
            var rs = _inventoryService.GetInventoryTransfer(
                model.FromDate,
                model.ToDate,
                model.TranNo,
                model.ProductCode,
                model.FromStoreId,
                model.ToStoreId,
                model.Barcode,
                (int?)model.Status,
                command.Page - 1,
                command.PageSize,
                out totalRecord,
                out SumQty
                );

            gridModel.Data = rs;
            gridModel.Total = totalRecord;

            gridModel.Aggregates = new
            {
                SumQty = SumQty.ToString("N0")
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryTransferGridDetail(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var rs = _inventoryService.GetInventoryTransferDetail(id);

            foreach (var p in rs)
            {
                var defaultProductPicture = _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);
            }

            gridModel.Data = rs;
            gridModel.Total = rs.Count();
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        [HttpPost]
        public ActionResult ApproveTransfer(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            _inventoryService.IVMTranITComplete(id, (int)DocType.IT, _workCtx.CurrentCustomer.Username);

            return Json(new { IsOke = true });
        }

        //public ActionResult CreateInventoryTransfer()
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.Aero87CreateIT))
        //        return AccessDeniedView();

        //    var model = new CategoryModel.AddCategoryProductModel();
        //    //categories
        //    model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    foreach (var c in _categoryService.GetAllCategories(showHidden: true))
        //        model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(_categoryService), Value = c.Id.ToString() });

        //    //manufacturers
        //    model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
        //        model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

        //    //stores
        //    model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    foreach (var s in _storeService.GetAllStores())
        //        model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

        //    //vendors
        //    model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
        //        model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

        //    //product types
        //    model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
        //    model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

        //    //product status
        //    model.AvailableProductStatus = PublishStatus.Published.ToSelectList(false).ToList();
        //    model.AvailableProductStatus.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "-1" });

        //    return View(model);
        //}
        #endregion

        #region Purchase Invoices

        public ActionResult PurchaseInvoices()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            ViewBag.IsAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            var model = new PurchaseInvoiceSearch() { FromDate = null, ToDate = DateTime.Now };

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllStore())
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });


            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PurchaseInvoicesList(GridCommand command, PurchaseInvoiceSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            model.RefNo = nullIfEmpty(model.RefNo);
            model.ProductCode = nullIfEmpty(model.ProductCode);
            model.Barcode = nullIfEmpty(model.Barcode);

            if (model.FromDate != null)
                model.FromDate = model.FromDate.Value.GetMinOfDate();

            if (model.ToDate != null)
                model.ToDate = model.ToDate.Value.GetMaxOfDate();

            var gridModel = new GridModel();

            var totalRecord = 0;
            decimal SumAmount = 0;
            int SumQty = 0;
            var rs = _purchaseInvoiceService.Search(
                model.FromDate,
                model.ToDate,
                model.StoreId,
                (int?)model.PIStatus,
                model.InvoiceNo,
                model.RefNo,
                model.ProductCode,
                model.Barcode,
                command.Page - 1,
                command.PageSize,
                out totalRecord,
                out SumAmount,
                out SumQty
                );


            var isAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);
            if (!isAd)
            {
                foreach (var it in rs)
                {
                    it.Amount = 0;
                }
                SumAmount = 0;
            }

            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            gridModel.Aggregates = new
            {
                SumAmount = SumAmount.ToString("N0"),
                SumQty = SumQty.ToString("N0")
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PurchaseInvoiceGridDetail(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87POS))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var totalRecord = 0;
            var rs = _purchaseInvoiceService.GetPurchaseInvoiceDetail(id,
                command.Page - 1,
                command.PageSize,
                out totalRecord);

            var isAd = _permissionService.Authorize(StandardPermissionProvider.Aero87Admin);

            foreach (var p in rs)
            {
                var defaultProductPicture = _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);

                if (!isAd)
                {
                    p.Amount = 0;
                    p.Price = 0;
                }
            }
            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        [HttpPost]
        public ActionResult ApprovePurchaseInvoice(PurchaseInvoiceApproveModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            _purchaseInvoiceService.PurchaseInvoiceApprove(model.InvoiceId, model.CreditAcctId, model.ObjId, model.Desc, _workCtx.CurrentCustomer.Username);

            return Json(new { IsOke = true });
        }

        [HttpPost]
        public ActionResult UnPostPurchaseInvoice(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            _purchaseInvoiceService.PurchaseInvoiceUnPost(id, _workCtx.CurrentCustomer.Username);

            return Json(new { IsOke = true });
        }

        #endregion

        #region GJ

        public ActionResult GeneralLedgers()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var model = new GeneralLedgerSearch() { FromDate = null, ToDate = DateTime.Now };

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult GeneralLedgersList(GridCommand command, GeneralLedgerSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            model.AcctCode = nullIfEmpty(model.AcctCode);
            model.DocNo = nullIfEmpty(model.DocNo);
            model.ObjCode = nullIfEmpty(model.ObjCode);
            model.ObjName = nullIfEmpty(model.ObjName);

            if (model.FromDate != null)
                model.FromDate = model.FromDate.Value.GetMinOfDate();

            if (model.ToDate != null)
                model.ToDate = model.ToDate.Value.GetMaxOfDate();

            var gridModel = new GridModel();

            var totalRecord = 0;

            var rs = _generalLedgerService.Search(
                model.FromDate,
                model.ToDate,
                (int?)model.DocType,
                model.DocNo,
                model.AcctCode,
                model.ObjCode,
                model.ObjName,
                command.Page - 1,
                command.PageSize,
                out totalRecord
                );

            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            gridModel.Aggregates = new
            {
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult GeneralLedgerGridDetail(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var gridModel = new GridModel();

            var rs = _generalLedgerService.GetGeneralLedgerDetail(id);

            gridModel.Data = rs;
            gridModel.Total = rs.Count();
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult GeneralLedgerGridDetailEdit(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var gridModel = new GridModel();

            var obj = Session[getKeyGJ(id)] as GeneralLedgerResult;

            if (obj.Items == null)
                obj.Items = new List<GeneralLedgerLineResult>();
            gridModel.Data = obj.Items;
            gridModel.Total = obj.Items.Count();
            
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult GeneralLedgerGridDetailInsert(GeneralLedgerLineResult model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var obj = Session[getKeyGJ(model.TransId)] as GeneralLedgerResult;

            processLine(obj, model);

            GridModel gridModel = new GridModel();
            gridModel.Data = obj.Items;
            gridModel.Total = obj.Items.Count();

            return new JsonResult
            {
                Data = gridModel
            };
        }

        private void processLine(GeneralLedgerResult master,GeneralLedgerLineResult line)
        {
            //Check
            var acct = _generalLedgerService.getAcctById(int.Parse(line.AcctCode));
            line.AcctName = acct.AcctName;
            line.AcctCode = acct.AcctCode;
            line.AcctId = acct.Id;

            if (line.Debit != 0) line.Credit = 0;

            if (line.Credit != 0) line.Debit = 0;

            if (line.Debit == 0 && line.Credit == 0)
                return;

            if (acct.RequireObj)
            {
                if (string.IsNullOrWhiteSpace(line.ObjCode))
                {
                    return;
                }
                else
                {
                    var obj = _generalLedgerService.getObjById(int.Parse(line.ObjCode));
                    line.ObjCode = obj.ObjCode;
                    line.RefObjId = obj.Id;
                    line.ObjName = obj.ObjName;
                }
            }
            else
            {
                line.RefObjId = null;
                line.ObjCode = null;
                line.ObjName = null;
            }

            if (!master.Items.Any())
            {
                line.LineId = 1;
            }
            else
            {
                var lineid = master.Items.Max(m => m.LineId);
                var sum = master.Items.Where(m => m.LineId == lineid).Sum(m => m.Debit - m.Credit);
                if (sum != 0)
                {
                    line.LineId = lineid;
                }
                else
                    line.LineId = lineid + 1;
            }

            line.CreatedDate = DateTime.Now;
            line.UpdatedDate = DateTime.Now;
            line.CreatedUser = _workCtx.CurrentCustomer.Username;
            line.UpdatedUser = _workCtx.CurrentCustomer.Username;

            master.Items.Add(line);
        }

        public ActionResult EditGeneralLedger()
        {
            int? docId = null;

            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var model = _generalLedgerService.GetGeneralLedger(docId);
            if(model.Id != 0)
                model.Items = _generalLedgerService.GetGeneralLedgerDetail(model.Id);

            Session[getKeyGJ(model.Id)] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditGeneralLedger(GeneralLedgerResult model)
        {
            model.Id = 0;

            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var mem = Session[getKeyGJ(model.Id)] as GeneralLedgerResult;
            mem.Desc = model.Desc;
            mem.DocDate = model.DocDate;
            mem.CreatedDate = DateTime.Now;
            mem.UpdatedDate = DateTime.Now;
            mem.CreatedUser = _workCtx.CurrentCustomer.Username;
            mem.UpdatedUser = _workCtx.CurrentCustomer.Username;
            mem.TypeId = (int)DocType.GJ;

            if (!mem.Items.Any() || 
                mem.Items.GroupBy(m => m.LineId).Select(m => new { 
                LineId = m.Key, 
                Sum = m.Sum(a => a.Debit - a.Credit),
                CreditCount = m.Where(f => f.Credit != 0).Count(),
                DebitCount = m.Where(f => f.Debit != 0).Count()
            }).Any(a => a.Sum != 0 || (a.CreditCount != 1 && a.DebitCount != 1))
                )
            {
                ModelState.AddModelError("","Thông tin không hợp lệ");

                return View(mem);
            }

            var docId = _generalLedgerService.EditGeneralLedger(mem);

            return RedirectToAction("GeneralLedgers");
        }

        private string getKeyGJ(int modelId)
        {
            return string.Format("EditGeneralLedger{0}", modelId);
        }

        #endregion

        #region IVM - Adj

        public ActionResult InventoryAdjs()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var model = new InventoryAdjSearch() { FromDate = null, ToDate = DateTime.Now };

            model.Stores = new List<SelectListItem>();
            model.Stores.Add(new SelectListItem() { Text = "Tất cả" });
            foreach (var m in _commonService.GetAllStore())
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryAdjList(GridCommand command, InventoryAdjSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            model.AdjNo = nullIfEmpty(model.AdjNo);
            model.ProductCode = nullIfEmpty(model.ProductCode);
            model.Barcode = nullIfEmpty(model.Barcode);

            if (model.FromDate != null)
                model.FromDate = model.FromDate.Value.GetMinOfDate();

            if (model.ToDate != null)
                model.ToDate = model.ToDate.Value.GetMaxOfDate();

            var gridModel = new GridModel();

            var totalRecord = 0;
            var rs = _inventoryService.InventoryAdjSearch(
                model.FromDate,
                model.ToDate,
                model.StoreId,
                (int?)model.IAStatus,
                model.AdjNo,
                model.ProductCode,
                model.Barcode,
                command.Page - 1,
                command.PageSize,
                out totalRecord
                );

            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            gridModel.Aggregates = new
            {

            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryAdjGridDetail(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var rs = _inventoryService.GetInventoryAdjDetail(id);

            foreach (var p in rs)
            {
                var defaultProductPicture = _mediaService.GetPicturesByProductId(p.ProductId, 1).FirstOrDefault();
                p.PictureUrl = _mediaService.GetPictureUrl(defaultProductPicture, 75, true);
            }

            gridModel.Data = rs;
            gridModel.Total = rs.Count();
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        public ActionResult EditInventoryAdj()
        {
            int? adjId = null;

            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var model = _inventoryService.GetInventoryAdj(adjId);
            if(model.Id != 0)
                model.Items = _inventoryService.GetInventoryAdjDetail(model.Id);

            model.Stores = new List<SelectListItem>();
            foreach (var m in _commonService.GetAllStore())
                model.Stores.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            Session[getKeyIA(model.Id)] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditInventoryAdj(InventoryAdjResult model)
        {
            model.Id = 0;

            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var mem = Session[getKeyIA(model.Id)] as InventoryAdjResult;
            mem.Desc = model.Desc;
            mem.CreatedDate = DateTime.Now;
            mem.UpdatedDate = DateTime.Now;
            mem.AdjDate = model.AdjDate;
            mem.CreatedUser = _workCtx.CurrentCustomer.Username;
            mem.UpdatedUser = _workCtx.CurrentCustomer.Username;
            mem.Qty = mem.Items.Sum(m => m.Qty);
            mem.CostAmount = mem.Items.Sum(m => m.CostAmount);
            mem.DebitAcctId = model.DebitAcctId;
            mem.ObjId = model.ObjId;
            mem.StoreId = model.StoreId;

            var docId = _inventoryService.EditInventoryAdj(mem);

            return RedirectToAction("InventoryAdjs");
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InventoryAdjGridDetailEdit(GridCommand command, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var gridModel = new GridModel();

            var obj = Session[getKeyIA(id)] as InventoryAdjResult;

            if (obj.Items == null)
                obj.Items = new List<InventoryAdjLineResult>();
            gridModel.Data = obj.Items;
            gridModel.Total = obj.Items.Count();

            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult InventoryAdjGridDetailInsert(InventoryAdjLineResult model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var obj = Session[getKeyIA(model.AdjId)] as InventoryAdjResult;

            processLineIA(obj, model);

            GridModel gridModel = new GridModel();
            gridModel.Data = obj.Items;
            gridModel.Total = obj.Items.Count();

            return new JsonResult
            {
                Data = gridModel
            };
        }

        private string getKeyIA(int modelId)
        {
            return string.Format("EditInventoryAdj{0}", modelId);
        }

        private static Regex barcodeNamePattern = new Regex(@"^\[(\d*)\]",RegexOptions.Compiled);

        private static int getIdBarcodeName(string name)
        {
            var match = barcodeNamePattern.Match(name);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            else
                throw new InvalidOperationException();
        }

        private void processLineIA(InventoryAdjResult master, InventoryAdjLineResult line)
        {
            var acct = _inventoryService.GetBarcodeById(getIdBarcodeName(line.FullName));

            line.FullName = acct.FullName;
            line.BarcodeId = acct.Id;

            if (line.Qty != 0)
            {
                line.CostAmount = 0;
            }


            if (line.Qty == 0 && line.CostAmount == 0)
            {
                throw new HttpException("Dữ liệu không hợp lệ");
            }

            if (master.Items != null && master.Items.Any())
            {
                line.Seq = master.Items.Max(m => m.Seq) + 1;
            }
            else line.Seq = 1;

            master.Items.Add(line);
        }

        #endregion

        #region Ajax Combobox

        [HttpPost]
        public ActionResult AcctAjax(string text)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var rs = _generalLedgerService.AcctAjax(text);

            return new JsonResult { Data = new SelectList(rs, "Id", "AcctFull") };
        }

        [HttpPost]
        public ActionResult ObjAjax(string text)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var rs = _generalLedgerService.ObjAjax(text);

            return new JsonResult { Data = new SelectList(rs, "Id", "ObjFull") };
        }

        [HttpPost]
        public ActionResult BarcodeAjax(string text)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();
            var rs = _inventoryService.BarcodeAjax(text);

            return new JsonResult { Data = new SelectList(rs, "FullName", "FullName") };
        }

        #endregion

        #region Account Balance

        public ActionResult AccountBalance()
        {
            var now = DateTime.Now;
            return View(new AccountBalanceSearch() { FromDate = now.Date.AddDays(-6), ToDate = now.GetMaxOfDate() });
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AccountBalanceList(GridCommand command, AccountBalanceSearch model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            model.FromDate = model.FromDate.GetMinOfDate();
            model.ToDate = model.ToDate.GetMaxOfDate();
               

            var gridModel = new GridModel();


            var rs = _generalLedgerService.GetAccountBalance(
                model.FromDate,
                model.ToDate,
                model.ObjId
                );

            gridModel.Data = rs;
            gridModel.Total = rs.Count();
            gridModel.Aggregates = new
            {
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AccountBalanceGridDetail(GridCommand command, AccountBalanceSearch model, int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.Aero87Admin))
                return AccessDeniedView();

            var gridModel = new GridModel();
            var totalRecord = 0;
            var rs = _generalLedgerService.GetAcctTransactionDetail(
                model.FromDate.Date,
                model.ToDate.Date.AddDays(1),
                id,
                model.ObjId,
                command.Page - 1,
                command.PageSize,
                out totalRecord
                );

            gridModel.Data = rs;
            gridModel.Total = totalRecord;
            return new JsonResult
            {
                Data = gridModel
            }; ;
        }

        #endregion
    }
}
