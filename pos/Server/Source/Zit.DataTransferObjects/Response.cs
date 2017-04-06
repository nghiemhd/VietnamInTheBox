using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zit.DataTransferObjects
{
    [DataContract(IsReference=true)]
    public class Response
    {
        [IgnoreDataMember]
        public bool HasError
        {
            get
            {
                if (Errors != null && Errors.Any())
                {
                    return true;
                }
                else
                    return false;
            }
        }
        [IgnoreDataMember]
        public bool HasInfo
        {
            get
            {
                if (Infos != null && Infos.Any())
                {
                    return true;
                }
                else
                    return false;
            }
        }
        [DataMember]
        public List<object> Infos { get; set; }
        [DataMember]
        public List<object> Errors { get; set; }

        public string ToErrorMsg()
        {
            if (HasError)
            {
                return Errors.Aggregate((a, b) =>
                {
                    return (a ?? "").ToString() + "\n" + (b ?? "").ToString();
                }).ToString();
            }
            return null;
        }

        public string ToInfoMsg()
        {
            if (HasInfo)
            {
                return Infos.Aggregate((a, b) =>
                {
                    return a.ToString() + "\n" + b.ToString();
                }).ToString();
            }
            return null;
        }


        public static Response<T> FromData<T>(T data)
        {
            Response<T> rp = new Response<T>(data);
            return rp;
        }
    }

    [DataContract]
    public class Response<T> : Response
    {
        public Response()
        {

        }

        public Response(T data)
        {
            Data = data;
        }
        [DataMember]
        public T Data { get; set; }
    }
}
