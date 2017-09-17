using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Location
    {
        public int _LocId { get; set; }
        public string _LocId_Conv { get; set; }
        public int _LI_Where { get; set; }
        //public List<string> _LI_Inner_Locations { get; set; }
        public List<int> _LI_Here_List { get; set; }
        public List<int> _LO_Province_Destination { get; set; }
        public int _LO_Hidden { get; set; }
        public int _LO_Shrouded { get; set; }
        public int _LO_Barrier { get; set; }
        public int _LO_Civ_Level { get; set; }
        public int _SL_Safe { get; set; }
        public int _SL_Shaft_Depth { get; set; }
        public int _SL_Damage { get; set; }
        public int _SL_Defense { get; set; }
        public int _SL_Castle_Level { get; set; }
        public int _SL_Capacity { get; set; }
        public int _SL_Moving { get; set; }
        public List<int> _SL_Teaches { get; set; }
        public int _SL_Effort_Required { get; set; }
        public int _SL_Effort_Given { get; set; }
        public List<int> _SL_Near_Cities { get; set; }
        public int _SL_Building_Materials { get; set; }
        public string _Name { get; set; }
        public string _Loc_Type { get; set; }
        public List<int> _Item_List { get; set; }
        public string _First_Line { get; set; }
        public List<int> _Trade_List { get; set; }
        public string _castle_id { get; set; }
        public int _Region_id { get; set; }
        public int _nbr_men { get; set; }
        public bool _ships_found { get; set; }
        public bool _enemy_found { get; set; }
        public string Calc_CurrentLoc { get; set; }
        public int Calc_CurrentRegion { get; set; }
        public static void Add(string InputKey, string InputString, List<Location> _locations)
        {
            JObject j1 = JObject.Parse(InputString);
            JArray myna;
            JArray myfl;
            if (j1.SelectToken("na") != null && j1.SelectToken("na").HasValues)
            {
                myna = (JArray)j1.SelectToken("na");
            }
            else
            {
                myna = null;
            }
            string myloctype;
            if (j1.SelectToken("firstline") != null && j1.SelectToken("firstline").HasValues)
            {
                myfl = (JArray)j1.SelectToken("firstline");
                string[] type_array = myfl[0].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                myloctype = type_array[2];
                if (type_array.Count() > 3)
                {
                    for (int i = 0; i < (type_array.Count() - 3); i++)
                    {
                        myloctype += " " + type_array[3 + i];
                    }
                }
            }
            else
            {
                myfl = null;
                myloctype = null;
            }
            _locations.Add(new Location
            {
                _LocId = Convert.ToInt32(InputKey),
                _LocId_Conv = Utilities.To_Oid(InputKey),
                _First_Line = myfl[0].ToString(),
                _Loc_Type = myloctype,
                _Name = myna[0].ToString(),
                _Item_List = JSON.list_Token(j1,"il"),
                _Trade_List = JSON.list_Token(j1, "tl"),
                _nbr_men = 0,
                _LI_Here_List = JSON.list_Token(j1, "LI.hl"),
                _LI_Where = JSON.int_Token(j1, "LI.wh"),
                _LO_Barrier = JSON.int_Token(j1, "LO.ba"),
                _LO_Hidden = JSON.int_Token(j1, "LO.hi"),
                _LO_Civ_Level = JSON.int_Token(j1, "LO.lc"),
                _LO_Province_Destination = JSON.list_Token(j1, "LO.pd"),
                _LO_Shrouded = JSON.int_Token(j1, "LO.sh"), 
                _SL_Building_Materials = JSON.int_Token(j1, "SL.bm"),
                _SL_Capacity = JSON.int_Token(j1, "SL.ca"),
                _SL_Castle_Level = JSON.int_Token(j1, "SL.cl"),
                _SL_Damage = JSON.int_Token(j1, "SL.da"),
                _SL_Defense = JSON.int_Token(j1, "SL.de"),
                _SL_Effort_Given = JSON.int_Token(j1, "SL.eg"),
                _SL_Effort_Required = JSON.int_Token(j1, "SL.er"),
                _SL_Moving = JSON.int_Token(j1, "SL.mo"),
                _SL_Near_Cities = JSON.list_Token(j1, "SL.nc"),
                _SL_Safe = JSON.int_Token(j1, "SL.sh"),
                _SL_Shaft_Depth = JSON.int_Token(j1, "SL.sd"),
                _SL_Teaches = JSON.list_Token(j1, "SL.te")
            });
        }
        public static void Identify_Port_Cities(List<Location> _locations)
        {
            foreach (Location _location in _locations.FindAll(x => x._Loc_Type.Contains("city")))
            {
                if (!_locations.Find(x => x._LocId == _location._LI_Where)._Loc_Type.Contains("mountain"))
                {
                    Location _location2 = _locations.Find(x => x._LocId == _location._LI_Where);
                    if (_locations.Find(x => x._LocId == _location2._LO_Province_Destination[0])._Loc_Type.Contains("ocean") ||
                        _locations.Find(x => x._LocId == _location2._LO_Province_Destination[1])._Loc_Type.Contains("ocean") ||
                        _locations.Find(x => x._LocId == _location2._LO_Province_Destination[2])._Loc_Type.Contains("ocean") ||
                        _locations.Find(x => x._LocId == _location2._LO_Province_Destination[3])._Loc_Type.Contains("ocean"))
                    {
                        _location._Loc_Type = "port city";
                    }
                }
            }
        }
        public static void Set_Region(List<Character> _characters, List<Itemz> _items,  List<Location> _locations, List<Player> _players,  List<Ship> _ships)
        {
            foreach(Character mychar in _characters)
            {
                CurrentLocation mycurrloc = new CurrentLocation();
                mycurrloc._current_loctype = 0;
                CurrentLocation myfinalcurrloc = CurrentLocation.Where_Am_I(mychar._CharId, mycurrloc, _characters, _locations, _ships);
                mychar.Calc_CurrentLoc = myfinalcurrloc._current_loc;
                mychar.Calc_CurrentRegion = myfinalcurrloc._current_region;
            }
            foreach (Ship myship in _ships)
            {
                CurrentLocation mycurrloc = new CurrentLocation();
                CurrentLocation myfinalcurrloc = CurrentLocation.Where_Am_I(myship._ShipId, mycurrloc, _characters, _locations, _ships);
                myship.Calc_CurrentLoc = myfinalcurrloc._current_loc;
                myship.Calc_CurrentRegion = myfinalcurrloc._current_region;
            }
            foreach (Location myloc in _locations.Where(c=>c._LocId != 0 && !c._Loc_Type.Equals("region")))
            {
                CurrentLocation mycurrloc = new CurrentLocation();
                CurrentLocation myfinalcurrloc = CurrentLocation.Where_Am_I(myloc._LocId, mycurrloc, _characters, _locations, _ships);
                myloc.Calc_CurrentLoc = myfinalcurrloc._current_loc;
                myloc.Calc_CurrentRegion = myfinalcurrloc._current_region;
                //if (myloc._LI_Where != 0)
                //{
                //    Console.WriteLine(myloc._LocId_Conv + " = " + Program._locations.Find(k => k._LocId == myloc._LI_Where)._Name);
                //}
            }
            // not sure what to do about this
            foreach (Location _myloc in _locations)
            {
                var return_tuple = new Tuple<int, bool, bool>(_myloc._nbr_men, _myloc._ships_found, _myloc._enemy_found);
                return_tuple = Count_Process_Loc(_myloc._nbr_men, _myloc, _myloc._ships_found, _myloc._enemy_found, _characters, _items, _locations, _players, _ships);
                _myloc._nbr_men = return_tuple.Item1;
                _myloc._ships_found = return_tuple.Item2;
                _myloc._enemy_found = return_tuple.Item3;
                if (_myloc._LI_Where != 0)
                {
                    Location _myloc2 = _locations.Find(x => x._LocId == _myloc._LI_Where);
                    if (_myloc2._Loc_Type.Equals("region"))
                    {
                        _myloc._Region_id = _myloc2._LocId;
                    }
                    else
                    {
                        Location _myloc3 = _locations.Find(x => x._LocId == _myloc2._LI_Where);
                        if (_myloc3._Loc_Type.Equals("region"))
                        {
                            _myloc._Region_id = _myloc3._LocId;
                        }
                        else
                        {
                            Location _myloc4 = _locations.Find(x => x._LocId == _myloc3._LI_Where);
                            if (_myloc4._Loc_Type.Equals("region"))
                            {
                                _myloc._Region_id = _myloc4._LocId;
                            }
                            else
                            {
                                Location _myloc5 = _locations.Find(x => x._LocId == _myloc4._LI_Where);
                                if (_myloc4._Loc_Type.Equals("region"))
                                {
                                    _myloc._Region_id = _myloc4._LocId;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Castle_Indicator(List<Location> _locations)
        {
            string[] indicators = new string[] { "!", "@", "#", "%", "^","*","-","+","~","=","a","b","c","d","e","f","g","h","j"};
            List<Location> mycastles = _locations.FindAll(x => x._Loc_Type.Equals("castle"));
            //Program._players.Sort((x, y) => x._FactionId.CompareTo(y._FactionId));
            mycastles.Sort((x,y) => x._Region_id.CompareTo(y._Region_id));
            int counter = 0;
            int saveregion = 0;
            foreach (Location castle in mycastles)
            {
                if (saveregion != castle._Region_id)
                {
                    counter = 0;
                    saveregion = castle._Region_id;
                    castle._castle_id = indicators[counter];
                    counter++;
                }
                else
                {
                    castle._castle_id = indicators[counter];
                    counter++;
                }
            }
        }
        public static string Return_CI(Location province, List<Character> _characters, List<Location> _locations)
        {
            if (province._LI_Here_List != null)
            {
                foreach (int _hereid in province._LI_Here_List)
                {
                    Character garr = _characters.Find(z => z._CharId == _hereid);
                    if (garr != null)
                    {
                        if (garr._Char_Type.Equals("garrison"))                    
                        {
                            return _locations.Find(y => y._LocId == garr._MI_Garrison_Castle)._castle_id;
                        }
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        private static Tuple <int, bool, bool> Count_Process_Loc(int nbr_men, Location _myloc, bool shipsfound, bool enemyfound, List<Character> _characters,List<Itemz> _items,  List<Location> _locations, List<Player> _players,  List<Ship> _ships)
        {
            var return_tuple = new Tuple<int, bool, bool>(nbr_men, shipsfound, enemyfound);
            if (_myloc._LI_Here_List != null)
            {
                foreach (int _mylochere in _myloc._LI_Here_List)
                {
                    if (_characters.Find(x => x._CharId == _mylochere) != null)
                    {
                        return_tuple = Count_Process_Char(nbr_men, _mylochere, shipsfound, enemyfound, _characters, _items, _players);
                        nbr_men = return_tuple.Item1;
                        shipsfound = return_tuple.Item2;
                        enemyfound = return_tuple.Item3;
                    }
                    else
                    {
                        Ship _myship = _ships.Find(x => x._ShipId == Convert.ToInt32(_mylochere));
                        if (_myship != null)
                        {
                            shipsfound = true;
                            var temp_tuple = new Tuple<int, bool, bool>(nbr_men, shipsfound, enemyfound);
                            return_tuple = temp_tuple;
                            nbr_men = return_tuple.Item1;
                            shipsfound = return_tuple.Item2;
                            enemyfound = return_tuple.Item3;
                            if (_myship._LI_Here_List != null)
                            {
                                foreach (int _myshiphere in _myship._LI_Here_List)
                                {
                                    return_tuple = Count_Process_Char(nbr_men, _myshiphere, shipsfound, enemyfound, _characters, _items, _players);
                                    nbr_men = return_tuple.Item1;
                                    shipsfound = return_tuple.Item2;
                                    enemyfound = return_tuple.Item3;
                                }
                            }
                        }
                        else
                        {
                            if (_locations.Find(x => x._LocId == Convert.ToInt32(_mylochere)) != null)
                            {
                                Location _mylochere2 = _locations.Find(x => x._LocId == Convert.ToInt32(_mylochere));
                                return_tuple = Count_Process_Loc(nbr_men, _mylochere2, shipsfound, enemyfound, _characters, _items, _locations, _players,  _ships);
                                nbr_men = return_tuple.Item1;
                                shipsfound = return_tuple.Item2;
                                enemyfound = return_tuple.Item3;
                            }
                        }
                    }
                }
            }
            return return_tuple;
        }
        private static Tuple<int, bool, bool> Count_Process_Char(int nbr_men, int _mylochere, bool shipsfound, bool enemyfound, List<Character> _characters, List<Itemz> _items, List<Player> _players)
        {
            var return_tuple = new Tuple<int, bool, bool>(nbr_men, shipsfound, enemyfound);
            Character _mychar = _characters.Find(x => x._CharId == _mylochere);
            if (_mychar._Item_List != null)
            {

                // ignore city garrison, but not stuff under them
                if (_mychar._Char_Type.Equals("garrison") && _mychar._LI_Where >= 56760 && _mychar._LI_Where <= 58759)
                { }
                else
                {
                    int iterations = _mychar._Item_List.Count / 2;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (_items.Find(x => x._ItemId == _mychar._Item_List[(i * 2) + 0])._IT_Prominent == 1)
                        {
                            nbr_men += _mychar._Item_List[(i * 2) + 1];
                        }
                    }
                    if (_players.Find(x=>x._FactionId == _mychar._PlayerId)._FactionId_Conv.Equals("100"))
                    {
                        enemyfound = true;
                    }
                }
                var temp_tuple = new Tuple<int, bool, bool>(nbr_men, shipsfound, enemyfound);
                return_tuple = temp_tuple;
                nbr_men = return_tuple.Item1;
                shipsfound = return_tuple.Item2;
                enemyfound = return_tuple.Item3;
            }
            // see if other characters stacked under
            if (_mychar._LI_Here_List != null)
            {
                foreach (int _mycharhere in _mychar._LI_Here_List)
                {
                    return_tuple = Count_Process_Char(nbr_men, _mycharhere, shipsfound, enemyfound, _characters, _items, _players);
                    nbr_men = return_tuple.Item1;
                    shipsfound = return_tuple.Item2;
                    enemyfound = return_tuple.Item3;
                }
            }
            return return_tuple;
        }
        public static int Calc_Exit_Distance(Location _loc1, Location _loc2)
        {
            if (_loc1._Loc_Type == "pit" || _loc2._Loc_Type == "pit")
            {
                return 28;
            }
            if (loc_depth(_loc1) > loc_depth(_loc2))
            {
                Location tmp;

                tmp = _loc1;
                _loc1 = _loc2;
                _loc2 = tmp;
            }

            int w_d = loc_depth(_loc1);
            int d_d = loc_depth(_loc2);

            if (d_d == 4)
                return 0;
            if (d_d == 3)
                return 1;

            if (_loc1._Loc_Type == "ocean" && _loc2._Loc_Type != "ocean")
            {
                return 2;
            }
            if (_loc1._Loc_Type != "ocean" && _loc2._Loc_Type == "ocean")
            {
                return 2;
            }
            //
            // skipping province logic
            //
            switch (_loc2._Loc_Type)
            {
                case "ocean":
                    // skipping sea lane logic
                    return 3;
                case "mountain": return 10;
                case "forest": return 8;
                case "swamp": return 14;
                case "desert": return 8;
                case "plain": return 7;
                case "underground": return 7;
                case "cloud": return 7;
                case "tunnel": return 5;
                case "chamber": return 5;
            }
            return 0;
        }
        public static int loc_depth(Location loc)
        {
            switch (loc._Loc_Type)
            {
                case "region": return 1;
                case "ocean":
                case "plain":
                case "forest":
                case "mountain":
                case "desert":
                case "swamp":
                case "underground":
                case "cloud":
                case "tunnel":
                case "chamber":  return 2;
                case "island":
                case "ring of stones":
                case "mallorn grove":
                case "bog":
                case "city":
                case "port city":
                case "lair":
                case "graveyard":
                case "ruins":
                case "battlefield":
                case "enchanted forest":
                case "rocky hill":
                case "circle of trees":
                case "pits":
                case "pasture":
                case "oasis":
                case "yew grove":
                //case "sand pit":
                case "sacred grove":
                case "poppy field":
                case "faery hill":
                case "sand pit": return 3;
                case "temple":
                case "galley":
                case "roundship":
                case "castle":
                case "gally-in-progress":
                case "roundship-in-progress":
                case "ghost ship":
                case "temple-in-progress":
                case "inn":
                case "inn-in-progress":
                case "castle-in-progress":
                case "mine":
                case "mine-in-progress":
                case "collapsed mine":
                case "tower":
                case "tower-in-progress":
                case "sewer": return 4;
            }
            return 0;
        }
        public static int Province_Has_Port_City(Location myloc, List<Location> _locations)
        {
            if (myloc._LI_Here_List != null)
            {
                foreach (int dest in myloc._LI_Here_List)
                {
                    Location _mylochere2 = _locations.Find(x => x._LocId == dest);
                    if (_mylochere2 != null)
                    {
                        if (_mylochere2._Loc_Type == "port city")
                        {
                            return _mylochere2._LocId;
                        }
                    }
                }
            }
            return 0;
        }
    } 
}
