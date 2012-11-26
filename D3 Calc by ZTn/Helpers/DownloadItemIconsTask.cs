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
using ZTn.BNet.D3;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Helpers
{
    public class DownloadItemIconsTask : AsyncTask<int, int, int>
    {
        Context context;
        ProgressDialog progress;

        public DownloadItemIconsTask(Context context)
        {
            this.context = context;
        }

        protected override int RunInBackground(params int[] @parameters)
        {
            HeroItems heroItems = D3Context.getInstance().heroItems;
            IconsContainer icons = D3Context.getInstance().icons;

            for (int index = 0; index < parameters.Length; index++)
            {
                try
                {
                    icons.head = D3Api.getItemIcon(heroItems.head.icon);
                    icons.torso = D3Api.getItemIcon(heroItems.torso.icon);
                    icons.feet = D3Api.getItemIcon(heroItems.feet.icon);
                    icons.hands = D3Api.getItemIcon(heroItems.hands.icon);
                    icons.shoulders = D3Api.getItemIcon(heroItems.shoulders.icon);
                    icons.legs = D3Api.getItemIcon(heroItems.legs.icon);
                    icons.bracers = D3Api.getItemIcon(heroItems.bracers.icon);
                    icons.mainHand = D3Api.getItemIcon(heroItems.mainHand.icon);
                    icons.offHand = D3Api.getItemIcon(heroItems.offHand.icon);
                    icons.waist = D3Api.getItemIcon(heroItems.waist.icon);
                    icons.rightFinger = D3Api.getItemIcon(heroItems.rightFinger.icon);
                    icons.leftFinger = D3Api.getItemIcon(heroItems.leftFinger.icon);
                    icons.neck = D3Api.getItemIcon(heroItems.neck.icon);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    D3Context.getInstance().heroItems = null;
                    throw exception;
                }
            }
            return 0;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();

            progress = ProgressDialog.Show(context, "In progress", "downloading");
        }

        protected override void OnProgressUpdate(params int[] values)
        {
            base.OnProgressUpdate(values);
        }

        protected override void OnPostExecute(int result)
        {
            base.OnPostExecute(result);

            progress.Dismiss();
        }
    }
}