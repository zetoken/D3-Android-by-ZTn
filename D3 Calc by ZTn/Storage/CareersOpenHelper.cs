using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Storage
{
    public class CareersOpenHelper : SQLiteOpenHelper
    {
        private static readonly String DATABASE_NAME = "d3calculator.db";
        private static readonly int DATABASE_VERSION = 1;

        public static readonly String TABLE_NAME = "accounts";
        public static readonly String FIELD_BATTLETAG = "battletag";
        public static readonly String FIELD_HOST = "host";

        private static readonly String TABLE_CREATE = "CREATE TABLE " + TABLE_NAME
            + " ("
            + "[_id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, "
            + FIELD_BATTLETAG + " TEXT, "
            + FIELD_HOST + " TEXT"
            + ");";

        public CareersOpenHelper(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(TABLE_CREATE);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
        }
    }
}