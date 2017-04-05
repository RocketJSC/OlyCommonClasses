using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class HTML_Item
    {
        public static void Generate_Item_List_HTML(string path, List<Character> _characters,  List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, "master_item_list.html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>Olympia Master Item List</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    w.WriteLine("<h3>Olympia Master Item List</h3>");
                    w.WriteLine("<table border=\"1\" style=\"border-collapse: collapse\">");
                    w.WriteLine("<tr>");
                    w.WriteLine("<th>Item</th><th>Item Type</th><th>Weight</th><th>Man Item</th><th>Prominent</th><th>Animal</th><th>Land Cap</th><th>Riding cap</th><th>Flying Cap</th><th>Who Has</th><th>Notes</th>");
                    w.WriteLine("</tr>");
                    foreach (Itemz _item in _items)
                    {
                        w.WriteLine("<tr>");
                        w.WriteLine("<td>" + _item._Name + " [" + _item._ItemId_Conv + "]</td>");
                        w.WriteLine("<td>" + (_item._Item_Type != "0" ? _item._Item_Type : "std item") + "</td>");
                        w.WriteLine("<td>" + _item._Weight + "</td>");
                        w.WriteLine("<td>" + (_item._IT_Man_Item == 1 ? "yes" : "") + "</td>");
                        w.WriteLine("<td>" + (_item._IT_Prominent == 1 ? "yes" : "") + "</td>");
                        w.WriteLine("<td>" + (_item._IT_Animal == 1 ? "yes" : "") + "</td>");
                        w.WriteLine("<td>" + _item._Land_Capacity + "</td>");
                        w.WriteLine("<td>" + _item._Ride_Capacity + "</td>");
                        w.WriteLine("<td>" + _item._Fly_Capacity + "</td>");
                        if (_item._IT_Who_Has != 0)
                        {
                            if (_characters.Find(x => x._CharId == _item._IT_Who_Has) != null)
                            {
                                Character _mychar = _characters.Find(x => x._CharId == _item._IT_Who_Has);
                                w.WriteLine("<td>" + _mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()) + "</td>");
                            }
                            else
                            {
                                w.WriteLine("<td>&nbsp;</td>");
                            }
                        }
                        else
                        {
                            w.WriteLine("<td>&nbsp;</td>");
                        }
                        w.WriteLine("<td>" + Itemz.Determine_Use(_item, _characters, _locations, _ships, _skills) + "&nbsp;</td>");
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
        public static void Generate_Orb_List_HTML(string path, List<Character> _characters, List<Itemz> _items)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, "master_orb_list.html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>Olympia Master Orb List</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    w.WriteLine("<h3>Olympia Master Orb List</h3>");
                    w.WriteLine("<table border=\"1\" style=\"border-collapse: collapse\">");
                    w.WriteLine("<tr>");
                    w.WriteLine("<th>Item</th><th>Who Has</th>");
                    w.WriteLine("</tr>");
                    foreach (Itemz _item in _items.Where(x=>x._IM_Use_Key.Equals(9)))
                    {
                        w.WriteLine("<tr>");
                        w.WriteLine("<td>" + _item._Name + " [" + _item._ItemId_Conv + "]</td>");
                        if (_item._IT_Who_Has != 0)
                        {
                            if (_characters.Find(x => x._CharId == _item._IT_Who_Has) != null)
                            {
                                Character _mychar = _characters.Find(x => x._CharId == _item._IT_Who_Has);
                                w.WriteLine("<td>" + _mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()) + "</td>");
                            }
                            else
                            {
                                w.WriteLine("<td>&nbsp;</td>");
                            }
                        }
                        else
                        {
                            w.WriteLine("<td>&nbsp;</td>");
                        }
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
        public static void Generate_Healing_Potion_List_HTML(string path, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, "master_healing_potion_list.html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>Olympia Master Healing Potion List</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    w.WriteLine("<h3>Olympia Master Healing Potion List</h3>");
                    w.WriteLine("<table border=\"1\" style=\"border-collapse: collapse\">");
                    w.WriteLine("<tr>");
                    w.WriteLine("<th>Item</th><th>Who Has</th><th>Location</th><th>Notes</th>");
                    w.WriteLine("</tr>");
                    foreach (Itemz _item in _items.Where(t=>t._Item_Type == "0").Where(u=>u._IM_Use_Key.Equals(2)))
                    {
                        w.WriteLine("<tr>");
                        w.WriteLine("<td>" + _item._Name + " [" + _item._ItemId_Conv + "]</td>");
                        if (_item._IT_Who_Has != 0)
                        {
                            if (_characters.Find(x => x._CharId == _item._IT_Who_Has) != null)
                            {
                                Character _mychar = _characters.Find(x => x._CharId == _item._IT_Who_Has);
                                w.WriteLine("<td>" + _mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()) + "</td>");
                                w.WriteLine("<td>" +
                                _mychar.Calc_CurrentLoc +
                                " in " +
                                _locations.Find(g => g._LocId == _mychar.Calc_CurrentRegion)._Name +
                                "&nbsp;</td>");
                            }
                            else
                            {
                                if (_locations.Find(x => x._LocId == _item._IT_Who_Has) != null)
                                {
                                    Location _myloc = _locations.Find(x => x._LocId == _item._IT_Who_Has);
                                    w.WriteLine("<td>" + _myloc._Name + " " + Utilities.Format_Anchor(_myloc._LocId.ToString()) + "</td>");
                                    w.WriteLine("<td>" +
                                    _myloc.Calc_CurrentLoc +
                                    " in " +
                                    _locations.Find(g => g._LocId == _myloc.Calc_CurrentRegion)._Name +
                                    "&nbsp;</td>");
                                }
                                else
                                {
                                    w.WriteLine("<td>&nbsp;</td>");
                                    w.WriteLine("<td>&nbsp;</td>");
                                }
                            }
                        }
                        else
                        {
                            w.WriteLine("<td>&nbsp;&nbsp;</td>");
                        }
                        w.WriteLine("<td>" + Itemz.Determine_Use(_item, _characters, _locations, _ships, _skills) + "&nbsp;</td>");
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
        public static void Generate_Projected_Cast_Potion_List_HTML(string path, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, "master_projected_cast_potion_list.html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>Olympia Master Projected Cast Potion List</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    w.WriteLine("<h3>Olympia Master Projected Cast Potion List</h3>");
                    w.WriteLine("<table border=\"1\" style=\"border-collapse: collapse\">");
                    w.WriteLine("<tr>");
                    w.WriteLine("<th>Item</th><th>Who Has</th><th>Location</th><th>Notes</th>");
                    w.WriteLine("</tr>");
                    foreach (Itemz _item in _items.Where(t => t._Item_Type == "0").Where(u => u._IM_Use_Key.Equals(5)))
                    {
                        w.WriteLine("<tr>");
                        w.WriteLine("<td>" + _item._Name + " [" + _item._ItemId_Conv + "]</td>");
                        if (_item._IT_Who_Has != 0)
                        {
                            if (_characters.Find(x => x._CharId == _item._IT_Who_Has) != null)
                            {
                                Character _mychar = _characters.Find(x => x._CharId == _item._IT_Who_Has);
                                w.WriteLine("<td>" + _mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()) + "</td>");
                                w.WriteLine("<td>" +
                                _mychar.Calc_CurrentLoc +
                                " in " +
                                _locations.Find(g=>g._LocId == _mychar.Calc_CurrentRegion)._Name +
                                "&nbsp;</td>");
                            }
                            else
                            {
                                if (_locations.Find(x => x._LocId == _item._IT_Who_Has) != null)
                                {
                                    Location _myloc = _locations.Find(x => x._LocId == _item._IT_Who_Has);
                                    w.WriteLine("<td>" + _myloc._Name + " " + Utilities.Format_Anchor(_myloc._LocId.ToString()) + "</td>");
                                    w.WriteLine("<td>" +
                                    _myloc.Calc_CurrentLoc +
                                    " in " +
                                    _locations.Find(g => g._LocId == _myloc.Calc_CurrentRegion)._Name +
                                    "&nbsp;</td>");
                                }
                                else
                                {
                                    w.WriteLine("<td>&nbsp;</td>");
                                    w.WriteLine("<td>&nbsp;</td>");
                                }
                            }
                        }
                        else
                        {
                            w.WriteLine("<td>&nbsp;</td>");
                            w.WriteLine("<td>&nbsp;</td>");
                        }
                        w.WriteLine("<td>" + Itemz.Determine_Use(_item, _characters, _locations, _ships, _skills) + "&nbsp;</td>");
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table>");
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
    }
}
