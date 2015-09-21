using SQLite;

namespace ZTn.Pcl.D3Calculator.Models
{
    [Table("accounts")]
    public class BnetAccount
    {
        /// <summary>
        /// Internal row id of the account in the database.
        /// </summary>
        [PrimaryKey, AutoIncrement, NotNull, Unique, Column("_id")]
        public int Id { get; set; }

        /// <summary>
        /// Battle tag of the account.
        /// </summary>
        [Column("battletag")]
        public string BattleTag { get; set; }

        /// <summary>
        /// Battle.net Host of the account.
        /// </summary>
        [Column("host")]
        public string Host { get; set; }

        /// <summary>
        /// Last updated row.
        /// </summary>
        [Column("updated")]
        public string Updated { get; set; }

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Id}:{BattleTag}@{Host} ({@Updated})}}";
        }

        #endregion
    }
}
