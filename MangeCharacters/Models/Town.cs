using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangeCharacters.Models
{
    public class Town
    {
        public int TownId { get; set; }
        public int HouseId{ get; set; }
        public string TownName{ get; set; }
        public string HouseName{ get; set; }
    }
}
