using System.Collections.Generic;
using Android.Content;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Upgrades
{
    internal class MigrationTo20
    {
        private readonly Dictionary<string, string> oldToNewHosts;

        public MigrationTo20()
        {
            oldToNewHosts = new Dictionary<string, string>
            {
                { "eu.battle.net", "eu.api.battle.net" },
                { "us.battle.net", "us.api.battle.net" },
                { "kr.battle.net", "kr.api.battle.net" },
                { "tw.battle.net", "tw.api.battle.net" }
            };
        }

        public void DoMigration()
        {
            var preferences = D3Calc.Instance.GetSharedPreferences(D3Calc.SettingsFilename, FileCreationMode.Private);

            var dbHelper = new AccountsOpenHelper(D3Calc.Instance);
            var db = dbHelper.WritableDatabase;

            foreach (var oldToNewHost in oldToNewHosts)
            {
                var values = new ContentValues();
                values.Put(AccountsOpenHelper.FIELD_HOST, oldToNewHost.Value);
                db.Update(AccountsOpenHelper.TABLE_NAME, values, AccountsOpenHelper.FIELD_HOST + "=?", new[] { oldToNewHost.Key });
            }

            db.Close();

            preferences.Edit()
                .PutInt(D3Calc.UpToDateVersion, 20)
                .Apply();
        }
    }
}