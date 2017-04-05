using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Skill
    {
        public int _SkillId { get; set; }
        public string _Name { get; set; }
        public string _First_Line { get; set; }
        public int _SK_Time_to_learn { get; set; }
        public int _SK_Required_Skill { get; set; }
        public static void Add(string InputKey, string InputString, List<Skill> _skills)
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
            if (j1.SelectToken("firstline") != null && j1.SelectToken("firstline").HasValues)
            {
                myfl = (JArray)j1.SelectToken("firstline");
            }
            else
            {
                myfl = null;
            }

            _skills.Add(new Skill
            {
                _SkillId = Convert.ToInt32(InputKey),
                _First_Line = myfl[0].ToString(),
                _Name = myna[0].ToString(),
                _SK_Required_Skill = JSON.int_Token(j1, "SK.rs"),
                _SK_Time_to_learn = JSON.int_Token(j1, "SK.tl")

            });
        }
    }
}
