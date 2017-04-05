using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class TradeXref
    {
        public int _ItemId { get; set; }
        public string _ItemId_Conv { get; set; }
        public string _Item_Name { get; set; }
        public int _Sell_Who_id { get; set; }
        public int _Sell_Region { get; set; }
        public string _Sell_Who_id_Conv { get; set; }
        public int _Sell_Who_Type { get; set; }
        public int _Sell_Number { get; set; }
        public int _Sell_Price { get; set; }
        public int _Buy_Who_id { get; set; }
        public int _Buy_Region { get; set; }
        public string _Buy_Who_id_Conv { get; set; }
        public int _Buy_Who_Type { get; set; }
        public int _Buy_Number { get; set; }
        public int _Buy_Price { get; set; }

        public static void Load_Sells(Location mylocation, Character mycharacter, List<TradeXref> mytradexref)
        {
            int l_itemid = 0;
            int l_entityid = 0;
            int l_qty = 0;
            int l_cost = 0;
            int l_region = 0;
            if (mylocation != null)
            {
                if (mylocation._Trade_List != null)
                {
                    int iterations = mylocation._Trade_List.Count() / 8;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (mylocation._Trade_List[(i * 8) + 0] == 2)
                        {
                            l_entityid = mylocation._LocId;
                            l_region = mylocation._Region_id;
                            l_itemid = mylocation._Trade_List[(i * 8) + 1];
                            l_qty = Convert.ToInt32(mylocation._Trade_List[(i * 8) + 2]);
                            l_cost = Convert.ToInt32(mylocation._Trade_List[(i * 8) + 3]);
                            if (mytradexref.Find(x => x._ItemId == l_itemid) == null)
                            {
                                if (mylocation._Trade_List[(i * 8) + 1] > 300) // don't load common stuff
                                {
                                    Add(mytradexref, l_itemid, l_entityid, l_qty, l_cost, l_region);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Load_Buys(Location mylocation, Character mycharacter, List<TradeXref> mytradexref)
        {
            int l_itemid = 0;
            int l_entityid = 0;
            int l_qty = 0;
            int l_cost = 0;
            int l_region = 0;
            if (mylocation != null)
            {
                if (mylocation._Trade_List != null)
                {
                    int iterations = mylocation._Trade_List.Count() / 8;
                    for (int i = 0; i < iterations; i++)
                    {
                        if (mylocation._Trade_List[(i * 8) + 0] == 1)
                        {
                            l_entityid = mylocation._LocId;
                            l_region = mylocation._Region_id;
                            l_itemid = mylocation._Trade_List[(i * 8) + 1];
                            l_qty = Convert.ToInt32(mylocation._Trade_List[(i * 8) + 2]);
                            l_cost = Convert.ToInt32(mylocation._Trade_List[(i * 8) + 3]);
                            if (mytradexref.Find(x => x._ItemId == l_itemid) != null)
                            {
                                if (mylocation._Trade_List[(i * 8) + 1] > 300) // don't load common stuff
                                {
                                    TradeXref _tradexref = mytradexref.Find(x => x._ItemId == l_itemid);
                                    if (_tradexref != null)
                                    {
                                        _tradexref._Buy_Who_id = l_entityid;
                                        _tradexref._Buy_Number = l_qty;
                                        _tradexref._Buy_Price = l_cost;
                                        _tradexref._Buy_Region = l_region;
                                    }
                                }
                            }
                            else
                            {
                                Add_Buy(mytradexref, l_itemid, l_entityid, l_qty, l_cost, l_region);
                            }
                        }
                    }
                }
            }
        }
        private static void Add(List<TradeXref> mytradexref, int l_itemid, int l_entityid, int l_qty, int l_cost, int l_region)
        {
            mytradexref.Add(new TradeXref
            {
                _ItemId = l_itemid,
                _Sell_Who_id = l_entityid,
                _Sell_Region = l_region,
                _Sell_Number = l_qty,
                _Sell_Price = l_cost,
                _Buy_Who_id = 0,
                _Buy_Number = 0,
                _Buy_Price = 0,
                _Buy_Region = 0
            });
        }
        private static void Add_Buy(List<TradeXref> mytradexref, int l_itemid, int l_entityid, int l_qty, int l_cost, int l_region)
        {
            mytradexref.Add(new TradeXref
            {
                _ItemId = l_itemid,
                _Buy_Who_id = l_entityid,
                _Buy_Region = l_region,
                _Buy_Number = l_qty,
                _Buy_Price = l_cost,
                _Sell_Who_id = 0,
                _Sell_Number = 0,
                _Sell_Price = 0,
                _Sell_Region = 0
            });
        }
    }
}

