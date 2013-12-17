using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using System;
using System.Globalization;

namespace ZTnDroid.D3Calculator.Storage
{
    public class AccountsDB
    {
        readonly SQLiteOpenHelper dbHelper;

        public AccountsDB(Context context)
        {
            dbHelper = new AccountsOpenHelper(context);
        }

        public long Delete(String battleTag, String host)
        {
            var db = dbHelper.WritableDatabase;
            return db.Delete(AccountsOpenHelper.TABLE_NAME, AccountsOpenHelper.FIELD_BATTLETAG + "=? AND " + AccountsOpenHelper.FIELD_HOST + "=?", new[] { battleTag, host });
        }

        public long Insert(String battleTag, String host)
        {
            var db = dbHelper.WritableDatabase;
            var values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            values.Put(AccountsOpenHelper.FIELD_UPDATED, DateTime.Today.ToString());
            return db.Insert(AccountsOpenHelper.TABLE_NAME, null, values);
        }

        public long Update(String oldBattleTag, String battleTag, String host)
        {
            var db = dbHelper.WritableDatabase;
            var values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            values.Put(AccountsOpenHelper.FIELD_UPDATED, DateTime.Today.ToString(CultureInfo.InvariantCulture));
            return db.Update(AccountsOpenHelper.TABLE_NAME, values, AccountsOpenHelper.FIELD_BATTLETAG + "=?", new[] { oldBattleTag });
        }

        public ICursor GetAccounts()
        {
            var db = dbHelper.ReadableDatabase;
            return db.Query(AccountsOpenHelper.TABLE_NAME,
                new[] { "_id", AccountsOpenHelper.FIELD_BATTLETAG, AccountsOpenHelper.FIELD_HOST, AccountsOpenHelper.FIELD_UPDATED },
                null, null,
                null, null, AccountsOpenHelper.FIELD_BATTLETAG);
        }

        public ICursor GetAccount(String battleTag, String host)
        {
            var db = dbHelper.ReadableDatabase;
            return db.Query(AccountsOpenHelper.TABLE_NAME,
                new[] { "_id", AccountsOpenHelper.FIELD_BATTLETAG, AccountsOpenHelper.FIELD_HOST, AccountsOpenHelper.FIELD_UPDATED },
                AccountsOpenHelper.FIELD_BATTLETAG + "=? AND " + AccountsOpenHelper.FIELD_HOST + "=?", new[] { battleTag, host },
                null, null, null);
        }
    }
}