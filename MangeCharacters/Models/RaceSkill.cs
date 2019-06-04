using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangeCharacters.Models
{
    public class RaceSkill
    {
        public string Name { get; set; }
        public List<string> Skills { get; set; } =  new List<string>();
    }
}
