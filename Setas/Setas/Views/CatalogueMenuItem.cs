using Setas.Common.Enums;
using Setas.Enums;
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
          
        }
        public EdibleTopClassEnum? Value { get; set; }
        public string Title { get; set; }

    }
}