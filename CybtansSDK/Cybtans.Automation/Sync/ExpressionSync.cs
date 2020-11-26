using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cybtans.Automation
{

    internal readonly struct MemberAccessVisitor
    {
        readonly MemberExpression exp;

        private MemberAccessVisitor(MemberExpression exp)
        {
            this.exp = exp;
        }

        public PropertyInfo Visit()
        {
            var property = exp.Member as PropertyInfo;
            if (property == null)
            {
                throw new InvalidOperationException($"{exp.Member} is not a property");
            }

            var expVisitor = Create(exp.Expression);
            if (expVisitor != null)
            {
                throw new InvalidOperationException($"{exp.Expression} is not supported");
            }

            return property;
        }

        internal static MemberAccessVisitor? Create(Expression body)
        {
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return new MemberAccessVisitor((MemberExpression)body);
                case ExpressionType.Parameter:
                    return null;
                default:
                    throw new InvalidOperationException($"Expression {body} is not valid");

            }
        }
    }

    public class ExpressionSync<T>
        where T : TestComponent
    {
        readonly T _component;
        readonly List<PropertySyncHandler> _handlers;

        public ExpressionSync(T component, List<PropertySyncHandler> handlers)
        {
            _component = component;
            _handlers = handlers;
        }

        public void WaitUntil(Func<T, bool> func = null)
        {
            Sync(_component.Wait(), func);
        }

        public void WaitUntilExist()
        {
            Sync(_component.Wait(), _ => _handlers.All(x => x.Value != null));
        }

        public void Now()
        {
            Sync(null, null);
        }

        private bool Sync(WebDriverWait wait, Func<T, bool> func)
        {
            return wait != null ? 
                wait.Until(driver => SyncHandlers(driver, func))
                : SyncHandlers(_component.Driver, func);
        }

        private bool SyncHandlers(IWebDriver driver, Func<T, bool> func)
        {
            try
            {
                foreach (var handler in _handlers)
                {
                    handler.Sync(driver, (ISearchContext)_component.Container ?? driver);                   
                }

                return func?.Invoke(_component) ?? true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

}
