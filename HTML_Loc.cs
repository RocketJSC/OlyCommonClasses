using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class HTML_Loc
    {
        public static void Write_Loc_HTML_File(Location _myloc, string path, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Player> _players,  List<Ship> _ships, List<Skill> _skills, List<Storm> _storms, List<TradeXref> _TradeXref_Unsorted)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, _myloc._LocId_Conv + ".html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>" + _myloc._Name +
                                " [" + _myloc._LocId_Conv + "], " +
                                _myloc._Loc_Type + ", in " +
                                (_locations.Find(x => x._LocId == Convert.ToInt32(_myloc._LI_Where))._Name) +
                                "</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    Write_Loc_Map_Anchor(_myloc, w, _locations);
                    Write_Loc_Page_Header(_myloc, w, _locations);
                    Write_Barrier(_myloc, w);
                    Write_Shroud(_myloc, w);
                    Write_Controlled_By(_myloc, w, _characters, _locations);
                    Write_Routes_Out(_myloc, w, _locations);
                    Write_Nearby_Cities(_myloc, w, _locations);
                    Write_Structure_Basic_Info(_myloc, w);
                    Write_Skills_Report(_myloc, w, _skills);
                    Write_Market_Report(_myloc, w, _characters, _items, _TradeXref_Unsorted, _locations);
                    // Print Here List (Inner Locations and Seen Here)
                    if (_myloc._LI_Here_List != null)
                    {
                        Write_Here_List_HTML(_myloc, w, _characters, _items, _locations, _ships, _storms);
                    }
                    // if hidden, print which factions have accessed it
                    if (_myloc._LO_Hidden == 1)
                    {
                        Write_Hidden_Accesses_HTML(_myloc, w, _players);
                    }
                    Write_Loc_Map_Anchor(_myloc, w, _locations);
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }

        private static void Write_Barrier(Location _myloc, StreamWriter w)
        {
            if (_myloc._LO_Barrier > 0)
            {
                w.WriteLine("<p>A magical barrier surrounds {0} [{1}].</p>", _myloc._Name, _myloc._LocId_Conv);
            }
        }
        private static void Write_Shroud(Location _myloc, StreamWriter w)
        {
            if (_myloc._LO_Shrouded > 0)
            {
                w.WriteLine("<p>A magical shroud surrounds {0} [{1}].</p>", _myloc._Name, _myloc._LocId_Conv);
            }
        }
        private static void Write_Loc_Map_Anchor(Location _myloc, StreamWriter w, List<Location> _locations)
        {
            if (_myloc._LocId >= 10000 && _myloc._LocId < 18000)
            {
                int x_coord = (10 * (int)((_myloc._LocId % 100) / 10));
                if (x_coord >= 70)
                {
                    x_coord = 60;
                }
                int y_coord = (1000 * (_myloc._LocId / 1000));
                if (y_coord >= 17000)
                {
                    y_coord = 16000;
                }
                w.WriteLine("<p>" + 
                    Utilities.Format_Anchor2("main_map_leaf_" + 
                    Utilities.To_Oid((y_coord + x_coord).ToString()), 
                    "Return to map</p>"));
            }
            else
            {
                if (_myloc._LocId >= 18000 && _myloc._LocId < 24000)
                {
                    int x_coord = (10 * (int)((_myloc._LocId % 100) / 10));
                    if (x_coord >= 50)
                    {
                        x_coord = 40;
                    }
                    int y_coord = (1000 * (_myloc._LocId / 1000));
                    if (y_coord >= 23000)
                    {
                        y_coord = 22000;
                    }
                    w.WriteLine("<p>" + Utilities.Format_Anchor2("faery_map_leaf_" + Utilities.To_Oid((y_coord + x_coord).ToString()), "Return to map</p>"));
                }
                else
                {
                    if (_myloc._LocId >= 24000 && _myloc._LocId < 30000)
                    {
                        int x_coord = (10 * (int)((_myloc._LocId % 100) / 10));
                        if (x_coord >= 50)
                        {
                            x_coord = 40;
                        }
                        int y_coord = (1000 * (_myloc._LocId / 1000));
                        if (y_coord >= 29000)
                        {
                            y_coord = 28000;
                        }
                        w.WriteLine("<p>" + Utilities.Format_Anchor2("hades_map_leaf_" + Utilities.To_Oid((y_coord + x_coord).ToString()), "Return to map</p>"));
                    }
                    else
                    {
                        // only works if in main map
                        if ((_myloc._LocId >= 56760 && _myloc._LocId < 58760) || (_myloc._LocId >= 59000 && _myloc._LocId < 79000))
                        {
                            string[] loc_array =_myloc.Calc_CurrentLoc.Split('|');
                            Location _myloc2 = _locations.Find(x=> x._LocId == Convert.ToInt32(Utilities.To_Int(loc_array[loc_array.Count() - 1])));
                            if (_myloc2 != null)
                            {
                                if (_myloc2._LocId >= 10000 && _myloc2._LocId < 18000)
                                {
                                    int x_coord = (10 * (int)((_myloc2._LocId % 100) / 10));
                                    if (x_coord >= 70)
                                    {
                                        x_coord = 60;
                                    }
                                    int y_coord = (1000 * (_myloc2._LocId / 1000));
                                    if (y_coord >= 17000)
                                    {
                                        y_coord = 16000;
                                    }
                                    w.WriteLine("<p>" +
                                        Utilities.Format_Anchor2("main_map_leaf_" +
                                        Utilities.To_Oid((y_coord + x_coord).ToString()),
                                        "Return to map</p>"));
                                }
                            }
                        }
                        else
                        {
                            // also need to write it for structures
                        }
                    }
                }
            }
        }

        private static void Write_Loc_Page_Header(Location _myloc, StreamWriter w, List<Location> _locations)
        {
            StringBuilder outline3 = new StringBuilder();
            outline3.Append("<H3>");
            outline3.Append(_myloc._Name);
            outline3.Append(" [");
            outline3.Append(_myloc._LocId_Conv);
            outline3.Append("], ");
            outline3.Append(_myloc._Loc_Type);
            outline3.Append(", in ");
            if (_myloc._Loc_Type.Contains("city"))
            {
                outline3.Append("province ");
            }
            outline3.Append((_locations.Find(x => x._LocId == Convert.ToInt32(_myloc._LI_Where))._Name));
            if (_myloc._Loc_Type.Contains("city"))
            {
                outline3.Append(" [");
                outline3.Append((_locations.Find(x => x._LocId == Convert.ToInt32(_myloc._LI_Where))._LocId_Conv));
                outline3.Append("]");
            }
            if (_myloc._SL_Safe == 1)
            {
                outline3.Append(", safe haven");
            }
            if (_myloc._LO_Hidden == 1)
            {
                outline3.Append(", hidden");
            }
            // civ level only for provinces, not sublocations
            if (_myloc._LocId >= 10000 && _myloc._LocId < 18000)
            {
                outline3.Append(", " + (_myloc._LO_Civ_Level.Equals(0) ? "wilderness" : "civ-" + _myloc._LO_Civ_Level));
            }
            outline3.Append("</H3>");
            w.WriteLine(outline3);
        }
        private static void Write_Controlled_By(Location _myloc, StreamWriter w, List<Character> _characters,  List<Location> _locations)
        {
            // Print Province Controlled By
            if (_myloc._LI_Here_List != null)
            {
                foreach (int _my_inner in _myloc._LI_Here_List)
                {
                    Character _my_dest_char = _characters.Find(x => x._CharId == Convert.ToInt32(_my_inner));
                    if (_my_dest_char != null)
                    {
                        if (!_my_dest_char._MI_Garrison_Castle.Equals(0))
                        {
                            StringBuilder outline = new StringBuilder();
                            outline.Append("Province controlled by ");
                            Location _my_dest_loc = _locations.Find(x => x._LocId == Convert.ToInt32(_my_dest_char._MI_Garrison_Castle));
                            if (_my_dest_loc != null)
                            {
                                outline.Append(_my_dest_loc._Name + " ");
                                outline.Append(Utilities.Format_Anchor(_my_dest_loc._LocId_Conv));
                                outline.Append(", " + _my_dest_loc._Loc_Type);
                                Location _my_dest_loc2 = _locations.Find(x => x._LocId == Convert.ToInt32(_my_dest_loc._LI_Where));
                                if (_my_dest_loc2 != null)
                                {
                                    // see if in city.  If so, get province location
                                    if (!_my_dest_loc2._Loc_Type.Contains("city"))
                                    {
                                        outline.Append(" in ");
                                        outline.Append(_my_dest_loc2._Name + " ");
                                        outline.Append(Utilities.Format_Anchor(_my_dest_loc2._LocId_Conv));
                                    }
                                    else
                                    {
                                        Location _my_dest_loc3 = _locations.Find(x => x._LocId == Convert.ToInt32(_my_dest_loc2._LI_Where));
                                        if (_my_dest_loc3 != null)
                                        {
                                            outline.Append(" in ");
                                            outline.Append(_my_dest_loc3._Name + " ");
                                            outline.Append(Utilities.Format_Anchor(_my_dest_loc3._LocId_Conv));
                                        }
                                    }
                                }
                            }
                            w.WriteLine(outline);
                            outline.Clear();
                            //// ruled by
                            if (_my_dest_loc._LI_Here_List != null)
                            {
                                Character _my_dest_char2 = _characters.Find(x => x._CharId == Convert.ToInt32(_my_dest_loc._LI_Here_List[0]));
                                if (_my_dest_char2 != null)
                                {
                                    outline.Append("<br>Ruled by ");
                                    Character top_dog;
                                    if (_my_dest_char2.Ultimate_Lord != 0)
                                    {
                                        top_dog = _characters.Find(x => x._CharId == _my_dest_char2.Ultimate_Lord);
                                    }
                                    else
                                    {
                                        top_dog = _my_dest_char2;
                                    }
                                    outline.Append(top_dog._Name + " " + Utilities.Format_Anchor(top_dog._CharId.ToString()));
                                    outline.Append(", " + Utilities.Xlate_Rank(top_dog._CH_Rank));
                                    //outline.Append(_my_dest_char2._Name);
                                    //outline.Append("," + _my_dest_char2._CH_Rank);
                                    w.WriteLine(outline);
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private static void Write_Routes_Out(Location _myloc, StreamWriter w, List<Location> _locations)
        {
            // Print Routes Out
            if (!_myloc._Loc_Type.Contains("city"))
            {
                if (_myloc._LO_Province_Destination != null)
                {
                    Write_Province_Destinations_HTML(_myloc, w, _locations);
                }
            }
            else
            {
                w.WriteLine("<H4>Routes leaving " + _myloc._Name + ":</H4>");
                StringBuilder outline = new StringBuilder();
                w.WriteLine("<ul>");
                if (_myloc._Loc_Type.Contains("city"))
                {
                    // to ocean provinces (city province can't be a mountain)
                    if (!_locations.Find(x => x._LocId == _myloc._LI_Where)._Loc_Type.Equals("mountain"))
                    {
                        Location _locations1 = _locations.Find(x => x._LocId == _myloc._LI_Where);
                        int dest_count = -1;
                        foreach (int _destloc in _locations1._LO_Province_Destination)
                        {
                            dest_count++;
                            if (_locations.Find(x => x._LocId == _destloc)._Loc_Type.Equals("ocean"))
                            {
                                outline.Append("<li>");
                                string direction;
                                switch (dest_count)
                                {
                                    case 0: direction = "North"; break;
                                    case 1: direction = "East"; break;
                                    case 2: direction = "South"; break;
                                    case 3: direction = "West"; break;
                                    default: direction = "Undefined"; break;
                                }
                                outline.Append(direction);
                                outline.Append(", to ");
                                outline.Append(_locations.Find(x => x._LocId == _destloc)._Name + " ");
                                outline.Append(Utilities.Format_Anchor((_locations.Find(x => x._LocId == _destloc)._LocId_Conv)));
                                // get region name
                                outline.Append(", " + _locations.Find(y => y._LocId == _locations.Find(x => x._LocId == _destloc)._LI_Where)._Name);
                                outline.Append(", 1 day");
                                outline.Append("</li>");
                            }
                        }
                    }
                }
                // to province
                outline.Append("<li>");
                outline.Append("Out, to ");
                outline.Append(_locations.Find(x => x._LocId == _myloc._LI_Where)._Name + " ");
                outline.Append(Utilities.Format_Anchor(_locations.Find(x => x._LocId == _myloc._LI_Where)._LocId_Conv));
                outline.Append(", 1 day");
                outline.Append("</li>");
                w.WriteLine(outline);
                w.WriteLine("</ul>");
            }
        }

        private static void Write_Nearby_Cities(Location _myloc, StreamWriter w, List<Location> _locations)
        {
            // cities near by
            if (_myloc._SL_Near_Cities != null)
            {
                w.WriteLine("<H4>Cities rumored to be nearby:</H4>");
                w.WriteLine("<ul>");
                foreach (int locationnb in _myloc._SL_Near_Cities)
                {
                    StringBuilder outline = new StringBuilder();
                    Location _nearby_city = _locations.Find(x => x._LocId == locationnb);
                    Location _city_province = _locations.Find(x => x._LocId == _nearby_city._LI_Where);
                    outline.Append("<li>");
                    outline.Append(_nearby_city._Name + " ");
                    outline.Append(Utilities.Format_Anchor(_nearby_city._LocId_Conv));
                    outline.Append(", in " + _city_province._Name + " ");
                    outline.Append(Utilities.Format_Anchor(_city_province._LocId_Conv));
                    outline.Append("</li>");
                    w.WriteLine(outline);
                }
                w.WriteLine("</ul>");
            }
        }

        private static void Write_Skills_Report(Location _myloc, StreamWriter w, List<Skill> _skills)
        {
            // Skills
            if (_myloc._SL_Teaches != null)
            {
                w.WriteLine("<H4>Skills taught here:</H4>");
                w.WriteLine("<ul>");
                foreach (int skill in _myloc._SL_Teaches)
                {
                    Skill _myskill = _skills.Find(x => x._SkillId == skill);
                    w.WriteLine("<li>" + _myskill._Name + " [" + skill + "]</li>");
                }
                w.WriteLine("</ul>");
            }
        }

        private static void Write_Market_Report(Location _myloc, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<TradeXref> _TradeXref_Unsorted, List<Location> _locations)
        {
            // Market Report
            if (_myloc._Trade_List != null)
            {
                // create consolidated trade list (location and characters)
                List<Trade> _trades = new List<Trade>();
                int iterations = _myloc._Trade_List.Count / 8;
                for (int i = 0; i < iterations; i++)
                {
                    if (_myloc._Trade_List[(i * 8) + 0] == 1
                    || _myloc._Trade_List[(i * 8) + 0] == 2)
                    {
                        Itemz _myitem = _items.Find(x => x._ItemId == _myloc._Trade_List[(i * 8) + 1]);
                        if (_myitem != null)
                        {
                            _trades.Add(new Trade
                            {
                                _ItemId = _myitem._ItemId,
                                _ItemId_Conv = _myitem._ItemId_Conv,
                                _Who_id = _myloc._LocId,
                                _Who_id_Conv = _myloc._LocId_Conv,
                                _Who_Type = 1,
                                _Trade_Kind = _myloc._Trade_List[(i * 8) + 0],
                                _Number = _myloc._Trade_List[(i * 8) + 2],
                                _Price = _myloc._Trade_List[(i * 8) + 3],
                                _Weight = _myitem._Weight,
                                _Item_Name = _myitem._Name,
                                _recip_Number = 0,
                                _recip_Price = 0,
                                _recip_Trade_Kind = 0,
                                _recip_Who_id = 0,
                                _recip_Who_id_Conv = null,
                                _recip_Who_Type = 0
                            });
                            // reread row just added
                            var Trade = (_trades.Find(x => x._ItemId == _myitem._ItemId));
                            if (Trade != null)
                            {
                                // lookup in tradexref
                                TradeXref _my_tradexref = _TradeXref_Unsorted.Find(t => t._ItemId == _myitem._ItemId);
                                if (_my_tradexref != null)
                                {
                                    // post reciprocal data on Trade 
                                    // buy
                                    if (Trade._Trade_Kind == 1)
                                    {
                                        Trade._recip_Trade_Kind = 2;
                                        Trade._recip_Who_id = _my_tradexref._Sell_Who_id;
                                        Trade._recip_Who_id_Conv = _my_tradexref._Sell_Who_id_Conv;
                                        Trade._recip_Number = _my_tradexref._Sell_Number;
                                        Trade._recip_Price = _my_tradexref._Sell_Price;
                                        Trade._recip_Who_Type = _my_tradexref._Sell_Who_Type;
                                    }
                                    else
                                    {
                                        // sell
                                        if (Trade._Trade_Kind == 2)
                                        {
                                            Trade._recip_Trade_Kind = 1;
                                            Trade._recip_Who_id = _my_tradexref._Buy_Who_id;
                                            Trade._recip_Who_id_Conv = _my_tradexref._Buy_Who_id_Conv;
                                            Trade._recip_Number = _my_tradexref._Buy_Number;
                                            Trade._recip_Price = _my_tradexref._Buy_Price;
                                            Trade._recip_Who_Type = _my_tradexref._Buy_Who_Type;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                List<Character> _chartradelist = _characters.FindAll(x => x._LI_Where == _myloc._LocId && x._Trade_List != null);
                if (_chartradelist.Count > 0)
                {
                    foreach (Character _mychar in _chartradelist)
                    {
                        int iterations2 = _mychar._Trade_List.Count / 8;
                        for (int i = 0; i < iterations2; i++)
                        {
                            if (_mychar._Trade_List[(i * 8) + 0] == 1
                            || _mychar._Trade_List[(i * 8) + 0] == 2)
                            {
                                Itemz _myitem = _items.Find(x => x._ItemId == Convert.ToInt32(_mychar._Trade_List[(i * 8) + 1]));
                                if (_myitem != null)
                                {
                                    _trades.Add(new Trade
                                    {
                                        _ItemId = _myitem._ItemId,
                                        _ItemId_Conv = _myitem._ItemId_Conv,
                                        _Who_id = _mychar._CharId,
                                        _Who_id_Conv = _mychar._CharId.ToString(),
                                        _Who_Type = 2,
                                        _Trade_Kind = Convert.ToInt32(_mychar._Trade_List[(i * 8) + 0]),
                                        _Number = Convert.ToInt32(_mychar._Trade_List[(i * 8) + 2]),
                                        _Price = Convert.ToInt32(_mychar._Trade_List[(i * 8) + 3]),
                                        _Weight = _myitem._Weight,
                                        _Item_Name = _myitem._Name,
                                        _recip_Number = 0,
                                        _recip_Price = 0,
                                        _recip_Trade_Kind = 0,
                                        _recip_Who_id = 0,
                                        _recip_Who_id_Conv = null,
                                        _recip_Who_Type = 0
                                    });
                                }
                            }
                        }
                    }
                }
                List<Trade> _trades_sorted = _trades.OrderBy(z => z._Trade_Kind).ThenBy(a => a._Who_id_Conv).ThenBy(b => b._ItemId_Conv).ToList();
                //  write report
                w.WriteLine("<H4>" + Utilities.Format_Anchor2("master_trade_buy_list","Market Report:") + "</H4>");
                w.WriteLine("<table border=\"1\" cellpadding=\"5\">");
                w.WriteLine("<tr><th>trade</th><th>who</th><th>price</th><th>qty</th><th>wt/ea</th><th>item</th><th>recip who</th><th>recip price</th><th>recip qty</th></tr>");
                foreach (Trade _mytrade in _trades_sorted)
                {
                    StringBuilder outline = new StringBuilder();
                    outline.Append("<tr><td>");
                    outline.Append(_mytrade._Trade_Kind == 1 ? "buy" : "sell");
                    outline.Append("</td><td>");
                    outline.Append(_mytrade._Who_id_Conv);
                    outline.Append("</td><td>");
                    outline.Append(_mytrade._Price);
                    outline.Append("</td><td>");
                    outline.Append(_mytrade._Number);
                    outline.Append("</td><td>");
                    outline.Append(_mytrade._Weight);
                    outline.Append("</td><td>");
                    outline.Append(_mytrade._Item_Name + " [" + _mytrade._ItemId_Conv + "]");
                    outline.Append("</td>");
                    if (_mytrade._recip_Who_id_Conv != null)
                    {
                        outline.Append("<td>" + _locations.Find(l=>l._LocId == _mytrade._recip_Who_id)._Name + " " +  Utilities.Format_Anchor(_mytrade._recip_Who_id_Conv) + "</td>");
                        outline.Append("<td>" + _mytrade._recip_Price + "</td>");
                        outline.Append("<td>" + _mytrade._recip_Number + "</td>");
                    }
                    else
                    {
                        outline.Append("<td>&nbsp;</td>");
                        outline.Append("<td>&nbsp;</td>");
                        outline.Append("<td>&nbsp;</td>");
                    }
                    outline.Append("</tr>");
                    w.WriteLine(outline);
                }
                w.WriteLine("</Table>");
            }
        }

        public static void Write_Province_Destinations_HTML(Location _myloc, StreamWriter w, List<Location> _locations)
        {
            w.WriteLine("<H4>Routes leaving " + _myloc._Name + ":</H4>");
            w.WriteLine("<ul>");
            if (_myloc._LO_Province_Destination.Count.CompareTo(0) > 0)
            {
                if (_myloc._LO_Province_Destination[0] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[0], w, "North", _myloc._Loc_Type, _locations);
                }
            }
            if (_myloc._LO_Province_Destination.Count.CompareTo(1) > 0)
            {
                if (_myloc._LO_Province_Destination[1] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[1], w, "East", _myloc._Loc_Type, _locations);
                }
            }
            if (_myloc._LO_Province_Destination.Count.CompareTo(2) > 0)
            {
                if (_myloc._LO_Province_Destination[2] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[2], w, "South", _myloc._Loc_Type, _locations);
                }
            }
            if (_myloc._LO_Province_Destination.Count.CompareTo(3) > 0)
            {
                if (_myloc._LO_Province_Destination[3] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[3], w, "West", _myloc._Loc_Type, _locations);
                }
            }
            if (_myloc._LO_Province_Destination.Count.CompareTo(4) > 0)
            {
                if (_myloc._LO_Province_Destination[4] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[4], w, "Up", _myloc._Loc_Type, _locations);
                }
            }
            if (_myloc._LO_Province_Destination.Count.CompareTo(5) > 0)
            {
                if (_myloc._LO_Province_Destination[5] != 0)
                {
                    Write_Province_Destination_HTML(_myloc._LO_Province_Destination[5], w, "Down", _myloc._Loc_Type, _locations);
                }
            }
            if (_locations.Find(x=>x._LocId == _myloc._LI_Where)._Loc_Type != "region")
            {
                Write_Province_Destination_HTML(_myloc._LI_Where, w, "Out", _locations.Find(x => x._LocId == _myloc._LI_Where)._Loc_Type, _locations);
            }
            w.WriteLine("</ul>");
        }
        public static void Write_Province_Destination_HTML(Int32 destination_location, StreamWriter w, String dest, String from_type, List<Location> _locations)
        {
            StringBuilder outline = new StringBuilder();
            Location _my_dest_loc = _locations.Find(x => x._LocId == destination_location);
            outline.Append("<li>");
            outline.Append(dest);
            if (_my_dest_loc._Loc_Type.Contains("city"))
            {
                outline.Append(", city");
            }
            outline.Append(", to ");
            outline.Append(_my_dest_loc._Name + " ");
            outline.Append(Utilities.Format_Anchor(_my_dest_loc._LocId_Conv));
            // determine days to move
            if (_my_dest_loc._LO_Barrier > 0)
            {
                outline.Append(", impassable<br>&nbsp;&nbsp;&nbsp;A magical barrier prevents entry.");
            }
            else
            {
                if (from_type == "ocean")
                {
                    if (_my_dest_loc._Loc_Type == "ocean")
                    {
                        outline.Append(", 3 days");
                    }
                    else
                    {
                        if ((_my_dest_loc._Loc_Type.Contains("city")))
                        {
                            outline.Append(", ");
                            outline.Append(_locations.Find(x => x._LocId == _my_dest_loc._LI_Where)._Name);
                            outline.Append(", 1 day");
                        }
                        else
                        {
                            outline.Append(", ");
                            outline.Append(_locations.Find(x => x._LocId == _my_dest_loc._LI_Where)._Name);
                            if (_my_dest_loc._Loc_Type == "mountain")
                            {
                                outline.Append(", impassable");
                            }
                            else
                            {
                                outline.Append(", 2 days");
                            }
                        }
                    }
                }
                else
                {
                    if (_my_dest_loc._Loc_Type == "plain")
                    {
                        outline.Append(", 7 days");
                    }
                    else
                    {
                        if (_my_dest_loc._Loc_Type == "forest")
                        {
                            outline.Append(", 8 days");
                        }
                        else
                        {
                            if (_my_dest_loc._Loc_Type == "mountain")
                            {
                                outline.Append(", 10 days");
                            }
                            else
                            {
                                if (_my_dest_loc._Loc_Type == "swamp")
                                {
                                    outline.Append(", 14 days");
                                }
                                else
                                {
                                    if (_my_dest_loc._Loc_Type == "desert")
                                    {
                                        outline.Append(", 8 days");
                                    }
                                    else
                                    {
                                        if (_my_dest_loc._Loc_Type == "ocean")
                                        {
                                            outline.Append(", ");
                                            outline.Append(_locations.Find(x => x._LocId == _my_dest_loc._LI_Where)._Name);
                                            if (from_type == "mountain")
                                            {
                                                outline.Append(", impassable");
                                            }
                                            else
                                            {
                                                // see if contains city, if so, impassible
                                                Boolean contains_city = false;
                                                foreach (int destloc in _my_dest_loc._LO_Province_Destination)
                                                {
                                                    if ((_locations.Find(x => x._LocId == destloc)._Loc_Type.Contains("city")))
                                                    {
                                                        contains_city = true;
                                                    }
                                                }
                                                if (!contains_city)
                                                {
                                                    outline.Append(", 2 days");
                                                }
                                                else
                                                {
                                                    outline.Append(", impassable");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dest == "Down" || dest == "Up")
                                            {
                                                outline.Append(", ");
                                                outline.Append(_locations.Find(x => x._LocId == _my_dest_loc._LI_Where)._Name);
                                            }
                                            if (dest == "Down" || dest == "Up" || dest == "Out" || from_type == "tunnel" || _my_dest_loc._Loc_Type == "tunnel")
                                            {
                                                outline.Append(", 0 days");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            outline.Append("</li>");
            w.WriteLine(outline);
            // check to see if was moving to city from ocean
            // if so, need to write impassable line for province
            if (from_type.Contains("ocean") && _my_dest_loc._Loc_Type.Contains("city"))
            {
                StringBuilder outlinein = new StringBuilder();
                Location _my_dest_locin = _locations.Find(x => x._LocId == _my_dest_loc._LI_Where);
                outlinein.Append("<li>");
                outlinein.Append(dest);
                outlinein.Append(", to ");
                outlinein.Append(_my_dest_locin._Name + " ");
                outlinein.Append(Utilities.Format_Anchor(_my_dest_locin._LocId_Conv));
                outlinein.Append(", ");
                outlinein.Append(_locations.Find(x => x._LocId == _my_dest_locin._LI_Where)._Name);
                outlinein.Append(", impassable");
                outlinein.Append("</li>");
                w.WriteLine(outlinein);
            }
        }
        public static void Write_Here_List_HTML(Location _myloc, StreamWriter w, List<Character> _characters, List<Itemz> _items,  List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            Boolean print_inner = false;
            Boolean seen_here = false;
            Boolean ships_docked = false;
            // Determine if Inner Locations/Seen Here
            foreach (int _my_inner in _myloc._LI_Here_List)
            {
                Location _my_dest_loc = _locations.Find(x => x._LocId == _my_inner);
                if (_my_dest_loc != null)
                {
                    print_inner = true;
                }
                else
                {
                    Character _my_dest_char = _characters.Find(x => x._CharId == _my_inner);
                    if (_my_dest_char != null)
                    {
                        seen_here = true;
                    }
                    else
                    {
                        Ship _my_dest_ship = _ships.Find(x => x._ShipId == _my_inner);
                        if (_my_dest_ship != null)
                        {
                            ships_docked = true;
                        }
                        else
                        {
                            Storm _my_dest_storm = _storms.Find(x => x._StormId == _my_inner);
                            if (_my_dest_storm != null)
                            {
                                // storm
                            }
                            else
                            {
                                Console.WriteLine("What is this? {0}", _my_inner);
                            }
                        }
                    }
                }
            }
            if (print_inner)
            {
                w.WriteLine("<H4>Inner Locations:</H4>");
                Write_Inner_Locs(_myloc, w, _characters, _items, _locations, _ships, _storms);
            }
            if (seen_here)
            {
                w.WriteLine("<H4>Seen Here:</H4>");
                Write_Seen_Here(_myloc, w, _characters, _items, _locations, _ships);
            }
            if (ships_docked)
            {
                w.WriteLine("<H4>" + (_myloc._Loc_Type != "ocean" ? "Ships docked at port:" : "Ships sighted:") + "</H4>");
                Write_Ships_Docked(_myloc, w,_characters, _items, _locations, _ships, _storms);
            }
        }

        private static void Write_Ships_Docked(Location _myloc, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            w.WriteLine("<ul>");
            foreach (int _my_inner in _myloc._LI_Here_List)
            {
                Ship _my_ship = _ships.Find(x => x._ShipId == _my_inner);
                if (_my_ship != null)
                {
                    Write_Ships_HTML(_my_ship, w, _characters, _items, _locations, _ships, _storms);
                }
            }
            w.WriteLine("</ul>");
        }

        private static void Write_Seen_Here(Location _myloc, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships)
        {
            w.WriteLine("<ul>");
            foreach (int _my_inner in _myloc._LI_Here_List)
            {
                Character _my_char = _characters.Find(x => x._CharId == _my_inner);
                if (_my_char != null)
                {
                    Write_Characters_HTML(_my_char, w, _characters, _items, _locations, _ships);
                }
            }
            w.WriteLine("</ul>");
        }
        private static void Write_Inner_Locs(Location _myloc, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            w.WriteLine("<ul>");
            foreach (int _my_inner in _myloc._LI_Here_List)
            {
                Location _my_dest_loc = _locations.Find(x => x._LocId == _my_inner);
                if (_my_dest_loc != null)
                {
                    Write_Sub_Locs_HTML(_myloc, w, _my_dest_loc, _characters, _items, _locations, _ships, _storms);
                }
            }
            w.WriteLine("</ul>");
        }

        public static void Write_Hidden_Accesses_HTML(Location _myloc, StreamWriter w, List<Player> _players)
        {
            bool firstline = true;
            foreach (Player _myplayer in _players)
            {
                if (_myplayer._Known != null)
                {
                    foreach (int _loc in _myplayer._Known)
                    {
                        if (_loc == _myloc._LocId)
                        {
                            if (firstline)
                            {
                                w.WriteLine("<H4>hidden location known by:</H4>");
                                w.WriteLine("<ul>");
                                firstline = false;
                            }
                            w.WriteLine("<li>{0} [{1}]</li>", _myplayer._Name, _myplayer._FactionId_Conv);
                        }
                    }
                }
            }
            if (!firstline)
            {
                w.WriteLine("</ul>");
            }
        }
        private static void Write_Sub_Locs_HTML(Location _myloc, StreamWriter w, Location _my_dest_loc, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<li>");
            outline.Append(_my_dest_loc._Name);
            outline.Append(" ");
            outline.Append(Utilities.Format_Anchor(_my_dest_loc._LocId_Conv));
            outline.Append(", " + _my_dest_loc._Loc_Type);
            outline.Append(_my_dest_loc._LO_Hidden.Equals(1) ? ", hidden":"");
            outline.Append((!_myloc._Loc_Type.Contains("city") && _my_dest_loc._SL_Defense == 0) ? ", 1 day" : "");
            outline.Append(_my_dest_loc._SL_Defense != 0 ?", defense " + _my_dest_loc._SL_Defense:"");
            if (_my_dest_loc._Loc_Type.Contains("castle"))
            {
                // level
                if (_my_dest_loc._SL_Castle_Level != 0)
                {
                    outline.Append(", level " + _my_dest_loc._SL_Castle_Level);
                }
            }
            if (_my_dest_loc._Loc_Type.Contains("mine"))
            {
                // level
                if (_my_dest_loc._SL_Shaft_Depth != 0)
                {
                    outline.Append(", level " + _my_dest_loc._SL_Shaft_Depth / 3);
                }
            }
            // % completed
            if (_my_dest_loc._SL_Effort_Given != 0 && _my_dest_loc._SL_Effort_Required != 0)
            {
                float pctComp = ((float)_my_dest_loc._SL_Effort_Given / (float)_my_dest_loc._SL_Effort_Required) * (float)100;
                outline.Append(", " + Convert.ToInt32(pctComp) + "% completed");
            }
            // damage
            if (_my_dest_loc._SL_Damage != 0)
            {
                outline.Append(", " + _my_dest_loc._SL_Damage + "% damaged");
                // what about collapsed??
            }
            // owner
            if (_my_dest_loc._LI_Here_List != null)
            {
                if (_my_dest_loc._LI_Here_List.Count > 0)
                {
                    Character text_character = _characters.Find(x => x._CharId == _my_dest_loc._LI_Here_List[0]);
                    if (text_character != null)
                    {
                        if (text_character._Char_Type != "garrison")
                        {
                            outline.Append(", owner: ");
                        }
                    }
                }
            }
            outline.Append("</li>");
            w.WriteLine(outline);
            // check for inner locations and/or characters
            if (_my_dest_loc._LI_Here_List != null)
            {
                w.WriteLine("<ul>");
                foreach (int _my_inner in _my_dest_loc._LI_Here_List)
                {
                    Location _my_dest_loc2 = _locations.Find(x => x._LocId == _my_inner);
                    if (_my_dest_loc2 != null)
                    {
                        Write_Sub_Locs_HTML(_myloc, w, _my_dest_loc2, _characters, _items, _locations, _ships, _storms);
                    }
                    else
                    {
                        Character _my_char_loc2 = _characters.Find(x => x._CharId == _my_inner);
                        if (_my_char_loc2 != null)
                        {
                            Write_Characters_HTML(_my_char_loc2, w, _characters, _items, _locations, _ships);
                        }
                        else
                        {
                            Ship _my_ship_loc2 = _ships.Find(x => x._ShipId == _my_inner);
                            if (_my_ship_loc2 != null)
                            {
                                Write_Ships_HTML(_my_ship_loc2, w, _characters, _items, _locations, _ships, _storms);
                            }
                            else
                            {
                                Storm _my_storm_loc2 = _storms.Find(x => x._StormId == _my_inner);
                                if (_my_storm_loc2 != null)
                                {
                                    //Write_Storms_HTML(_my_ship_loc2, w);
                                }
                                else
                                {
                                    Console.WriteLine("What is it?? {0}", _my_inner);
                                }
                            }
                        }
                    }
                }
                w.WriteLine("</ul>");
            }
        }
        public static void Write_Characters_HTML(Character _myChar, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<li>");
            outline.Append(_myChar._Name + " ");
            outline.Append(Utilities.Format_Anchor(_myChar._CharId.ToString()));
            if ((Utilities.Xlate_Loyalty(_myChar._CH_LOY_Kind, _myChar._CH_LOY_Rate) != "Undefined") &&
                (Utilities.Xlate_Loyalty(_myChar._CH_LOY_Kind, _myChar._CH_LOY_Rate) != "Npc"))
            {
                outline.Append(" (" + Utilities.Xlate_Loyalty(_myChar._CH_LOY_Kind, _myChar._CH_LOY_Rate));
                if (_myChar._CH_Skills_List != null)
                {
                    int iterations = _myChar._CH_Skills_List.Count() / 5;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (_myChar._CH_Skills_List[(i * 5) + 0] == 909)
                        {
                            outline.Append(":AB");
                        }
                    }
                }
                outline.Append(")");
            }
            else
            {
                if (_myChar._CH_Skills_List != null)
                {
                    int iterations = _myChar._CH_Skills_List.Count() / 5;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (_myChar._CH_Skills_List[(i * 5) + 0] == 909)
                        {
                            outline.Append("(AB)");
                        }
                    }
                }
            }
            if (!_myChar._Char_Type.Equals("0"))
            {
                outline.Append(", " + _myChar._Char_Type);
            }
            if (_myChar._CH_Prisoner == 1)
            {
                outline.Append(", prisoner");
            }
            if (Character.Is_Priest(_myChar))
            {
                outline.Append(", priest");
            }
            if (_myChar._CM_Magician == 1)
            {
                outline.Append(Character.Mage_Type(_myChar) != "" ? (", " + Character.Mage_Type(_myChar)) : "");
            }
            if (_myChar._CH_Guard == 1)
            {
                outline.Append(", on guard");
            }
            //concealed
            if (_myChar._CM_Hide_Self.Equals(1))
            {
                if (Character.ReallyHidden(_myChar, _characters, _items, _locations, _ships))
                {
                    outline.Append(", concealed (maybe)");
                }
            }
            // get possessions
            if (_myChar._Item_List != null)
            {
                int num_items = _myChar._Item_List.Count / 2;
                for (int i = 0; i < num_items; i++)
                {
                    if (_items.Find(x => x._ItemId == _myChar._Item_List[Convert.ToInt32((i * 2) + 0)])._IT_Prominent == 1)
                    {
                        string temp_str = _myChar._Item_List[(i * 2) + 1] + " " +
                            (Convert.ToInt32(_myChar._Item_List[(i * 2) + 1]) == 1 ?
                            _items.Find(x => x._ItemId == _myChar._Item_List[Convert.ToInt32((i * 2) + 0)])._Name :
                            _items.Find(x => x._ItemId == _myChar._Item_List[Convert.ToInt32((i * 2) + 0)])._Plural);
                        if (outline.Length + temp_str.Length > 100 )
                        {
                            outline.Append(",<br>");
                            w.WriteLine(outline);
                            outline.Clear();
                        }
                        else
                        {
                            outline.Append(", ");
                        }
                        outline.Append(temp_str);
                    }
                }
            }
            if (_myChar._LI_Here_List != null)
            {
                outline.Append(", accompanied by: ");
            }
            outline.Append("</li>");
            w.WriteLine(outline);
            if (_myChar._LI_Here_List != null)
            {
                w.WriteLine("<ul>");
                foreach (int _my_stacked_charid in _myChar._LI_Here_List)
                {
                    Character _my_stacked_char = _characters.Find(x => x._CharId ==_my_stacked_charid);
                    Write_Characters_HTML(_my_stacked_char, w, _characters, _items, _locations, _ships);
                }
                w.WriteLine("</ul>");
            }
        }
        public static void Write_Ships_HTML(Ship _myship, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<li>");
            outline.Append(_myship._Name);
            outline.Append(" " + Utilities.Format_Anchor(_myship._ShipId.ToString()));
            outline.Append(", " + _myship._Ship_Type);
            // bound storm
            if (_myship._SL_Bound_Storm != 0)
            {
                outline.Append(", Storm " + _myship._SL_Bound_Storm + " (strength: " + _storms.Find(x=>x._StormId == _myship._SL_Bound_Storm)._Storm_Strength + ")");
            }
            // loaded
            // defense
            if (_myship._SL_Defense != 0)
            {
                outline.Append(", defense " + _myship._SL_Defense);
            }
            // % completed
            if (_myship._SL_Effort_Given != 0 && _myship._SL_Effort_Required != 0)
            {
                float pctComp = ((float)_myship._SL_Effort_Given / (float)_myship._SL_Effort_Required) * (float)100;
                outline.Append(", " + (Convert.ToInt32(pctComp)) + "% completed");
            }
            // damage
            if (_myship._SL_Damage != 0)
            {
                outline.Append(", " + _myship._SL_Damage + "% damaged");
            }

            // owner
            if (_myship._LI_Here_List != null)
            {
                outline.Append(", owner:");
            }
            outline.Append("</li>");
            w.WriteLine(outline);
            if (_myship._LI_Here_List != null)
            {
                w.WriteLine("<ul>");
                foreach (int _charid in _myship._LI_Here_List)
                {
                    Character _my_char = _characters.Find(x => x._CharId == Convert.ToInt32(_charid));
                    if (_my_char != null)
                    {
                        Write_Characters_HTML(_my_char, w, _characters, _items, _locations, _ships);
                    }
                    else
                    {
                        Console.WriteLine("Non-Character {0} in Ship {1}", _charid, _myship._ShipId);
                    }
                }
                w.WriteLine("</ul>");
            }
        }
        private static void Write_Structure_Basic_Info(Location _myloc, StreamWriter w)
        {
            if (_myloc._SL_Defense > 0 || _myloc._SL_Damage > 0 || (_myloc._SL_Effort_Given < _myloc._SL_Effort_Required))
            {
                StringBuilder outline = new StringBuilder();
                w.WriteLine("<table>");
                Write_Structure_Pct_Complete(_myloc, w);
                Write_Structure_Defense(_myloc, w);
                Write_Structure_Damaged(_myloc, w);
                Write_Structure_Level(_myloc, w);
                w.WriteLine("</table>");
            }
        }
        private static void Write_Structure_Damaged(Location _myloc, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Damaged:</td>");
            w.WriteLine("<td>" + _myloc._SL_Damage + "%</td>");
            w.WriteLine("</tr>");
        }

        private static void Write_Structure_Defense(Location _myloc, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Defense:</td>");
            w.WriteLine("<td>" + _myloc._SL_Defense + "</td>");
            w.WriteLine("</tr>");
        }
        private static void Write_Structure_Pct_Complete(Location _myloc, StreamWriter w)
        {
            if (_myloc._SL_Effort_Given < _myloc._SL_Effort_Required)
            {
                w.WriteLine("<tr>");
                w.WriteLine("<td>Percent Complete:</td>");
                w.WriteLine("<td>" + ((float)_myloc._SL_Effort_Given / (float)_myloc._SL_Effort_Required) * 100f + "%</td>");
                w.WriteLine("</tr>");
            }
            w.WriteLine("<tr>");
        }
        private static void Write_Structure_Level(Location _myloc, StreamWriter w)
        {
            if (_myloc._SL_Shaft_Depth > 0)
            {
                w.WriteLine("<tr>");
                w.WriteLine("<td>Level:</td>");
                w.WriteLine("<td>" + (_myloc._SL_Shaft_Depth / 3) + "</td>");
                w.WriteLine("</tr>");
            }
            w.WriteLine("<tr>");
        }
    }
}
