using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Zit.Utils;
using Zit.BusinessObjects.SqlResultModel;
using Zit.DataTransferObjects;
using Zit.BusinessObjects.Enums;
using Zit.Client.Wpf.Infractstructure;
using Zit.BusinessObjects.BusinessModels;

namespace Zit.Client.Wpf
{
    public static class Map
    {
        public static void Boot()
        {
            #region Convert Version

            Mapper.CreateMap<byte[], string>().ConvertUsing(binary => binary.ToBase64());

            Mapper.CreateMap<string, byte[]>().ConvertUsing(str => str.ToByte());

            #endregion

            Mapper.CreateMap<POSSaleOrderDetailModel, POSSaleOrderDetailModel>();
        }
    }
}
