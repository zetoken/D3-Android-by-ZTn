using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Storage
{
    public class AccountsDB
    {
        SQLiteOpenHelper dbHelper;

        public AccountsDB(Context context)
        {
            dbHelper = new AccountsOpenHelper(context);
        }

        public long delete(String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            return db.Delete(AccountsOpenHelper.TABLE_NAME, AccountsOpenHelper.FIELD_BATTLETAG + "=? AND " + AccountsOpenHelper.FIELD_HOST + "=?", new String[] { battleTag, host });
        }

        public long insert(String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            values.Put(AccountsOpenHelper.FIELD_UPDATED, DateTime.Today.ToString());
            return db.Insert(AccountsOpenHelper.TABLE_NAME, null, values);
        }

        public long update(String oldBattleTag, String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            values.Put(AccountsOpenHelper.FIELD_UPDATED, DateTime.Today.ToString());
            return db.Update(AccountsOpenHelper.TABLE_NAME, values, AccountsOpenHelper.FIELD_BATTLETAG + "=?", new String[] { oldBattleTag });
        }

        public ICursor getAccounts()
        {
            SQLiteDatabase db = dbHelper.ReadableDatabase;
            return db.Query(AccountsOpenHelper.TABLE_NAME,
                new String[] { "_id", AccountsOpenHelper.FIELD_BATTLETAG, AccountsOpenHelper.FIELD_HOST, AccountsOpenHelper.FIELD_UPDATED },
                null, null,
				null, null, AccountsOpenHelper.FIELD_BATTLETAG);
        }

        public ICursor getAccount(String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.ReadableDatabase;
            return db.Query(AccountsOpenHelper.TABLE_NAME,
                new String[] { "_id", AccountsOpenHelper.FIELD_BATTLETAG, AccountsOpenHelper.FIELD_HOST, AccountsOpenHelper.FIELD_UPDATED },
                AccountsOpenHelper.FIELD_BATTLETAG + "=? AND " + AccountsOpenHelper.FIELD_HOST + "=?", new String[] { battleTag, host },
                null, null, null);
        }
    }
}