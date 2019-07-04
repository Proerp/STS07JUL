using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Global.Class.Library
{
    public static class GlobalDefaultValue
    {
        #region This slightly method ignores base class properties
        static public void Apply(object self)
        {
            if (self == null)
                return;

            Type baseType = self.GetType().BaseType;
            PropertyInfo[] baseProps = baseType != null ? baseType.GetProperties() : new PropertyInfo[0];

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(self))
            {
                bool skip = false;

                foreach (PropertyInfo info in baseProps)
                    if (prop.Name == info.Name)
                    {
                        skip = true;
                        break;
                    }

                if (skip)
                    continue;

                DefaultValueAttribute attr = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

                if (attr == null)
                    continue;

                prop.SetValue(self, attr.Value);
            }

        }
        #endregion


        #region The following backup apply to all property, including base clase
        //static public void ApplyDefaultValues(object self)
        //{
        //     foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(self)) {
        //         DefaultValueAttribute attr = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
        //         if (attr == null) continue;
        //         prop.SetValue(self, attr.Value);
        //     }
        //}

        #endregion


    }

}
