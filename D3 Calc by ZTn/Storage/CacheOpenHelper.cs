using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Database.Sqlite;

namespace ZTnDroid.D3Calculator.Storage
{
    public class CacheOpenHelper : SQLiteOpenHelper
    {
        private static readonly String DATABASE_NAME = "d3cache.db";
        private static readonly int DATABASE_VERSION = 1;

        public static readonly String TABLE_NAME = "cache";
        public static readonly String FIELD_URL = "url";
        public static readonly String FIELD_FILE = "file";
        public static readonly String FIELD_UPDATED = "updated";

        private static readonly String TABLE_CREATE = "CREATE TABLE " + TABLE_NAME
            + " ("
            + "[_id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, "
            + FIELD_URL + " TEXT, "
            + FIELD_FILE + " TEXT, "
            + FIELD_UPDATED + " TEXT"
            + ");";

        public CacheOpenHelper(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(TABLE_CREATE);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_NAME);
            OnCreate(db);
        }
    }
}
