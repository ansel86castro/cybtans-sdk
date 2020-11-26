using Cybtans.Automation;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cybtans.Automation
{
    public static class TestComponentExtensions
    {
        private static PropertySyncHandler GetSyncHandler<T>(T item, Expression expression) where T : TestComponent
        {
            var visitor = MemberAccessVisitor.Create(expression);
            if (visitor == null)
                throw new InvalidOperationException($"Expression not supported { expression }");

            var property = visitor.Value.Visit();

            if (!item._syncHandler.TryGetValue(property.Name, out PropertySyncHandler handler))
            {
                throw new InvalidOperationException($"Property {property.Name} is not decorated with a Selector attribute");
            }

            return handler;
        }

        public static ExpressionSync<T> Sync<T>(this T item, params Expression<Func<T, object>>[] properties)
            where T : TestComponent
        {
            List<PropertySyncHandler> handlers = new List<PropertySyncHandler>();
            foreach (var expression in properties)
            {
                PropertySyncHandler handler = GetSyncHandler(item, expression.Body);
                handlers.Add(handler);
            }

            return new ExpressionSync<T>(item, handlers);
        }

        public static ExpressionSync<T> SyncAll<T>(this T item)
           where T : TestComponent
        {
            return new ExpressionSync<T>(item, item._syncHandler.Values.ToList());
        }


        public static TestComponent ScrollIntoView(this TestComponent component, bool alignToTop = false)
        {
            if (component.Container == null)
                throw new InvalidOperationException("component's container is not defined");
            return component.ScrollIntoView(component.Container, alignToTop);
        }

        public static TestComponent ScrollIntoViewIfNeeded(this TestComponent component)
        {
            if (component.Container == null)
                throw new InvalidOperationException("component's container is not defined");
            return component.ScrollIntoViewIfNeeded(component.Container);
        }

        public static TestComponent ScrollIntoView(this TestComponent component, IWebElement element, bool alignToTop = false)
        {
            component.Driver.ScrollIntoView(element, alignToTop);
            return component;
        }

        public static TestComponent ScrollIntoViewIfNeeded(this TestComponent component, IWebElement element)
        {
            component.Driver.ScrollIntoViewIfNeeded(element);
            return component;
        }

        public static T ScrollIntoView<T>(this T component, Expression<Func<T, IWebElement>> property, bool alignToTop = false)
            where T : TestComponent
        {
            var handler = GetSyncHandler(component, property.Body);
            component.Driver.ScrollIntoView((IWebElement)handler.Value, alignToTop);
            return component;
        }

        public static T ScrollIntoViewIfNeeded<T>(this T component, Expression<Func<T, IWebElement>> property)
            where T : TestComponent
        {
            var handler = GetSyncHandler(component, property.Body);
            handler.Sync(component.Driver, component.SearchContext);

            component.Driver.ScrollIntoViewIfNeeded((IWebElement)handler.Value);
            return component;
        }

        public static T Click<T>(this T component, Expression<Func<T, IWebElement>> property)
            where T : TestComponent
        {
            var handler = GetSyncHandler(component, property.Body);
            handler.Sync(component.Driver, component.SearchContext);

            var element = (IWebElement)handler.Value;
            component.Driver.ScrollIntoViewIfNeeded(element);

            element.Click();

            return component;
        }

    }


}
