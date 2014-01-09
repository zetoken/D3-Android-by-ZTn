using System.Reflection;
using ZTnDroid.D3Calculator.Helpers;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public abstract class UpdatableFragment : Fragment
    {
        public void UpdateFragment()
        {
            ZTnTrace.Trace(this, MethodBase.GetCurrentMethod());

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