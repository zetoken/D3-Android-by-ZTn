using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace ZTnDroid.D3Calculator.Storage
{
    public class CacheDB
    {
        SQLiteOpenHelper dbHelper;

        public CacheDB(Context context)
        {
            dbHelper = new CacheOpenHelper(context);
        }

        public long delete(String url)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            return db.Delete(CacheOpenHelper.TABLE_NAME, CacheOpenHelper.FIELD_URL + "=?", new String[] { url });
        }

        public ICursor getCacheByUrl(String url)
        {
            SQLiteDatabase db = dbHelper.ReadableDatabase;
            return db.Query(CacheOpenHelper.TABLE_NAME,
                new String[] { "_id", CacheOpenHelper.FIELD_URL, CacheOpenHelper.FIELD_UPDATED },
                CacheOpenHelper.FIELD_URL + "=?", new String[] { url },
                null, null, null);
        }

        public long insert(String url, String cachedFileName)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(CacheOpenHelper.FIELD_URL, url);
            values.Put(CacheOpenHelper.FIELD_FILE, cachedFileName);
            values.Put(CacheOpenHelper.FIELD_UPDATED, DateTime.Today.ToString());
            return db.Insert(CacheOpenHelper.TABLE_NAME, null, values);
        }

        public long insertOrUpdate(String url, String cachedFileName)
        {
            if (getCacheByUrl(url).Count == 1)
                return updateTime(url);
            else
                return insert(url, cachedFileName);
        }

        public long updateTime(String url)
        {
            SQLiteDatabase db = dbHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(CacheOpenHelper.FIELD_UPDATED, DateTime.Today.ToString());
            return db.Update(CacheOpenHelper.TABLE_NAME, values, CacheOpenHelper.FIELD_URL + "=", new String[] { url });
        }
    }
}
