using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EFMVC.Core.Common
{
    public class IgnoreErrorPropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var props = new string[] { "InputStream", "Filter", "Length", "Position", "ReadTimeout", "WriteTimeout", "LastActivityDate", "LastUpdatedDate", "Session" };
            if (props.Contains(property.PropertyName)) { property.Ignored = true; }
            return property;
        }
    }
}
