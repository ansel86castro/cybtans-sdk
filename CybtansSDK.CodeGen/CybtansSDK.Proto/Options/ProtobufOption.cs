#nullable enable

using CybtansSdk.Proto.AST;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CybtansSdk.Proto.Options
{
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }


    public class ProtobufOption
    {
        Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

        public ProtobufOption(OptionsType type)
        {
            Type = type;
            var props = GetType().GetProperties();

            foreach (var p in props)
            {
                var field = p.GetCustomAttribute<FieldAttribute>();
                if(field != null)
                {
                    properties.Add(field.Name, p);
                }
            }
        }

        public OptionsType Type { get; }

        public void Set(string name, object value)
        {
            if (!properties.TryGetValue(name, out var p))
            {
                throw new InvalidOperationException($"Property {name} not found");
            }

            if (value is ConstantExp c)
            {
                p.SetValue(this, c.Value);
            }
            else if (value is InitializerExp init)
            {
                ProtobufOption? instance = (ProtobufOption?)Activator.CreateInstance(p.PropertyType);
                if (instance == null)
                    throw new InvalidOperationException($"Type not found {p.PropertyType}");

                init.Initialize(instance);
            }
            else
            {
                p.SetValue(this, value);
            }
        }

        public void Set(IdentifierExpression path, object value)
        {
            ProtobufOption option = this;
            if(path.Left != null)
            {
                option = GetOption(path.Left);                
            }

            option.Set(path.Id, value);
        }

        public ProtobufOption GetOption(IdentifierExpression path)
        {
            if(path.Left == null)
            {
                return GetOption(path.Id);
            }

            var option = GetOption(path.Left);
            return option.GetOption(path.Id);
        }

        public ProtobufOption GetOption(string name)
        {
            if (!properties.TryGetValue(name, out var p))
                throw new InvalidOperationException($"Property {name} not found");

            var value = (ProtobufOption?)p.GetValue(this);
            if (value == null)
            {
                value = (ProtobufOption?)Activator.CreateInstance(p.PropertyType);
                if (value == null)
                    throw new InvalidOperationException($"Type not found {p.PropertyType}");
                p.SetValue(this, value);
            }

            return value;
        }
    }
}
