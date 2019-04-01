using System;
using System.Collections.Generic;
using System.Text;

namespace Setas.Models
{
    public class HistoryItemDisplayModel
    {
        public string TakenOn { get; set; }

        public int MushroomId { get; set; }

        public MushroomDisplayModel Mushroom { get; set; }
    }
}
