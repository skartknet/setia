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
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}