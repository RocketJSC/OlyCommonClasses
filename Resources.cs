using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Resources
    {
        public static void Generate_Admit_Accept_Faction_Files(string path, List<Character> _characters, List<Location> _locations, List<Player> _players, List<Ship> _ships)
        {
            // generate file for each faction
            foreach(Player player in _players.Where(x => x._Player_Type == "pl_regular"))
            {
                Generate_Admit_Accept_Faction_File(player, path, _characters, _locations, _players, _ships);
            }
        }
        private static void Generate_Admit_Accept_Faction_File(Player player, string path, List<Character> _characters, List<Location> _locations, List<Player> _players, List<Ship> _ships)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, "accept_admit_" + player._FactionId_Conv + ".txt"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    // generate overall list of factions
                    string faction_list = "";
                    foreach (Player myplayer in _players.Where(x => x._Player_Type == "pl_regular"))
                    {
                        faction_list = faction_list + myplayer._FactionId_Conv + " ";
                    }
                    // write admit for each faction in alliance
                    w.WriteLine("# faction accepts");
                    foreach (Player _myplayer in _players.Where(x => x._Player_Type == "pl_regular"))
                    {
                        w.WriteLine("accept {0} 0 # {1}",_myplayer._FactionId_Conv, _myplayer._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each character in faction to allow stacking
                    w.WriteLine("# character stacking admits");
                    foreach (Character mychar in _characters.Where(x=>x._PlayerId == player._FactionId))
                    {
                        w.WriteLine("admit {0} {1} # {2}", mychar._CharId, faction_list, mychar._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each castle to allow stacking
                    w.WriteLine("# castle admits");
                    foreach (Location myloc in _locations.Where(x => x._Loc_Type == "castle"))
                    {
                        w.WriteLine("admit {0} {1} # {2}", myloc._LocId_Conv, faction_list, myloc._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each tower to allow stacking
                    w.WriteLine("# tower admits");
                    foreach (Location myloc in _locations.Where(x => x._Loc_Type == "tower"))
                    {
                        w.WriteLine("admit {0} {1} # {2}", myloc._LocId_Conv, faction_list, myloc._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each mine to allow stacking
                    w.WriteLine("# mine admits");
                    foreach (Location myloc in _locations.Where(x => x._Loc_Type == "mine"))
                    {
                        w.WriteLine("admit {0} {1} # {2}", myloc._LocId_Conv, faction_list, myloc._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each inn to allow stacking
                    w.WriteLine("# inn admits");
                    foreach (Location myloc in _locations.Where(x => x._Loc_Type == "inn"))
                    {
                        w.WriteLine("admit {0} {1} # {2}", myloc._LocId_Conv, faction_list, myloc._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each mine to allow stacking
                    w.WriteLine("# temple admits");
                    foreach (Location myloc in _locations.Where(x => x._Loc_Type == "temple"))
                    {
                        w.WriteLine("admit {0} {1} # {2}", myloc._LocId_Conv, faction_list, myloc._Name);
                    }
                    w.WriteLine(" ");
                    // write admit for each ship to allow stacking
                    w.WriteLine("# ship admits");
                    foreach (Ship myship in _ships)
                    {
                        w.WriteLine("admit {0} {1} # {2}", myship._ShipId, faction_list, myship._Name);
                    }
                    w.WriteLine(" ");
                }
                fs.Dispose();
            }
        }
    }
}
