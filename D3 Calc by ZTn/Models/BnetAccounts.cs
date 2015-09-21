using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace ZTn.Pcl.D3Calculator.Models
{
    /// <summary>
    /// Wrapper for Battle.net accounts database.
    /// </summary>
    class BnetAccounts
    {
        private static readonly object Locker = new object();
        private static readonly SQLiteConnection Database;

        static BnetAccounts()
        {
            if (Database != null)
            {
                return;
            }

            Database = DependencyService.Get<ISqLite>().GetConnection();
            Database.CreateTable<BnetAccount>();
            Database.Insert(new BnetAccount { BattleTag = "Tok#2360", Host = "eu.api.battle.net" });
            Database.Insert(new BnetAccount { BattleTag = "Solo#2284", Host = "eu.api.battle.net" });
            Database.Insert(new BnetAccount { BattleTag = "None#0000", Host = "us.api.battle.net" });
        }

        /// <summary>
        /// Get all stored accounts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BnetAccount> GetAllAccounts()
        {
            lock (Locker)
            {
                return Database.Table<BnetAccount>();
            }
        }

        /// <summary>
        /// Get account stored in database having <paramref name="battleTag"/> and <paramref name="host"/> fields.
        /// </summary>
        /// <param name="battleTag"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public BnetAccount GetAccount(string battleTag, string host)
        {
            lock (Locker)
            {
                return Database.Table<BnetAccount>().FirstOrDefault(a => a.BattleTag == battleTag && a.Host == host);
            }
        }

        /// <summary>
        /// Inserts a new account in the database.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public BnetAccount InsertAccount(BnetAccount account)
        {
            lock (Locker)
            {
                Database.Insert(account);
            }

            return account;
        }

        /// <summary>
        /// Deletes a given account from the database using its <see cref="BnetAccount.Id"/> field.
        /// </summary>
        /// <param name="account"></param>
        public void DeleteAccount(BnetAccount account)
        {
            lock (Locker)
            {
                Database.Delete<BnetAccount>(account.Id);
            }
        }
    }
}
