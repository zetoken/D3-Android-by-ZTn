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
    public class CareersDB
    {
        SQLiteOpenHelper dbHelper;

        public CareersDB(Context context)
        {
            dbHelper = new CareersOpenHelper(context);
        }

        public long insert(String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(CareersOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(CareersOpenHelper.FIELD_HOST, host);
            return db.Insert(CareersOpenHelper.TABLE_NAME, null, values);
        }

        public long update(String oldBattleTag, String battleTag, String host)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(CareersOpenHelper.FIELD_BATTLETAG, battleTag);
            values.Put(CareersOpenHelper.FIELD_HOST, host);
            return db.Update(CareersOpenHelper.TABLE_NAME, values, CareersOpenHelper.FIELD_BATTLETAG + "=" + oldBattleTag, null);
        }

        public long delete(String battleTag)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            return db.Delete(CareersOpenHelper.TABLE_NAME, CareersOpenHelper.FIELD_BATTLETAG + "=" + battleTag, null);
        }

        public ICursor getAccounts()
        {
            SQLiteDatabase db = dbHelper.ReadableDatabase;
            return db.RawQuery("SELECT * FROM " + CareersOpenHelper.TABLE_NAME, null);
        }
    }
}