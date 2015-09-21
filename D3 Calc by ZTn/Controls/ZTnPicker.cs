using System;
using System.Collections;
using Xamarin.Forms;

namespace ZTn.Pcl.D3Calculator.Controls
{
    /// <summary>
    /// Improved Picker supporting bindings using ItemsSource property.
    /// Inspired by http://forums.xamarin.com/discussion/19079/data-binding-for-the-items-source-of-a-picker-control.
    /// </summary>
    public class ZTnPicker : Picker
    {
        public static BindableProperty ItemsSourceProperty = BindableProperty.Create<ZTnPicker, IEnumerable>(
                x => x.ItemsSource, default(IEnumerable), BindingMode.OneWay, null, OnItemsSourceChanged
            );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var picker = (ZTnPicker)bindable;

            picker.Items.Clear();

            if (newvalue != null)
            {
                foreach (var item in newvalue)
                {
                    picker.Items.Add(item.ToString());
                }
            }
        }
    }
}
