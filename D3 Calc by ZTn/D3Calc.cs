using System;

using Android.App;
using Android.Content;
using Android.Runtime;

using ZTn.BNet.D3;
using ZTn.BNet.D3.DataProviders;

using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator
{
#if DEBUG
    [Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo", Debuggable = true)]
#else
    [Application(Icon = "@drawable/icon", Label = "D3 Calc by ZTn", Theme = "@android:style/Theme.Holo", Debuggable = false)]
#endif
    public class D3Calc : Android.App.Application
    {
        #region >> Fields

        private static D3Calc _application;

        public static ISharedPreferences preferences;

        #endregion

        #region >> Properties

        public static D3Calc instance
        {
            get { return D3Calc._application; }
        }

        #endregion

        #region >> Constants

        public static readonly String SETTINGS_FILENAME = "settings";
        public static readonly String SETTINGS_ONLINEMODE = "onlineMode";

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

            _application = this;

            // Load preferences
            preferences = GetSharedPreferences(SETTINGS_FILENAME, FileCreationMode.Private);
            // Default offline mode
            D3Context.instance.onlineMode = (preferences.GetBoolean(SETTINGS_ONLINEMODE, false) ? OnlineMode.Online : OnlineMode.OnlineIfMissing);

            // Always start D3Api with cache available
            DataProviders.CacheableDataProvider dataProvider = new DataProviders.CacheableDataProvider(new ZTn.BNet.D3.DataProviders.HttpRequestDataProvider());
            dataProvider.onlineMode = D3Context.instance.onlineMode;
            D3Api.dataProvider = dataProvider;

            // Set english locale by default
            D3Api.locale = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        #endregion
    }
}