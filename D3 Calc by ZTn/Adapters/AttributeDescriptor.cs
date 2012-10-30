using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributeDescriptor
    {
        public String name;
        public String value;

        public AttributeDescriptor(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public AttributeDescriptor(String name, ItemValueRange value)
        {
            this.name = name;
            this.value = value.min.ToString();
        }

        public AttributeDescriptor(String name, long value)
        {
            this.name = name;
            this.value = value.ToString();
        }

        public AttributeDescriptor(String name, double value)
        {
            this.name = name;
            this.value = String.Format("{0:0.00}", value);
        }

        public AttributeDescriptor(String name, double value, Boolean percent)
        {
            this.name = name;
            if (percent)
                this.value = String.Format("{0:0.00} %", 100 * value);
            else
                this.value = String.Format("{0:0.00}", value);
        }
    }
}