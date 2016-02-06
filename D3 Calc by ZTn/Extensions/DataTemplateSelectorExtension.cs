using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZTn.Pcl.D3Calculator.Extensions
{
    public class DataTemplateSelectorExtension : IMarkupExtension
    {
        public Page Page { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new DataTemplate(GetHookedCell);
        }

        private Cell GetHookedCell()
        {
            var content = new ViewCell();

            content.BindingContextChanged += OnBindingContextChanged;

            return content;
        }

        private DataTemplate FindDataTemplateByName(string name)
        {
            object targetDataTemplate;
            DataTemplate dataTemplate;

            if (Page?.Resources != null && Page.Resources.TryGetValue(name, out targetDataTemplate))
            {
                dataTemplate = targetDataTemplate as DataTemplate;
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }

            if (Application.Current.Resources != null && Application.Current.Resources.TryGetValue(name, out targetDataTemplate))
            {
                dataTemplate = targetDataTemplate as DataTemplate;

                return dataTemplate;
            }

            return null;
        }

        static IEnumerable<string> GetInheritedClasses(object anObject)
        {
            var type = anObject.GetType();
            while (type != typeof(object))
            {
                yield return type.Name;
                type = type.GetTypeInfo().BaseType;
            }
        }

        internal void OnBindingContextChanged(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            var bindingContext = cell.BindingContext;

            if (bindingContext == null)
            {
                return;
            }

            foreach (var inheritedClassName in GetInheritedClasses(bindingContext))
            {
                var dataTemplate = FindDataTemplateByName(inheritedClassName);
                if (dataTemplate != null)
                {
                    cell.View = ((ViewCell)dataTemplate.CreateContent()).View;
                    break;
                }
            }
        }
    }

}