using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Stack
    {
        public int _entityid { get; set; }
        public string _entity_type { get; set; }
        public int _entity_level { get; set; }
        public static Stack Add(int entity_id, string loc, int level)
        {
            Stack stack_entry = new Stack
            {
                _entityid = entity_id,
                _entity_type = loc,
                _entity_level = level
            };
            return stack_entry;
        }
        public static List<Stack> Chase_Structure(int entity_id, List<Stack> stack_list, int level, List<Character> _characters, List<Location> _locations, List<Ship> _ships)
        {
            if (_locations.Find(x => x._LocId == entity_id) != null)
            {
                stack_list.Add(Add(entity_id, "loc", level));
                Location myloc = _locations.Find(x => x._LocId == entity_id);
                if (myloc._LI_Here_List != null)
                {
                    level++;
                    foreach (int loc in myloc._LI_Here_List)
                    {
                        stack_list = Chase_Structure(loc, stack_list, level, _characters, _locations, _ships);
                    }
                }
            }
            else
            {
                if (_characters.Find(x => x._CharId == entity_id) != null)
                {
                    stack_list.Add(Add(entity_id, "char", level));
                    Character mychar = _characters.Find(x => x._CharId == entity_id);
                    if (mychar._LI_Here_List != null)
                    {
                        level++;
                        foreach (int chars in mychar._LI_Here_List)
                        {
                            stack_list = Chase_Structure(chars, stack_list, level, _characters, _locations, _ships);
                        }
                    }
                }
                else
                {
                    if (_ships.Find(x => x._ShipId == entity_id) != null)
                    {
                        stack_list.Add(Add(entity_id, "ship", level));
                        Ship myship = _ships.Find(x => x._ShipId == entity_id);
                        if (myship._LI_Here_List != null)
                        {
                            level++;
                            foreach (int ship in myship._LI_Here_List)
                            {
                                stack_list = Chase_Structure(ship, stack_list, level, _characters, _locations, _ships);
                            }
                        }
                    }
                    else
                    {
                        stack_list.Add(Add(entity_id, "unknown", level));
                    }
                }
            }
            return stack_list;
        }
    }
}
