using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsComputingApp.src.utils
{
    public static class EnumUtils
    {
        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            var enumType = enumerationValue.GetType();
            var memberInfos = enumType.GetMember(enumerationValue.ToString());
            if (memberInfos.Length > 0)
            {
                var attributes = memberInfos[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
    }
}
