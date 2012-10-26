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

        public long accountInsert(String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            return db.Insert(AccountsOpenHelper.TABLE_NAME, null, values);
        }

        public long accountUpdate(String oldBattleTag, String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(AccountsOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(AccountsOpenHelper.FIELD_HOST, host);
            return db.Update(AccountsOpenHelper.TABLE_NAME, values, AccountsOpenHelper.FIELD_BATTLETAG + "=" + oldBattleTag, null);
        }

        public long accountDelete(String battleTag)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            return db.Delete(AccountsOpenHelper.TABLE_NAME, AccountsOpenHelper.FIELD_BATTLETAG + "=" + battleTag, null);
        }

        public ICursor getAccounts()
        {
            SQLiteDatabase db = dbHelper.ReadableDatabase;
            return db.RawQuery("SELECT * FROM " + AccountsOpenHelper.TABLE_NAME, null);
        }
    }
}