using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Storm
    {
        public int _StormId { get; set; }
        public string _First_Line { get; set; }
        public string _Storm_Type { get; set; }
        public int _Storm_Strength { get; set; }
        public int _Where { get; set; }
        public static void Add(string InputKey, string InputString, List<Storm> _storms)
        {
            JObject j1 = JObject.Parse(InputString);
            JArray myna;
            JArray myfl;
            if (j1.SelectToken("na") != null && j1.SelectToken("na").HasValues)
            {
                myna = (JArray)j1.SelectToken("na");
            }
            else
            {
                myna = null;
            }
            string mystormtype;
            if (j1.SelectToken("firstline") != null && j1.SelectToken("firstline").HasValues)
            {
                myfl = (JArray)j1.SelectToken("firstline");
                string[] type_array = myfl[0].ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                mystormtype = type_array[2];
            }
            else
            {
                myfl = null;
                mystormtype = null;
            }

            _storms.Add(new Storm
            {
                _StormId = Convert.ToInt32(InputKey),
                _First_Line = myfl[0].ToString(),
                _Storm_Type = mystormtype,
                _Where = JSON.int_Token(j1, "LI.wh"),
                _Storm_Strength = JSON.int_Token(j1, "MI.ss")
            });
        }
    }
}
