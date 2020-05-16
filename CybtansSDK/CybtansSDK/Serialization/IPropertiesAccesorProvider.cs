using System;
using System.Collections.Generic;
using System.Text;

namespace CybtansSdk.Serialization
{
    public interface IPropertiesAccesorProvider
    {
        IPropertiesAccesor GetAccesor();
    }

    public interface IPropertiesAccesor
    {
        int[] GetPropertyCodes();

        object GetValue(object target, int propertyCode);

        void SetValue(object target, int propertyCode, object value);

        Type GetPropertyType(int propertyCode);
    }
}
