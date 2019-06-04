using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangeCharacters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace MangeCharacters.Controllers
{
    public class HeroController : Controller
    {
        private readonly IConfiguration configuration;
        public HeroController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Your()
        {
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            var heroes = new List<HeroViewModel>();
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT  HeroId, Name, Strength, Magic, HP, RaceName, ProfessionName, CultName FROM FANTASYRPG.Hero
                        Join FANTASYRPG.Race using (RaceID)
                        Join FANTASYRPG.Profession using (ProfessionID)
                        Join FANTASYRPG.Cult using (CultID) 
                        WHERE Hero.UserID = {HttpContext.Session.GetInt32("UserID")}
                        ORDER BY HeroId";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            var hero = new HeroViewModel();
                            hero.HeroId = reader.GetInt32(0);
                            hero.Name = reader.GetString(1);
                            hero.Strength = reader.GetInt32(2);
                            hero.Magic = reader.GetInt32(3);
                            hero.HP = reader.GetInt32(4);
                            hero.RaceName = reader.GetString(5);
                            hero.ProfessionName = reader.GetString(6);
                            hero.CultName = reader.GetString(7);
                            heroes.Add(hero);
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }
            return View("All", heroes);
        }
        public IActionResult All()
        {
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            var heroes = new List<HeroViewModel>();
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT  HeroId, Name, Strength, Magic, HP, RaceName, ProfessionName, CultName UserName FROM FANTASYRPG.Hero
                        Join FANTASYRPG.Race using (RaceID)
                        Join FANTASYRPG.Profession using (ProfessionID)
                        Join FANTASYRPG.Cult using (CultID) 
                        Join FANTASYRPG.""User"" using (UserID)
                        ORDER BY HeroId";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            var hero = new HeroViewModel();
                            hero.HeroId = reader.GetInt32(0);
                            hero.Name = reader.GetString(1);
                            hero.Strength = reader.GetInt32(2);
                            hero.Magic = reader.GetInt32(3);
                            hero.HP = reader.GetInt32(4);
                            hero.RaceName = reader.GetString(5);
                            hero.ProfessionName = reader.GetString(6);
                            hero.CultName = reader.GetString(7);
                            heroes.Add(hero);
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }
            return View(heroes);
        }
        public IActionResult Details(int heroId)
        {
            var hero = new HeroViewModel();
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT  HeroId, Name, Strength, Magic, HP, RaceName, ProfessionName, CultName FROM FANTASYRPG.Hero
                        Join FANTASYRPG.Race using (RaceID)
                        Join FANTASYRPG.Profession using (ProfessionID)
                        Join FANTASYRPG.Cult using (CultID) 
                        WHERE Hero.HeroId = {heroId}";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            hero.HeroId = reader.GetInt32(0);
                            hero.Name = reader.GetString(1);
                            hero.Strength = reader.GetInt32(2);
                            hero.Magic = reader.GetInt32(3);
                            hero.HP = reader.GetInt32(4);
                            hero.RaceName = reader.GetString(5);
                            hero.ProfessionName = reader.GetString(6);
                            hero.CultName = reader.GetString(7);
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }
            return View(hero);
        }
        public IActionResult PickRace()
        {
            var raceProffesionList = new List<RaceProfessionViewModel>();
           
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.Race
                        JOIN FANTASYRPG.RACE_PROFESSION USING(RaceID)
                        JOIN FANTASYRPG.Profession Using(PROFESSIONID)
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            var raceProffesion = new RaceProfessionViewModel
                            {
                                ProfessionId = reader.GetInt32(0),
                                RaceId = reader.GetInt32(1),
                                RaceName = reader.GetString(2),
                                ProfesionName = reader.GetString(4)
                            };
                            raceProffesionList.Add(raceProffesion);
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(raceProffesionList);
        }
        public IActionResult PickTown(int raceId, int profId)
        {
            HttpContext.Session.SetInt32("raceId", raceId);
            HttpContext.Session.SetInt32("profId", profId);
            var towns = new List<Town>();

            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.Town
                        JOIN FANTASYRPG.TOWN_HOUSE USING(TOWNID)
                        JOIN FANTASYRPG.House USING(HouseID)
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            towns.Add(new Town()
                            {
                                HouseId = reader.GetInt32(0),
                                TownId = reader.GetInt32(1),
                                TownName = reader.GetString(2),
                                HouseName = reader.GetString(4)
                            });
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(towns);
        }
        public IActionResult PickCult(int townId)
        {
            HttpContext.Session.SetInt32("townId", townId);
            var cults = new List<Cult>();

            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.Cult
                        JOIN FANTASYRPG.Magic USING(MagicID)
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            cults.Add(new Cult()
                            {
                                MagicId = reader.GetInt32(0),
                                CultId = reader.GetInt32(1),
                                CultName = reader.GetString(2),
                                MagicName = reader.GetString(3)
                            });
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(cults);
        }
        public IActionResult RaceSkills(int raceId)
        {
            var raceSkills = new RaceSkill();

            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.Race
                        JOIN FANTASYRPG.RACE_Skill USING(RaceID)
                        JOIN FANTASYRPG.SKILL Using(SkillID)
                        WHERE RaceID = {raceId}
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            raceSkills.Name = reader.GetString(2);
                            raceSkills.Skills.Add(reader.GetString(4));
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(raceSkills);
        }

        public IActionResult SpellMagic(int magicId)
        {
            var magicSpell = new MagicSpell();

            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.Spell
                        JOIN FANTASYRPG.Magic USING(MagicID)
                        WHERE MagicID = {magicId}
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            magicSpell.Name = reader.GetString(3);
                            magicSpell.Spells.Add(reader.GetString(2));
                        }
                    };
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(magicSpell);
        }

        public IActionResult PickUser(int magicId, int cultId)
        {
            HttpContext.Session.SetInt32("magicId", magicId);
            HttpContext.Session.SetInt32("cultId", cultId);
            var users = new List<User>();
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"SELECT * FROM FANTASYRPG.""User""";
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.RowSize > 0)
                        {
                            users.Add(new Models.User()
                            {
                                IsAdmin = reader.GetBoolean(2),
                                UserId = reader.GetInt32(0),
                                UserName  = reader.GetString(1)
                            });
                        }
                    }
                    reader.Dispose();
                    connection.Close();
                }
            }

            return View(users);
        }
        public IActionResult Create(int userId)
        {
            HttpContext.Session.SetInt32("chosenUser", userId);
            return View();
        }
        [HttpPost]
        public IActionResult Create(Hero hero)
        {
            var magicId = HttpContext.Session.GetInt32("magicId");
            var pickedUserId = HttpContext.Session.GetInt32("chosenUser");
            var raceId = HttpContext.Session.GetInt32("raceId");
            var townId = HttpContext.Session.GetInt32("townId");
            var cultId = HttpContext.Session.GetInt32("cultId");
            var userId = HttpContext.Session.GetInt32("UserID");
            var profId = HttpContext.Session.GetInt32("profId");
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"INSERT INTO FANTASYRPG.Hero(HeroID, Name, Strength, Magic, HP, ProfessionID, RaceID, TownID, CultID, UserID) 
VALUES((SELECT MAX(HeroId)+1 from FANTASYRPG.Hero), '{hero.Name}', {hero.Strength},{magicId}, {hero.HP}, {profId}, {raceId}, {townId}, {cultId}, {pickedUserId} )
                        ";
                    OracleDataReader reader = command.ExecuteReader();
                    reader.Dispose();
                    connection.Close();
                }
            }
            return RedirectToAction("Your", "Hero");
        }
        public IActionResult LevelUp(int heroId)
        {
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"UPDATE FANTASYRPG.Hero
SET Strength = (SELECT STRENGTH FROM FANTASYRPG.Hero WHERE HeroID = {heroId}) + 5,
    Magic = (SELECT Magic FROM FANTASYRPG.Hero WHERE HeroID = {heroId}) + 5,
    HP = (SELECT HP FROM FANTASYRPG.Hero WHERE HeroID = {heroId}) + 5
WHERE HeroID = {heroId}";
                    OracleDataReader reader = command.ExecuteReader();
                    reader.Dispose();
                    connection.Close();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Delete(int heroId)
        {
            using (OracleConnection connection = new OracleConnection(configuration["OracleConnectionString"]))
            {
                using (OracleCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText =
                        $@"DELETE FROM FANTASYRPG.Hero
WHERE HeroId = {heroId}";
                    OracleDataReader reader = command.ExecuteReader();
                    reader.Dispose();
                    connection.Close();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}