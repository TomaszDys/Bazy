using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangeCharacters.Models
{
    public class MagicSpell
    {
        public string Name { get; set; }
        public List<string> Spells { get; set; } =  new List<string>();
    }
}
