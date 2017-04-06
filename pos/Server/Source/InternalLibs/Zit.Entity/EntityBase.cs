using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace Zit.Entity
{
    [DataContract(IsReference=true)]
    public class EntityBase : IEntity {
        /// <summary>
        /// Property to Check Entity is Updated By Call Update Method In Repository
        /// </summary>
        [IgnoreDataMember]
        public bool UpdateMethodCalled = false;

        [IgnoreDataMember]
        private PropertyInfo[] _propertyInfos = null;

        public override string ToString()
        {
            if (_propertyInfos == null)
                _propertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();
            foreach (var info in _propertyInfos)
            {
                var ret = info.GetValue(this, null);
                sb.AppendLine(info.Name + ": " + (ret != null ? ret.ToString() : ""));
            }
            return sb.ToString();
        }
    }
}
