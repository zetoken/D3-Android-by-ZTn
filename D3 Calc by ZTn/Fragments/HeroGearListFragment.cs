using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroGearListFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreate");
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreateView");
            
            View view = inflater.Inflate(Resource.Layout.ViewHeroGear, container, false);
             
            return view;
        }
    }
}