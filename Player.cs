using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Player
    {
        public int _FactionId { get; set; }
        public string _FactionId_Conv { get; set; }
        public String _First_Line { get; set; }
        public string _Name { get; set; }
        public string _Player_Type { get; set; }
        public string _Full_Name { get; set; }
        public string _Email_Address { get; set; }
        public int _Fast_Study_Points { get; set; }
        public int _Noble_Points { get; set; }
        public int _First_Turn { get; set; }
        public int _Last_Turn { get; set; }
        public List<int> _Known { get; set; }
        public List<int> _Unit_List { get; set; }
        public List<int> _Unformed_Unit_List { get; set; }
        public string _Password { get; set; }

        public static void Add(string InputKey, string InputString, List<Player> _players)
        {
            JObject j1 = JObject.Parse(InputString);
            JArray myna;
            JArray myfl;
            string myplayertype;
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
                myplayertype = type_array[2];
            }
            else
            {
                myfl = null;
                myplayertype = null;
            }

            _players.Add(new Player
            {
                _FactionId = Convert.ToInt32(InputKey),
                _FactionId_Conv = Utilities.To_Oid(InputKey),
                _First_Line = myfl[0].ToString(),
                _Name = myna[0].ToString(),
                _Player_Type = myplayertype,
                _Email_Address = JSON.string_Token(j1, "PL.em"),
                _Fast_Study_Points = JSON.int_Token(j1, "PL.fs"),
                _First_Turn = JSON.int_Token(j1, "PL.ft"),
                _Full_Name = JSON.string_Token(j1,"PL.fn"),
                _Known = JSON.list_Token(j1, "PL.kn"),
                _Last_Turn = JSON.int_Token(j1, "PL.lt"),
                _Noble_Points = JSON.int_Token(j1, "PL.np"),
                _Password = JSON.string_Token(j1, "PL.pw"),
                _Unformed_Unit_List = JSON.list_Token(j1, "PL.uf"),
                _Unit_List = JSON.list_Token(j1, "PL.un")
            });
        }
    }
}
