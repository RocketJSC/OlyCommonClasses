using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Trade
    {
        public int _ItemId { get; set; }
        public string _ItemId_Conv { get; set; }
        public int _Who_id { get; set; }
        public string _Who_id_Conv { get; set; }
        public int _Who_Type { get; set; }
        public int _Trade_Kind { get; set; }
        public int _Number { get; set; }
        public int _Price { get; set; }
        public int _Weight { get; set; }
        public string _Item_Name { get; set; }
        public int _recip_Who_id { get; set; }
        public string _recip_Who_id_Conv { get; set; }
        public int _recip_Who_Type { get; set; }
        public int _recip_Trade_Kind { get; set; }
        public int _recip_Number { get; set; }
        public int _recip_Price { get; set; }
    }
}
