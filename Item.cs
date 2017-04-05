using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Itemz
    {
        public int _ItemId { get; set; }
        public string _ItemId_Conv { get; set; }
        public string _Item_Type { get; set; }
        public string _Name { get; set; }
        public string _First_Line { get; set; }
        public string _Plural { get; set; }
        public int _Weight { get; set; }
        public int _Land_Capacity { get; set; }
        public int _Ride_Capacity { get; set; }
        public int _Fly_Capacity { get; set; }
        public int _IT_Prominent { get; set; }
        public int _IT_Man_Item { get; set; }
        public int _IT_Animal { get; set; }
        public int _IT_Attack { get; set; }
        public int _IT_Defense { get; set; }
        public int _IT_Missile { get; set; }
        public int _IT_Who_Has { get; set; }
        public int _IM_Aura_Bonus { get; set; }
        public int _IM_May_Study { get; set; }
        public int _IM_Use_Key { get; set; }
        public int _IM_Attack_Bonus { get; set; }
        public int _IM_Defense_Bonus { get; set; }
        public int _IM_Missile_Bonus { get; set; }
        public int _IM_Aura { get; set; }
        public int _IM_Project_Cast { get; set; }
        public int _IM_Lore { get; set; }
        public int _PL_Unit { get; set; }
        public static void Add(string InputKey, string InputString, List<Itemz> _items)
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
            string myitemtype;
            if (j1.SelectToken("firstline") != null && j1.SelectToken("firstline").HasValues)
            {
                myfl = (JArray)j1.SelectToken("firstline");
                string[] type_array = myfl[0].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                myitemtype = type_array[2];
            }
            else
            {
                myfl = null;
                 myitemtype = null;
            }

            _items.Add(new Itemz
            {
                _ItemId = Convert.ToInt32(InputKey),
                _ItemId_Conv = Utilities.To_Oid(InputKey),
                _First_Line = myfl[0].ToString(),
                _Name = myna[0].ToString(),
                _Item_Type = myitemtype,
                _IM_Attack_Bonus = JSON.int_Token(j1, "IM.ab"),
                _IM_Aura = JSON.int_Token(j1, "IM.au"),
                _IM_Aura_Bonus = JSON.int_Token(j1, "IM.ba"),
                _IM_Defense_Bonus = JSON.int_Token(j1, "IM.db"),
                _IM_May_Study = JSON.int_Token(j1, "IM.ms"),
                _IM_Missile_Bonus = JSON.int_Token(j1, "IM.mb"),
                _IM_Use_Key = JSON.int_Token(j1, "IM.uk"),
                _IT_Animal = JSON.int_Token(j1, "IT.an"),
                _IT_Attack = JSON.int_Token(j1, "IT.at"),
                _IT_Defense = JSON.int_Token(j1, "IT.df"),
                _Fly_Capacity = JSON.int_Token(j1, "IT.fc"),
                _Land_Capacity = JSON.int_Token(j1, "IT.lc"),
                _IM_Lore = JSON.int_Token(j1, "IM.lo"),
                _IT_Missile = JSON.int_Token(j1, "IT.mi"),
                _IM_Project_Cast = JSON.int_Token(j1, "IM.pc"),
                _Plural = JSON.string_Token(j1, "IT.pl"),
                _IT_Prominent = JSON.int_Token(j1, "IT.pr"),
                _IT_Man_Item = JSON.int_Token(j1, "IT.mu"),
                _Ride_Capacity = JSON.int_Token(j1, "IT.rc"),
                _Weight = JSON.int_Token(j1, "IT.wt"),
                _IT_Who_Has = JSON.int_Token(j1, "IT.un"),
                _PL_Unit = JSON.int_Token(j1, "PL.un")
            });
        }
        public static string Determine_Use (Itemz _myitem, List<Character> _characters,  List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            //if (_myitem._Item_Type.Equals("0"))
            //{
            //    return "";
            //}
            if (_myitem._Item_Type.Equals("0"))
            {
                if (_myitem._IM_Use_Key != 0)
                {
                    if (_myitem._IM_Use_Key.Equals(1))
                    {
                        return "death potion";
                    }
                    if (_myitem._IM_Use_Key.Equals(2))
                    {
                        return "healing potion";
                    }
                    if (_myitem._IM_Use_Key.Equals(3))
                    {
                        return "slave potion";
                    }
                    if (_myitem._IM_Use_Key.Equals(4))
                    {
                        return "palantir";
                    }
                    if (_myitem._IM_Use_Key.Equals(5))
                    {
                        StringBuilder outline = new StringBuilder();
                        outline.Append("projected cast potion");
                        if (_myitem._IM_Project_Cast != 0)
                        {
                            Location _myloc = _locations.Find(x=>x._LocId == _myitem._IM_Project_Cast);
                            if (_myloc != null)
                            {
                                outline.Append(", " + _myloc._Name + " " + Utilities.Format_Anchor( _myloc._LocId_Conv) );
                            }
                            else
                            {
                                Ship _myship = _ships.Find(x => x._ShipId == _myitem._IM_Project_Cast);
                                if (_myship != null)
                                {
                                    outline.Append(", " + _myship._Name + " " + Utilities.Format_Anchor(_myship._ShipId.ToString()));
                                }
                                else
                                {
                                    outline.Append(", target [" + _myitem._IM_Project_Cast + "] no longer exists");
                                }
                            }
                        }
                        return outline.ToString();
                    }
                    if (_myitem._IM_Use_Key.Equals(6))
                    {
                        return "quick cast potion";
                    }
                    if (_myitem._IM_Use_Key.Equals(7))
                    {
                        // drum, nothing for now
                        return "";
                    }
                    if (_myitem._IM_Use_Key.Equals(8))
                    {
                        return "elf stone";
                    }
                    if (_myitem._IM_Use_Key.Equals(9))
                    {
                        return "orb";
                    }
                    return "undefined use: " + _myitem._IM_Use_Key;
                }
                else
                {
                    return "";
                }
            }
            if (_myitem._Item_Type.Equals("artifact"))
            {
                StringBuilder outline = new StringBuilder();
                if (_myitem._IM_Attack_Bonus != 0)
                {
                    if (outline.Length > 0)
                    {
                        outline.Append(", ");
                    }
                    outline.Append("attack +" + _myitem._IM_Attack_Bonus);
                }
                if (_myitem._IM_Defense_Bonus != 0)
                {
                    if (outline.Length > 0)
                    {
                        outline.Append(", ");
                    }
                    outline.Append("defense +" + _myitem._IM_Defense_Bonus);
                }
                if (_myitem._IM_Missile_Bonus != 0)
                {
                    if (outline.Length > 0)
                    {
                        outline.Append(", ");
                    }
                    outline.Append("missile +" + _myitem._IM_Missile_Bonus);
                }
                if (_myitem._IM_Aura_Bonus != 0)
                {
                    if (outline.Length > 0)
                    {
                        outline.Append(", ");
                    }
                    outline.Append("aura +" + _myitem._IM_Aura_Bonus);
                }
                return outline.ToString();
            }
            if (_myitem._Item_Type.Equals("dead body"))
            {
                Character _mychar = _characters.Find(x => x._CharId == _myitem._PL_Unit);
                if (_mychar != null)
                {
                    return ("Formerly known as: " + _mychar._Name + " [" + Utilities.Format_Anchor(_mychar._CharId.ToString()) + "]");
                }
                else
                {
                    return "dead, deader, deadest";
                }
            }
            if (_myitem._Item_Type.Equals("npc_token"))
            {
                if (_myitem._PL_Unit != 0)
                {
                    Character _mychar = _characters.Find(x => x._CharId == _myitem._PL_Unit);
                    if (_mychar != null)
                    {
                        return ("Controls: " + _mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()));
                    }
                    else
                    {
                        return "undefined";
                    }
                }
                else
                {
                    return "undefined";
                }
            }
            if (_myitem._Item_Type.Equals("auraculum"))
            {
                return "aura: " + _myitem._IM_Aura;
            }
            if (_myitem._Item_Type.Equals("scroll"))
            {
                Skill _myskill = _skills.Find(x => x._SkillId == _myitem._IM_May_Study);
                if (_myskill != null)
                {
                    return "study: " + _myskill._Name + " [" + _myitem._IM_May_Study + "]";
                }
                else
                {
                    return "";
                }
            }
            if (_myitem._Item_Type.Equals("tradegood"))
            {
                StringBuilder outline = new StringBuilder();
                List<Location> _myloc = _locations.FindAll(x => x._Trade_List != null);
                if (_myloc != null)
                {
                    foreach (Location _myloctrade in _myloc)
                    {
                        int iterations = _myloctrade._Trade_List.Count / 8;
                        for (int i = 0; i < iterations; i++)
                        {
                            if ((_myloctrade._Trade_List[(i * 8) + 1]) == _myitem._ItemId)
                            {
                                if (_myloctrade._Trade_List[(i * 8) + 0] == 1 || _myloctrade._Trade_List[(i * 8) + 0] == 2)
                                {
                                    if (_myloctrade._Trade_List[(i * 8) + 0] == 1)
                                    {
                                        if (outline.Length > 0)
                                        {
                                            outline.Append(", ");
                                        }
                                        outline.Append("buy: " + _myloctrade._Name + " " + Utilities.Format_Anchor(_myloctrade._LocId_Conv));
                                    }
                                    if (_myloctrade._Trade_List[(i * 8) + 0] == 2)
                                    {
                                        if (outline.Length > 0)
                                        {
                                            outline.Append(", ");
                                        }
                                        outline.Append("sell: " + _myloctrade._Name + " " + Utilities.Format_Anchor(_myloctrade._LocId_Conv));
                                    }
                                }
                            }
                        }
                    }
                    if (outline.Length > 0)
                    {
                        return outline.ToString();
                    }
                    else
                    {
                        return "inactive";
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        public static bool Is_Fighter(Itemz myitem)
        {
            if (myitem._IT_Attack > 0
                || myitem._IT_Defense > 0
                || myitem._IT_Missile > 0
                || myitem._ItemId == 18)
            {
                return true;
            }
            return false;
        }
        public static Weight Add_Item_Weight(Itemz myitem, int qty, Weight myweight)
        {
            int item_weight = myitem._Weight * qty;
            if (myitem._Land_Capacity > 0)
            {
                myweight._land_cap += myitem._Land_Capacity * qty;
            }
            else
            {
                myweight._land_weight += item_weight;
            }
            if (myitem._Ride_Capacity > 0)
            {
                myweight._ride_cap += myitem._Ride_Capacity * qty;
            }
            else
            {
                myweight._ride_weight +=  item_weight;
            }
            if (myitem._Fly_Capacity > 0)
            {
                myweight._fly_cap += myitem._Fly_Capacity * qty;
            }
            else
            {
                myweight._fly_weight += item_weight;
            }
            myweight._total_weight += item_weight;
            if(myitem._IT_Animal == 1)
            {
                myweight._animals += qty;
            }
            return myweight;
        }
    }
}
