using Setas.Common.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Setas.Models.Data
{
    [Table("HistoryItems")]
    public class HistoryItem
    {
        public DateTime TakenOn { get; set; }

        [Indexed]
        [ForeignKey(typeof(MushroomData))]
        public int MushroomId { get; set; }

        [ManyToOne("MushroomId", ReadOnly = true)]
        public MushroomData Mushroom { get; set; }


    }
}
