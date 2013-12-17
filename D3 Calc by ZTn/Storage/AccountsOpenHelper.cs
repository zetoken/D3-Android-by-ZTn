using Android.Content;
using Android.Database.Sqlite;
using System;

namespace ZTnDroid.D3Calculator.Storage
{
    public class AccountsOpenHelper : SQLiteOpenHelper
    {
        private const String DATABASE_NAME = "d3calculator.db";
        private const int DatabaseVersion = 1;

        public static readonly String TABLE_NAME = "accounts";
        public static readonly String FIELD_BATTLETAG = "battletag";
        public static readonly String FIELD_HOST = "host";
        public static readonly String FIELD_UPDATED = "updated";

        private static readonly String TABLE_CREATE = "CREATE TABLE " + TABLE_NAME
            + " ("
            + "[_id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, "
            + FIELD_BATTLETAG + " TEXT, "
            + FIELD_HOST + " TEXT, "
            + FIELD_UPDATED + " TEXT"
            + ");";

        public AccountsOpenHelper(Context context)
            : base(context, DATABASE_NAME, null, DatabaseVersion)
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