using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Ship
    {
        public int _ShipId { get; set; }
        public string _Name { get; set; }
        public string _First_Line { get; set; }
        public string _Ship_Type { get; set; }
        public List<int> _LI_Here_List { get; set; }
        public int _LI_Where { get; set; }
        public int _SL_Damage { get; set; }
        public int _SL_Defense { get; set; }
        public int _SL_Capacity { get; set; }
        public int _SL_Moving { get; set; }
        public int _SL_Bound_Storm { get; set; }
        public int _SL_Effort_Required { get; set; }
        public int _SL_Effort_Given { get; set; }
        public string Calc_CurrentLoc { get; set; }
        public int Calc_CurrentRegion { get; set; }
        public static void Add(string InputKey, string InputString, List<Ship> _ships)
        {
            JObject j1 = JObject.Parse(InputString);
            JArray myfl;
            JArray myna;
            string mychartype;
            if (j1.SelectToken("na") != null && j1.SelectToken("na").HasValues)
            {
                myna = (JArray)j1.SelectToken("na");
            }
            else
            {
                myna = null;
            }
            if (j1.SelectToken("firstline") != null && j1.SelectToken("firstline").HasValues)
            {
                myfl = (JArray)j1.SelectToken("firstline");
                string[] type_array = myfl[0].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                mychartype = type_array[2];
            }
            else
            {
                myfl = null;
                mychartype = null;
            }
            _ships.Add(new Ship
            {
                _ShipId = Convert.ToInt32(InputKey),
                _First_Line = myfl[0].ToString(),
                _Name = myna[0].ToString(),
                _Ship_Type = mychartype,
                _LI_Here_List = JSON.list_Token(j1, "LI.hl"),
                _LI_Where = JSON.int_Token(j1, "LI.wh"),
                _SL_Bound_Storm = JSON.int_Token(j1, "SL.bs"),
                _SL_Capacity = JSON.int_Token(j1, "SL.ca"),
                _SL_Damage = JSON.int_Token(j1, "SL.da"),
                _SL_Defense = JSON.int_Token(j1, "SL.de"),
                _SL_Effort_Given = JSON.int_Token(j1, "SL.eg"),
                _SL_Effort_Required = JSON.int_Token(j1, "SL.er"),
                _SL_Moving = JSON.int_Token(j1, "SL.mo")
            });
        }
        public static int Determine_Ship_Weight(List<Stack> ship_stack, List<Character> _characters, List<Itemz> _items)
        {
            int total_weight = 0;
            foreach (Stack stack_entry in ship_stack.Where(x => x._entity_type == "char"))
            {
                if (_characters.Find(x => x._CharId == stack_entry._entityid) != null)
                {
                    total_weight += Character.Determine_Unit_Weights(_characters.Find(x => x._CharId == stack_entry._entityid),_items)._total_weight;
                }
            }
            return total_weight;
        }
    }
}
