using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cybtans.Expressions.Ast
{
    public interface IASTContext
    {
        void AddVariable<T>(string name, T value);

        void AddVariable<T>(string name, Func<T> resolver);

        FunctionDeclaration GetFunctionDeclaration(string name);

        FunctionDeclaration GetFunctionDeclaration(Type netType, string name);

        VariableDeclaration GetVariableDeclaration(string name);
    }

    public class ASTContext : IASTContext
    {

        SortedList<string, VariableDeclaration> variables = new SortedList<string, VariableDeclaration>();
        SortedList<string, FunctionDeclaration> functions = new SortedList<string, FunctionDeclaration>();

        public Type ModelType { get; set; }

        public Type ResponseType { get; set; }

        public System.Linq.Expressions.Expression Parameter { get; set; }

        public FunctionDeclaration GetFunction(string name)
        {
            functions.TryGetValue(name, out FunctionDeclaration f);
            return f;
        }

        public FunctionDeclaration GetFunctionDeclaration(string name)
        {
            functions.TryGetValue(name, out FunctionDeclaration f);
            return f;
        }

        public FunctionDeclaration GetFunctionDeclaration(Type netType, string name)
        {
            if(functions.TryGetValue(name , out var f))
            {
              
            }

            return f;
        }

        public VariableDeclaration GetVariableDeclaration(string name)
        {           
            if (!variables.TryGetValue(name, out VariableDeclaration v))
            {
                var targetType = ResponseType ?? ModelType;

                PropertyInfo modelProperty;
                System.Linq.Expressions.Expression linqExpression;

                var responseProperty = targetType.GetProperty(name);
                if (responseProperty == null)
                {                    
                    responseProperty = targetType.GetProperty(name.Pascal());
                }

                if (responseProperty == null)
                    throw new RecognitionException("Property not found " + name + " in " + targetType.Name);

                if (ResponseType == null || ResponseType == ModelType)
                {
                    modelProperty = responseProperty;
                    linqExpression = Parameter;
                }
                else
                {
                    NavigationPropertyAttribute navAttr = responseProperty.GetCustomAttribute<NavigationPropertyAttribute>();
                    if (navAttr != null)
                    {
                        modelProperty = ModelType.GetProperty(navAttr.NavigationProperty);
                        linqExpression = System.Linq.Expressions.Expression.Property(Parameter, modelProperty);

                        modelProperty = modelProperty.PropertyType.GetProperty(navAttr.Property);
                    }
                    else
                    {
                        modelProperty = ModelType.GetProperty(name);
                        linqExpression = Parameter;
                    }
                }

                v = new PropertyVariableDeclaration(linqExpression, modelProperty);
                variables.Add(name, v);
            }

            return v;
        }

        public void AddVariable<T>(string name, T value)
        {
            ConstantVariableDeclaration v = new ConstantVariableDeclaration(name, value, typeof(T));
            variables.Add(name, v);
        }

        public void AddVariable<T>(string name, Func<T> resolver)
        {
            ConstantVariableDeclaration v = new ConstantVariableDeclaration(name, resolver, typeof(T));
            variables.Add(name, v);
        }

        public void AddFunction(string name, MethodInfo method)
        {
            FunctionDeclaration f = new FunctionDeclaration(name, method);
            functions.Add(f.Name, f);
        }

        public void AddFunction(string name, Delegate function)
        {
            FunctionDeclaration f = new FunctionDeclaration(name, function.Method)
            {
                Target = function.Target
            };

            functions.Add(f.Name, f);
        }
        
        public void AddFunctionsFromType(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public| BindingFlags.Static);
            foreach (var item in methods)
            {
                var attr = item.GetCustomAttribute<ParserFunctionAttribute>();
                if (attr != null)
                {
                    if (attr.Name != null)
                    {
                        AddFunction(attr.Name, item);
                    }
                    else
                    {
                        AddFunction(item.Name, item);
                    }
                }
            }
        }

        public void AddFunctionsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes().Where(t => t.IsClass && t.IsSealed && t.IsAbstract))
            {
                AddFunctionsFromType(type);
            }
        }

    }

    public class ChildASTContext : IASTContext
    {
        IASTContext parent;
        SortedList<string, VariableDeclaration> variables = new SortedList<string, VariableDeclaration>();

        public ChildASTContext(IASTContext parent)
        {
            this.parent = parent;
        }

        public FunctionDeclaration GetFunctionDeclaration(string name)
        {
            return parent.GetFunctionDeclaration(name);
        }

        public FunctionDeclaration GetFunctionDeclaration(Type netType, string name)
        {
            return parent.GetFunctionDeclaration(netType, name);
        }

        public VariableDeclaration GetVariableDeclaration(string name)
        {
           if(!variables.TryGetValue(name ,out VariableDeclaration v))
            {
                return parent.GetVariableDeclaration(name);
            }

            return v;
        }

        public void AddVariable<T>(string name, T value)
        {
            ConstantVariableDeclaration v = new ConstantVariableDeclaration(name, value, typeof(T));
            variables.Add(name, v);
        }

        public void AddVariable<T>(string name, Func<T> resolver)
        {
            ConstantVariableDeclaration v = new ConstantVariableDeclaration(name, resolver, typeof(T));
            variables.Add(name, v);
        }
    }

}
