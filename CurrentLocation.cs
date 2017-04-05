using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class CurrentLocation
    {
        public string _current_loc { get; set; }
        public int _current_region { get; set; }
        public int _current_loctype { get; set; }
        public static CurrentLocation Where_Am_I(int entityid, CurrentLocation mycurrloc, List<Character> _characters, List<Location> _locations,  List<Ship> _ships)
        {
            if (entityid != 0)
            {
                Character mychar = _characters.Find(x => x._CharId == entityid);
                if (mychar != null)
                {
                    mycurrloc = Where_Am_I(mychar._LI_Where, mycurrloc, _characters, _locations,  _ships);
                }
                else
                {
                    Ship myship = _ships.Find(x => x._ShipId == entityid);
                    if (myship != null)
                    {
                        mycurrloc._current_loctype = (mycurrloc._current_loctype == 0 ? 2 : mycurrloc._current_loctype);
                        mycurrloc._current_loc += (mycurrloc._current_loc == null ? myship._ShipId.ToString() : "|" + myship._ShipId.ToString());
                        mycurrloc = Where_Am_I(myship._LI_Where, mycurrloc, _characters, _locations,  _ships);
                    }
                    else
                    {
                        Location myloc = _locations.Find(x => x._LocId == entityid);
                        if (myloc != null)
                        {
                            mycurrloc._current_loctype = (mycurrloc._current_loctype == 0 ? 1 : mycurrloc._current_loctype);
                            if (_locations.Find(f => f._LocId == myloc._LI_Where)._Loc_Type == "region")
                            {
                                mycurrloc._current_loc += (mycurrloc._current_loc == null ? myloc._LocId_Conv : "|" + myloc._LocId_Conv);
                                mycurrloc._current_region = myloc._LI_Where;
                            }
                            else
                            {
                                mycurrloc._current_loc += (mycurrloc._current_loc == null ? myloc._LocId_Conv : "|" + myloc._LocId_Conv);
                                //Location myloc2 = Program._locations.Find(f => f._LocId == myloc._LI_Where);
                                mycurrloc = Where_Am_I(myloc._LI_Where, mycurrloc, _characters, _locations, _ships);
                            }
                        }
                        else
                        {
                            mycurrloc._current_loc = "unknown";
                            mycurrloc._current_region = 999;
                            mycurrloc._current_loctype = 999;
                        }
                    }
                }
            }
            return mycurrloc;
        }
    }
}
