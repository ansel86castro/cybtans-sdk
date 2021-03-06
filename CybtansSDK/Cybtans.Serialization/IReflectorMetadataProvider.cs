﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Serialization
{
    public interface IReflectorMetadataProvider
    {
        IReflectorMetadata GetAccesor();
    }

    public interface IReflectorMetadata
    {
        int[] GetPropertyCodes();

        object GetValue(object target, int propertyCode);

        void SetValue(object target, int propertyCode, object value);

        Type GetPropertyType(int propertyCode);

        string GetPropertyName(int propertyCode);

        int GetPropertyCode(string propertyName);
    }
}
