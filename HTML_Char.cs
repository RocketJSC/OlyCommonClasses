using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class HTML_Char
    {
        public static void Write_Char_HTML_File(Character _mychar, string path, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, _mychar._CharId + ".html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>" + _mychar._Name +
                                " [" + _mychar._CharId + "]" +
                                "</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    Write_Char_Page_Header(_mychar, w);
                    if (_mychar._CH_Prisoner == 1)
                    {
                        w.WriteLine("<p>" + _mychar._Name + " [" + _mychar._CharId + "] is being held prisoner.</p>");
                    }
                    Write_Char_Basic_Info(_mychar, w, _characters, _items, _locations, _ships, _skills);

                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
        private static void Write_Char_Page_Header(Character _myChar, StreamWriter w)
        {
            StringBuilder outline3 = new StringBuilder();
            outline3.Append("<H3>");
            outline3.Append(_myChar._Name);
            outline3.Append(" [");
            outline3.Append(_myChar._CharId);
            outline3.Append("]");
            outline3.Append("</H3>");
            w.WriteLine(outline3);
        }
        private static void Write_Char_Basic_Info(Character _myChar, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Skill> _skills)
        {
            w.WriteLine("<table>");
            Write_Char_Location(_myChar, w, _characters, _locations, _ships);
            if (_myChar._Char_Type.ToUpper() != "GARRISON")
            {
                Write_Char_Rank(_myChar, w);
                Write_Char_Loyalty(_myChar, w);
                Write_Char_Stacked_Under(_myChar, w, _characters);
                Write_Char_Stacked_Over(_myChar, w, _characters);
                Write_Char_Health(_myChar, w);
                Write_Char_Combat(_myChar, w);
                Write_Char_Break_Point(_myChar, w);
                Write_Char_Pledged_To(_myChar, w, _characters);
                Write_Char_Pledged_To_Us(_myChar, w, _characters);
                Write_Char_Concealed(_myChar, w, _characters, _items, _locations, _ships);
                Write_Char_Aura(_myChar, w, _items);
                // missing show appear common
                Write_Char_Prisoners(_myChar, w, _characters);
            }
            w.WriteLine("</table>");
            if (_myChar._Char_Type.ToUpper() != "GARRISON")
            {
                Write_Char_Skills_Known(_myChar, w, _skills);
            }
            Write_Char_Inventory(_myChar, w, _items);
            if (_myChar._Char_Type.ToUpper() != "GARRISON")
            {
                Write_Char_Capacity(_myChar, w, _items);
                Write_Char_Pending_Trades(_myChar, w, _items);
                Write_Visions_Received(_myChar, w, _characters);
            }
        }

        private static void Write_Char_Pending_Trades(Character _myChar, StreamWriter w, List<Itemz> _items)
        {
            if (_myChar._Trade_List != null)
            {
                int iterations = _myChar._Trade_List.Count() / 8;
                w.WriteLine("<p>Pending Trades:</p>");
                w.WriteLine("<table>");
                w.WriteLine("<tr><td style=\"text-align:right\">trade</td><td style=\"text-align:right\">price</td><td style=\"text-align:right\">qty</td><td style=\"text-align:left\">item</td></tr>");
                w.WriteLine("<tr><td style=\"text-align:right\">---</td><td style=\"text-align:right\">----</td><td style=\"text-align:left\">-----</td></tr>");
                for (int i = 0; i < iterations; i++)
                {
                    int _type = _myChar._Trade_List[(i * 8)];
                    int _item = _myChar._Trade_List[(i * 8) + 1];
                    int _qty = Convert.ToInt32(_myChar._Trade_List[(i * 8) + 2]);
                    int _cost = Convert.ToInt32(_myChar._Trade_List[(i * 8) + 3]);
                    w.WriteLine("<tr>");
                    Itemz _myitem = _items.Find(x => x._ItemId == Convert.ToInt32(_item));
                    if (_myitem != null)
                    {
                        w.WriteLine("<td style=\"text-align:right\">" + (_type == 1 ? "buy" : "sell") + "</td>");
                        w.WriteLine("<td style=\"text-align:right\">" + _cost + "</td>");
                        w.WriteLine("<td style=\"text-align:right\">" + _qty + "</td>");
                        w.WriteLine("<td style=\"text-align:left\">" + (_qty == 1 ? _myitem._Name : _myitem._Plural) + " [" + Utilities.To_Oid(_item.ToString()) + "]</td>");
                    }
                    w.WriteLine("</tr>");
                }
                w.WriteLine("</table>");
            }
        }

        private static void Write_Char_Capacity(Character _myChar, StreamWriter w, List<Itemz> _items)
        {
            Weight myweight = Character.Determine_Unit_Weights(_myChar, _items);
            StringBuilder outline = new StringBuilder();
            outline.Append("<p>Capacity: ");
            string land_pct = "";
            if (myweight._land_cap > 0)
            {
                land_pct = " land (" + ((myweight._land_weight * 100) / myweight._land_cap).ToString("N0") + "%)";
            }
            outline.AppendFormat("{0}/{1}{2}", myweight._land_weight.ToString("N0"), myweight._land_cap.ToString("N0"), land_pct);
            string ride_pct = "";
            if (myweight._ride_cap > 0)
            {
                ride_pct = " ride (" + ((myweight._ride_weight * 100) / myweight._ride_cap).ToString("N0") + "%)";
                outline.AppendFormat(" {0}/{1}{2}", myweight._ride_weight.ToString("N0"), myweight._ride_cap.ToString("N0"), ride_pct);
            }
            string fly_pct = "";
            if (myweight._fly_cap > 0)
            {
                fly_pct = " fly: (" + ((myweight._fly_weight * 100) / myweight._fly_cap).ToString("N0") + "%)";
                outline.AppendFormat(" {0}/{1}{2}", myweight._fly_weight.ToString("N0"), myweight._fly_cap.ToString("N0"), fly_pct);
            }
            outline.Append("</p>");
            w.WriteLine(outline);
        }
        private static void Write_Char_Inventory(Character _myChar, StreamWriter w, List<Itemz> _items)
        {
            int total_weight = 0;
            if (_myChar._Item_List != null)
            {
                int iterations = _myChar._Item_List.Count() / 2;
                w.WriteLine("Inventory:");
                w.WriteLine("<table>");
                w.WriteLine("<tr><td style=\"text-align:right\">qty</td><td style=\"text-align:left\">name</td><td style=\"text-align:right\">weight</td><td style=\"text-align:left\">&nbsp;</td></tr>");
                w.WriteLine("<tr><td style=\"text-align:right\">---</td><td style=\"text-align:left\">----</td><td style=\"text-align:right\">-----</td></tr>");
                for (int i = 0; i < iterations; i++)
                {
                    int _item = _myChar._Item_List[(i * 2)];
                    int _qty = _myChar._Item_List[(i * 2) + 1];
                    w.WriteLine("<tr>");
                    w.WriteLine("<td style=\"text-align:right\">" + _qty.ToString("N0") + "</td>");
                    Itemz _myitem = _items.Find(x => x._ItemId == _item);
                    if (_myitem != null)
                    {
                        w.WriteLine("<td style=\"text-align:left\">" + (_qty == 1 ? _myitem._Name : _myitem._Plural) + " [" + Utilities.To_Oid(_item.ToString()) + "]</td>");
                        w.WriteLine("<td style=\"text-align:right\">" + (_myitem._Weight * _qty).ToString("N0") + "</td>");
                        total_weight += (_myitem._Weight * _qty);
                        StringBuilder outline = new StringBuilder();
                        outline.Append("<td>");
                        if (_myChar._Char_Type.ToUpper() != "GARRISON")
                        {
                            if (_myitem._Fly_Capacity > 0)
                            {
                                outline.Append("fly " + (_myitem._Fly_Capacity * _qty).ToString("N0"));
                            }
                            else
                            {
                                if (_myitem._Ride_Capacity > 0)
                                {
                                    outline.Append("ride " + (_myitem._Ride_Capacity * _qty).ToString("N0"));
                                }
                                else
                                {
                                    if (_myitem._Land_Capacity > 0)
                                    {
                                        outline.Append("cap " + (_myitem._Land_Capacity * _qty).ToString("N0"));
                                    }
                                }
                            }
                            if (Itemz.Is_Fighter(_myitem))
                            {
                                outline.AppendFormat(" ({0},{1},{2})", _myitem._IT_Attack, _myitem._IT_Defense, _myitem._IT_Missile);
                            }
                        }
                        outline.Append(_myitem._IM_Attack_Bonus > 0 ? ("+" + _myitem._IM_Attack_Bonus + " attack"):"");
                        outline.Append(_myitem._IM_Defense_Bonus > 0 ? ("+" + _myitem._IM_Defense_Bonus + " defense") : "");
                        outline.Append(_myitem._IM_Missile_Bonus > 0 ? ("+" + _myitem._IM_Missile_Bonus + " missile") : "");
                        if (Character.Is_Magician(_myChar) && _myitem._IM_Aura_Bonus > 0)
                        {
                            outline.AppendFormat("+{0} aura)", _myitem._IM_Aura_Bonus);
                        }
                        outline.Append("&nbsp;</td>");
                        w.WriteLine(outline);
                    }
                    else
                    {
                        w.WriteLine("<td>" + "undefined" + "</td>");
                        w.WriteLine("<td>" + 0 + "</td>");
                        w.WriteLine("<td>&nbsp;</td>");
                    }
                    w.WriteLine("</tr>");
                }
                if (_myChar._Char_Type.ToUpper() != "GARRISON")
                {
                    w.WriteLine("<tr><td></td><td></td><td style=\"text-align:right\">=====</td><td>&nbsp;</td></tr>");
                    w.WriteLine("<tr><td></td><td></td><td style=\"text-align:right\">" + total_weight.ToString("N0") + "</td><td>&nbsp;</td></tr>");
                }
                w.WriteLine("</table>");
            }
            else
            {
                w.WriteLine("<p>" + _myChar._Name + " [" + _myChar._CharId + "] has no possessions.</p>");
            }
        }

        private static void Write_Char_Skills_Known(Character _myChar, StreamWriter w, List<Skill> _skills)
        {
            if (_myChar._CH_Skills_List != null)
            {
                int iterations = _myChar._CH_Skills_List.Count() / 5;
                bool printKnown = false;
                bool printUnknown = false;
                for (int i = 0; i < iterations; i++)
                {
                    int _skill = _myChar._CH_Skills_List[(i * 5) + 0];
                    int _know = _myChar._CH_Skills_List[(i * 5) + 1];
                    int _days_studied = _myChar._CH_Skills_List[(i * 5) + 2];
                    if (_know == 2)
                    {
                        if (!printKnown)
                        {
                            w.WriteLine("<p>Skills known:</p>");
                            w.WriteLine("<ul style=\"list-style-type:none\">");
                            printKnown = true;
                        }
                        if (_skills.Find(x => x._SkillId == _skill) != null)
                        {
                            Skill _myskill = _skills.Find(x => x._SkillId == _skill);
                            w.WriteLine("<li>" + (_myskill._SK_Required_Skill != 0 ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : "") + _myskill._Name + " [" + _myskill._SkillId + "]</li>");
                        }
                    }
                    if (_know == 1)
                    {
                        if (!printUnknown)
                        {
                            if (printKnown)
                            {
                                w.WriteLine("</ul>");
                            }
                            w.WriteLine("<p>Partially known skills:</p>");
                            w.WriteLine("<ul style=\"list-style-type:none\">");
                            printUnknown = true;
                        }
                        if (_skills.Find(x => x._SkillId == _skill) != null)
                        {
                            Skill _myskill = _skills.Find(x => x._SkillId == _skill);
                            w.WriteLine("<li>" + _myskill._Name + " [" + _myskill._SkillId + "], {0}/{1}</li>", _days_studied, _myskill._SK_Time_to_learn);
                        }
                    }
                }
                if (printKnown || printUnknown)
                {
                    w.WriteLine("</ul>");
                }
            }
        }

        private static void Write_Char_Prisoners(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            List<Character> prisoner_list = _characters.FindAll(x => x._LI_Where == _myChar._CharId && x._CH_Prisoner == 1);
            if (prisoner_list.Count > 0)
            {
                string label = "Prisoner:";
                if (prisoner_list.Count > 1)
                {
                    label = "Prisoners:";
                }
                foreach (Character _myprisoner in prisoner_list)
                {
                    w.WriteLine("<tr>");
                    w.WriteLine("<td>" + label + "</td>");
                    label = "";
                    w.WriteLine("<td>" + _myprisoner._Name + " " + Utilities.Format_Anchor(_myprisoner._CharId.ToString()) + ", health " + _myprisoner._CH_Health + "</td>");
                    w.WriteLine("</tr>");
                }
            }
        }

        private static void Write_Char_Aura(Character _myChar, StreamWriter w, List<Itemz> _items)
        {
            if (_myChar._CM_Magician == 1)
            {
                if (_myChar._CM_Cur_Aura != 0)
                {
                    w.WriteLine("<tr><td>Current Aura:</td><td>" + _myChar._CM_Cur_Aura + "</td></tr>");
                }
                if (_myChar._CM_Auraculum != null)
                {
                    int AuraculumAura = _items.Find(x=>x._ItemId == _myChar._CM_Auraculum[0])._IM_Aura;
                    w.WriteLine("<tr><td>Max Aura:</td><td>" + (_myChar._CM_Max_Aura + AuraculumAura) + " (" + _myChar._CM_Max_Aura + " + " + AuraculumAura + ")</td></tr>");
                }
                else
                {
                    w.WriteLine("<tr><td>Max Aura:</td><td>" + (_myChar._CM_Max_Aura)  + "</td></tr>");
                }
            }
        }

        private static void Write_Char_Concealed(Character _myChar, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships)
        {
            if (_myChar._CM_Hide_Self == 1)
            {
                w.WriteLine("<tr>");
                w.WriteLine("<td>Concealed:</td>");
                w.WriteLine("<td>{0}</td>", Character.ReallyHidden(_myChar, _characters, _items, _locations, _ships) == true ? "Yes" : "Yes, but not alone");
                w.WriteLine("</tr>");
            }
        }

        private static void Write_Char_Pledged_To_Us(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            List<Character> _pledgedlist = _characters.FindAll(x => x._CM_Pledged_To == _myChar._CharId);
            if (_pledgedlist.Count > 0)
            {
                string pledgelabel = "Pledged to us:";
                foreach (Character _pledgechar in _pledgedlist)
                {
                    w.WriteLine("<tr>");
                    w.WriteLine("<td>" + pledgelabel + "</td>");
                    pledgelabel = "&nbsp;";
                    w.WriteLine("<td>" + _pledgechar._Name + " " + Utilities.Format_Anchor(_pledgechar._CharId.ToString()) + " </td>");
                    w.WriteLine("</tr>");
                }
            }
        }

        private static void Write_Char_Pledged_To(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            if (_myChar._CM_Pledged_To != 0)
            {
                Character _mychar2 = _characters.Find(x => x._CharId == _myChar._CM_Pledged_To);
                if (_mychar2 != null)
                {
                    w.WriteLine("<tr>");
                    w.WriteLine("<td>Pledged to:</td>");
                    w.WriteLine("<td>" + _mychar2._Name + " " + Utilities.Format_Anchor(_mychar2._CharId.ToString()) + " </td>");
                    w.WriteLine("</tr>");
                }
            }
        }

        private static void Write_Char_Break_Point(Character _myChar, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Break Point:</td>");
            w.WriteLine("<td>" + _myChar._CH_Break_Point + (_myChar._CH_Break_Point.Equals(0) ? "% (fight to the death)" : "%") + " </td>");
            w.WriteLine("</tr>");
            // vision protection
            if (_myChar._CM_Vision_Protect != 0)
            {
                w.WriteLine("<tr>");
                w.WriteLine("<td>Receive Vision:</td>");
                w.WriteLine("<td>" + _myChar._CM_Vision_Protect + " protection" + " </td>");
                w.WriteLine("</tr>");
            }
        }

        private static void Write_Char_Combat(Character _myChar, StreamWriter w)
        {
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<tr>");
            w.WriteLine("<td>Combat:</td>");
            outline.Append("<td>");
            outline.Append("attack " + _myChar._CH_Attack);
            outline.Append(", defense " + _myChar._CH_Defense);
            outline.Append(", missile " + _myChar._CH_Missile);
            outline.Append("</td>");
            outline.Append("</tr>");
            w.WriteLine(outline);
            outline.Clear();
            w.WriteLine("<tr>");
            outline.Append("<td>&nbsp;</td>");
            outline.Append("<td>");
            outline.Append("behind " + _myChar._CH_Behind);
            outline.Append(_myChar._CH_Behind.Equals(0) ? " (front line in combat)" : " (stay behind in combat)");
            outline.Append("</td>");
            w.WriteLine("</tr>");
            w.WriteLine(outline);
        }
        private static void Write_Char_Health(Character _myChar, StreamWriter w)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<tr>");
            outline.Append("<td>Health:</td>");
            outline.Append("<td>");
            outline.Append(_myChar._CH_Health + "%");
            if (_myChar._CH_Health < 100)
            {
                outline.Append(_myChar._CH_Sick.Equals(1) ? " (getting worse)":" (getting better");
            }
            outline.Append("</td>");
            outline.Append("</tr>");
            w.WriteLine(outline);
        }

        private static void Write_Char_Stacked_Over(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            if (_myChar._LI_Here_List != null)
            {
                StringBuilder outline2 = new StringBuilder();
                StringBuilder outline3 = new StringBuilder();
                outline2.Append("Stacked Over:");
                foreach (int _stacked_under in _myChar._LI_Here_List)
                {
                    if (_characters.Find(x => x._CharId == _stacked_under) != null)
                    {
                        Character _mychar_over = _characters.Find(x => x._CharId == _stacked_under);
                        if (_mychar_over._Char_Type == "0")
                        {
                            outline2.Append("<br>");
                            outline3.Append(_mychar_over._Name + " " + Utilities.Format_Anchor(_mychar_over._CharId.ToString()) + "<br>");
                        }
                    }
                }
                w.WriteLine("<tr><td>" + outline2 + "</td><td>" + outline3 + "</td></tr>");
            }
        }

        private static void Write_Char_Stacked_Under(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            if (_characters.Find(x => x._CharId == Convert.ToInt32(_myChar._LI_Where)) != null)
            {
                Character _mychar_under = _characters.Find(x => x._CharId == Convert.ToInt32(_myChar._LI_Where));
                if (_mychar_under._Char_Type == "0")
                {
                    w.WriteLine("<tr>");
                    w.WriteLine("<td>Stacked Under:</td>");
                    w.WriteLine("<td>" + _mychar_under._Name + " " + Utilities.Format_Anchor(_mychar_under._CharId.ToString()) + "</td>");
                    w.WriteLine("</tr>");
                }
            }
        }

        private static void Write_Char_Loyalty(Character _myChar, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Loyalty:</td>");
            w.WriteLine("<td>" + Utilities.Xlate_Loyalty(_myChar._CH_LOY_Kind, _myChar._CH_LOY_Rate) + "</td>");
            w.WriteLine("</tr>");
        }

        private static void Write_Char_Rank(Character _myChar, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Rank:</td>");
            w.WriteLine("<td>" + Utilities.Xlate_Rank(_myChar._CH_Rank) + "</td>");
            w.WriteLine("</tr>");
        }

        private static void Write_Char_Location(Character _myChar, StreamWriter w, List<Character> _characters, List<Location> _locations, List<Ship> _ships)
        {
            // location
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<tr>");
            w.WriteLine("<td>Location:</td>");
            outline.Append("<td>");
            Location _myloc = _locations.Find(x => x._LocId == Convert.ToInt32(_myChar._LI_Where));
            if (_myloc != null)
            {
                Determine_Char_Location(outline, _myloc, _locations);
            }
            else
            {
                Character _mychar2 = _characters.Find(x => x._CharId == Convert.ToInt32(_myChar._LI_Where));
                if (_mychar2 != null)
                {
                    Location _myloca = _locations.Find(x => x._LocId == Convert.ToInt32(_mychar2._LI_Where));
                    if (_myloca != null)
                    {
                        Determine_Char_Location(outline, _myloca, _locations);
                    }
                }
                else
                {
                    Ship _myship = _ships.Find(x => x._ShipId == Convert.ToInt32(_myChar._LI_Where));
                    if (_myship != null)
                    {
                        outline.Append(_myship._Name + " " + Utilities.Format_Anchor(_myship._ShipId.ToString()) + ", ");
                        Location _myloca = _locations.Find(x => x._LocId == _myship._LI_Where);
                        if (_myloca != null)
                        {
                            Determine_Char_Location(outline, _myloca, _locations);
                        }
                    }
                }
            }
            outline.Append("</td>");
            w.WriteLine(outline);
            w.WriteLine("</tr>");
        }

        public static void Determine_Char_Location(StringBuilder outline, Location _myloc, List<Location> _locations)
        {
            outline.Append(_myloc._Name + " " + Utilities.Format_Anchor(_myloc._LocId_Conv));
            if (_locations.Find(x => x._LocId == _myloc._LI_Where) != null)
            {
                Location _myloc2 = _locations.Find(x => x._LocId == _myloc._LI_Where);
                if (_myloc2._Loc_Type != "region")
                {
                    outline.Append(", in " + _myloc2._Name + " " + Utilities.Format_Anchor(_myloc2._LocId_Conv));
                    if (_locations.Find(x => x._LocId == _myloc2._LI_Where) != null)
                    {
                        Location _myloc3 = _locations.Find(x => x._LocId == _myloc2._LI_Where);
                        if (_myloc3._Loc_Type != "region")
                        {
                            outline.Append(", in " + _myloc3._Name + " " + Utilities.Format_Anchor(_myloc3._LocId_Conv));
                            if (_locations.Find(x => x._LocId == _myloc3._LI_Where) != null)
                            {
                                Location _myloc4 = _locations.Find(x => x._LocId == _myloc3._LI_Where);
                                if (_myloc4._Loc_Type != "region")
                                {
                                    outline.Append(", in " + _myloc4._Name + " " + Utilities.Format_Anchor(_myloc4._LocId_Conv));
                                }
                                else
                                {
                                    outline.Append(", in " + _myloc4._Name);
                                }
                            }
                        }
                        else
                        {
                            outline.Append(", in " + _myloc3._Name);
                        }
                    }
                }
                else
                {
                    outline.Append(", in " + _myloc2._Name);
                }
            }
        }
        private static void Write_Visions_Received(Character _myChar, StreamWriter w, List<Character> _characters)
        {
            if (_myChar._CM_Already_Visioned != null)
            {
                if (_myChar._CM_Already_Visioned.Count > 0)
                {
                    w.WriteLine("<p>Visions Received:</p>");
                    w.WriteLine("<table>");
                    int rows = (int)((_myChar._CM_Already_Visioned.Count / 2) + 0.5);
                    for (int i = 0; i < rows; i++)
                    {
                        w.WriteLine("<tr>");
                        Character mycharl = _characters.Find(x=>x._CharId == _myChar._CM_Already_Visioned[i]);
                        if (mycharl != null)
                        {
                            w.WriteLine("<td>{0} {1}</td>", mycharl._Name, Utilities.Format_Anchor(mycharl._CharId.ToString()));
                        }
                        else
                        {
                            w.WriteLine("<td>missing {0}</td>", _myChar._CM_Already_Visioned[i]);
                        }
                        if ((i + rows) <= _myChar._CM_Already_Visioned.Count)
                        {
                            Character mycharr = _characters.Find(x => x._CharId == _myChar._CM_Already_Visioned[i + rows]);
                            if (mycharr != null)
                            {
                                w.WriteLine("<td>{0} {1}</td>", mycharr._Name, Utilities.Format_Anchor(mycharr._CharId.ToString()));
                            }
                            else
                            {
                                w.WriteLine("<td>missing {0}</td>", _myChar._CM_Already_Visioned[i]);
                            }
                        }
                        else
                        {
                            w.WriteLine("<td>&nbsp;</td>");
                        }
                        w.WriteLine("</tr>");
                    }
                    w.WriteLine("</table>");
                }
            }
        }
    }
}
