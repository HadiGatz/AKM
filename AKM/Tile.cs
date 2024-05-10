using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class Tile
    {
        protected string name;
        protected string cardName;

        public Tile(string name)
        {
            this.name = name;
            cardName = name.ToLower().Replace(" ", "").Replace(".", "");
    }
        public string GetName()
        { 
            return name; 
        }
        public string GetCardName()
        { 
            return cardName;
        }
    }
}
