using System;
using System.Collections.Generic;
using System.Text;

namespace Setas.Models
{
    class HistoryItemDisplayModel
    {
        public string TakenOn { get; set; }

        public Guid MushroomId { get; set; }

        public MushroomDisplayModel Mushroom { get; set; }
    }
}
