using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangeCharacters.Models
{
    public class HeroViewModel:Hero
    {
        public int HeroId { get; set; }
        public string RaceName { get; set; }
        public string ProfessionName { get; set; }
        public string CultName { get; set; }
    }
}
