using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class HTML_Ship
    {
        public static void Write_Ship_HTML_File(Ship _myship, string path, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships, List<Storm> _storms)
        {
            using (FileStream fs = new FileStream(System.IO.Path.Combine(path, _myship._ShipId + ".html"), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<HTML>");
                    w.WriteLine("<HEAD>");
                    w.WriteLine("<TITLE>" + _myship._Name +
                                " [" + _myship._ShipId + "], " +
                                _myship._Ship_Type +
                                "</TITLE>");
                    w.WriteLine("</HEAD>");
                    w.WriteLine("<BODY>");
                    Write_Ship_Page_Header(_myship, w);
                    Write_Ship_Basic_Info(_myship, w, _characters, _items, _locations, _ships,  _storms);
                    w.WriteLine("</BODY>");
                    w.WriteLine("</HTML>");
                }
                fs.Dispose();
            }
        }
        private static void Write_Ship_Page_Header(Ship _myship, StreamWriter w)
        {
            StringBuilder outline3 = new StringBuilder();
            outline3.Append("<H3>");
            outline3.Append(_myship._Name);
            outline3.Append(" [");
            outline3.Append(_myship._ShipId);
            outline3.Append("]");
            outline3.Append(", " + _myship._Ship_Type);
            outline3.Append("</H3>");
            w.WriteLine(outline3);
        }
        private static void Write_Ship_Basic_Info(Ship _myship, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships,  List<Storm> _storms)
        {
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<table>");
            Write_Ship_Location(_myship, w, _locations);
            Write_Ship_Pct_Complete(_myship, w);
            Write_Ship_Pct_Loaded(_myship, w, _characters, _items, _locations, _ships);
            Write_Ship_Defense(_myship, w);
            Write_Ship_Damaged(_myship, w);
            Write_Ship_Owner(_myship, w, _characters);
            Write_Ship_Seen_Here(_myship, w, _characters, _locations, _ships);
            Write_Ship_Bound_Storm(_myship, w, _storms);
            w.WriteLine("</table>");
        }

        private static void Write_Ship_Bound_Storm(Ship _myship, StreamWriter w, List<Storm> _storms)
        {
            if (_myship._SL_Bound_Storm != 0)
            {
                Storm _mystorm = _storms.Find(x => x._StormId == _myship._SL_Bound_Storm);
                if (_mystorm != null)
                {
                    w.WriteLine("<tr>");
                    w.WriteLine("<td>Bound Storm:</td>");
                    w.WriteLine("<td>" + _mystorm._Storm_Type + " [" + _myship._SL_Bound_Storm + "] (strength: " + _mystorm._Storm_Strength + ")</td>");
                    w.WriteLine("</tr>");
                }
            }
        }
        private static void Write_Ship_Seen_Here(Ship _myship, StreamWriter w, List<Character> _characters, List<Location> _locations, List<Ship> _ships)
        {
            List<Stack> ship_stack = new List<Stack>();
            int level = 0;
            ship_stack = Stack.Chase_Structure(_myship._ShipId, ship_stack, level, _characters, _locations, _ships);
            if (ship_stack.Count > 1)
            {
                string label1 = "Seen Here:";
                foreach (Stack stack_entry in ship_stack.Where(x => x._entity_type == "char"))
                {
                    if (_characters.Find(x => x._CharId == stack_entry._entityid) != null)
                    {
                        Character _mychar = _characters.Find(x => x._CharId == stack_entry._entityid);
                        w.WriteLine("<tr>");
                        w.WriteLine("<td>" + label1 + "</td>");
                        label1 = "";
                        StringBuilder outline = new StringBuilder();
                        outline.Append("<td>");
                        for (int i = 0; i < (stack_entry._entity_level - 1); i++)
                        {
                            outline.Append("*&nbsp;&nbsp;");
                        }
                        outline.Append(_mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()));
                        outline.Append("</td>");
                        w.WriteLine(outline);
                        w.WriteLine("</tr>");
                    }
                }
            }
        }

        private static void Write_Ship_Owner(Ship _myship, StreamWriter w, List<Character> _characters)
        {
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<tr>");
            w.WriteLine("<td>Owner:</td>");
            outline.Append("<td>");
            if (_myship._LI_Here_List != null)
            {
                if (_myship._LI_Here_List.Count > 0)
                {
                    Character _mychar = _characters.Find(x => x._CharId == _myship._LI_Here_List[0]);
                    outline.Append(_mychar._Name + " " + Utilities.Format_Anchor(_mychar._CharId.ToString()));
                }
                else
                {
                    outline.Append("unoccupied");
                }
            }
            else
            {
                outline.Append("unoccupied");
            }
            outline.Append("</td>");
            w.WriteLine(outline);
            w.WriteLine("</tr>");
        }

        private static void Write_Ship_Damaged(Ship _myship, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Damaged:</td>");
            w.WriteLine("<td>" + _myship._SL_Damage + "%</td>");
            w.WriteLine("</tr>");
        }

        private static void Write_Ship_Defense(Ship _myship, StreamWriter w)
        {
            w.WriteLine("<tr>");
            w.WriteLine("<td>Defense:</td>");
            w.WriteLine("<td>" + _myship._SL_Defense + "</td>");
            w.WriteLine("</tr>");
        }

        private static void Write_Ship_Pct_Loaded(Ship _myship, StreamWriter w, List<Character> _characters, List<Itemz> _items, List<Location> _locations, List<Ship> _ships)
        {
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<td>Loaded:</td>");
            outline.Append("<td>");
            // calculate load of all passengers
            List<Stack> ship_stack = new List<Stack>();
            int level = 0;
            ship_stack = Stack.Chase_Structure(_myship._ShipId, ship_stack, level, _characters, _locations, _ships);
            if (ship_stack.Count > 1)
            {
                int total_weight = Ship.Determine_Ship_Weight(ship_stack, _characters, _items);
                int actual_capacity = _myship._SL_Capacity - ((_myship._SL_Capacity * _myship._SL_Damage) / 100);
                outline.Append(((total_weight * 100) / actual_capacity) + "%");
            }
            else
            {
                outline.Append("0%");
            }
            outline.Append("</td>");
            w.WriteLine(outline);
            w.WriteLine("</tr>");
        }
        private static void Write_Ship_Pct_Complete(Ship _myship, StreamWriter w)
        {
            if (_myship._SL_Effort_Given < _myship._SL_Effort_Required)
            {
                w.WriteLine("<tr>");
                w.WriteLine("<td>Percent Complete:</td>");
                w.WriteLine("<td>" + ((float)_myship._SL_Effort_Given / (float)_myship._SL_Effort_Required) * 100f + "%</td>");
                w.WriteLine("</tr>");
            }
            w.WriteLine("<tr>");
        }

        private static void Write_Ship_Location(Ship _myship, StreamWriter w, List<Location> _locations)
        {
            StringBuilder outline = new StringBuilder();
            w.WriteLine("<tr>");
            w.WriteLine("<td>Location:</td>");
            outline.Append("<td>");
            Location _myloc = _locations.Find(x => x._LocId == Convert.ToInt32(_myship._LI_Where));
            if (_myloc != null)
            {
                HTML_Char.Determine_Char_Location(outline, _myloc, _locations);
            }
            outline.Append("</td>");
            w.WriteLine(outline);
            outline.Clear();
            w.WriteLine("</tr>");
        }
    }
}
