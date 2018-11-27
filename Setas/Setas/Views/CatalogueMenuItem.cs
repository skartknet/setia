using Setas.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setas.Views
{

    public class CatalogueMenuItem
    {
        public CatalogueMenuItem()
        {
            TargetType = typeof(CatalogueDetail);
        }
        public Edible Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}