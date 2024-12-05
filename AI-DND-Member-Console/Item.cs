using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_DND_Member_Console
{
    public class Item
    {
        public string name;
        public int quantity;
        public string description;
        public override string ToString()
        {
            return $"Item Name: {name}\nQuantity: {quantity}\nDescription: {description}";
        }
    }
}
