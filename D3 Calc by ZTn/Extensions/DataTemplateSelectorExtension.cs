using System;
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

        internal void OnBindingContextChanged(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            var bindingContext = cell.BindingContext;

            if (bindingContext == null)
            {
                return;
            }

            var dataTemplate = FindDataTemplateByName(bindingContext.GetType().Name);
            if (dataTemplate != null)
            {
                cell.View = ((ViewCell)dataTemplate.CreateContent()).View;
            }
        }
    }

}