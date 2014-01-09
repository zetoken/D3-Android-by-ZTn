using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace ZTnDroid.D3Calculator
{
    public class ZTnFragmentActivity : FragmentActivity
    {
        #region >> FragmentActivity

        /// <inheritdoc/>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetIcon(Resource.Drawable.Icon);
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }
}