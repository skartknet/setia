using Setas.Data;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Setas.Models.Data
{
    [Table("HistoryItems")]
    public class HistoryItem
    {
        public DateTime TakenOn { get; set; }

        [ForeignKey(typeof(Mushroom))]
        public int MushroomId { get; set; }

        [ManyToOne(ReadOnly = true)]
        public Mushroom Mushroom { get; set; }

    }
}
