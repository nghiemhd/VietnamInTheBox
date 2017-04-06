using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Zit.Core;
using Zit.DataTransferObjects;

namespace Zit.ServiceLibs
{
    public static class ResponseHelper
    {
        public static Response ToResponse(this BusinessProcess process)
        {
            Response rp = new Response();
            rp.Infos = process.GetInfos();
            rp.Errors = process.GetErrors();
            return rp;
        }

        public static Response<T> ToResponse<T>(this BusinessProcess process, T data)
        {
            Response<T> rp = new Response<T>(data);
            rp.Infos = process.GetInfos();
            rp.Errors = process.GetErrors();
            return rp;
        }
    }
}
