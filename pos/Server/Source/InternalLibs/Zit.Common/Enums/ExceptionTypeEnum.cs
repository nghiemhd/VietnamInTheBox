using System.Runtime.Serialization;

namespace Zit.Common.Enums
{
    [DataContract]
    public enum ExceptionTypeEnum
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Business = 1,

        [EnumMember]
        Security = 2,

        [EnumMember]
        Unknown = 3
    }
}