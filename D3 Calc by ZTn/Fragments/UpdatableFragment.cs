using System.Reflection;

using Android.App;

using ZTnDroid.D3Calculator.Helpers;

using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public abstract class UpdatableFragment : Fragment
    {
        public void updateFragment()
        {
            ZTnTrace.trace(this, MethodInfo.GetCurrentMethod());

            if (Activity != null)
            {
                Activity.SupportFragmentManager
                    .BeginTransaction()
                    .Detach(this)
                    .Attach(this)
                    .Commit();
            }
        }
    }
}