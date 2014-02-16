using Android.App;
using Android.Content;
using Android.Runtime;
using System;
using System.Globalization;
using ZTn.BNet.D3;
using ZTn.BNet.D3.DataProviders;
using ZTnDroid.D3Calculator.Storage;
using CacheableDataProvider = ZTnDroid.D3Calculator.DataProviders.CacheableDataProvider;

namespace ZTnDroid.D3Calculator
{
#if DEBUG
    [Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo", Debuggable = true)]
#else
    [Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo", Debuggable = false)]
#endif
    public class D3Calc : Application
    {
        #region >> Fields

        public static ISharedPreferences Preferences;

        #endregion

        #region >> Properties

        public static D3Calc Instance { get; private set; }

        #endregion

        #region >> Constants

        public static readonly String SettingsFilename = "settings";
        public static readonly String SettingsOnlinemode = "onlineMode";

        #endregion

        #region >> Constructors

        public D3Calc(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        #endregion

        #region >> Application

        public override void OnCreate()
        {
            base.OnCreate();

            Instance = this;

            // Load preferences
            Preferences = GetSharedPreferences(SettingsFilename, FileCreationMode.Private);
            // Default offline mode
            D3Context.Instance.FetchMode = (Preferences.GetBoolean(SettingsOnlinemode, false) ? FetchMode.Online : FetchMode.OnlineIfMissing);

            // Always start D3Api with cache available
            var dataProvider = new CacheableDataProvider(new HttpRequestDataProvider())
            {
                FetchMode = D3Context.Instance.FetchMode
            };
            D3Api.DataProvider = dataProvider;

            // Set english locale by default
            D3Api.Locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        #endregion
    }
}