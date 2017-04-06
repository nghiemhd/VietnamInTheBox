using System;
using AutoMapper;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Configurations;
using Zit.DataTransferObjects;
using Zit.Utils;

namespace Zit.Services.Maps
{
    public static class Map
    {
        public static void Boot()
        {
            #region Convert Version

            Mapper.CreateMap<byte[], string>().ConvertUsing(binary => binary.ToBase64());

            Mapper.CreateMap<string, byte[]>().ConvertUsing(str => str.ToByte());

            #endregion

            Mapper.CreateMap<AppConfig, AppConfigClient>();

            Mapper.CreateMap<usp_CreatePOSDetailByBarcode, POSSaleOrderDetailModel>();

            Mapper.CreateMap<POSSaleOrderModel, POS_SaleOrder>();

            Mapper.CreateMap<POSSaleOrderDetailModel, POS_SaleOrderDetail>().ForMember(m => m.IVM_Barcode,o => o.Ignore());

            Mapper.CreateMap<usp_GetPOSSaleOrderByOrderNo, POSSaleOrderModel>();
            Mapper.CreateMap<usp_GetPOSSaleOrderDetailBySaleOrderId, POSSaleOrderDetailModel>();
            Mapper.CreateMap<usp_CreateITDetailByBarcode, InvenrotyTransferDetailModel>();

            Mapper.CreateMap<InventoryTransferModel, IVM_InvTransfer>();
            Mapper.CreateMap<InvenrotyTransferDetailModel, IVM_InvTransferDetail>().ForMember(m => m.IVM_Barcode, o => o.Ignore());
            Mapper.CreateMap<usp_GetInvTransferByTransferNo, InventoryTransferModel>();
            Mapper.CreateMap<usp_GetInvTransferDetailByTransferId, InvenrotyTransferDetailModel>();
        }
    }
}