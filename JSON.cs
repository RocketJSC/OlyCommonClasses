using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace OlyCommonClasses
{
    public class JSON
    {
        public static List<int> list_Token(JObject j1, string token)
        {
            if (j1.SelectToken(token) != null && j1.SelectToken(token).HasValues)
            {
                JArray mytoken;
                mytoken = (JArray)j1.SelectToken(token);
                List<int> mytokenlist;
                mytokenlist = mytoken.ToObject<List<int>>();
                return mytokenlist.ToList();
            }
            return null;
        }
        public static int int_Token(JObject j1, string token, int default_value = 0)
        {
            if (j1.SelectToken(token) != null && j1.SelectToken(token).HasValues)
            {
                JArray mytoken;
                mytoken = (JArray)j1.SelectToken(token);
                return (int)(mytoken[0]);
            }
            return default_value;
        }
        public static string string_Token(JObject j1, string token)
        {
            if (j1.SelectToken(token) != null && j1.SelectToken(token).HasValues)
            {
                JArray mytoken;
                mytoken = (JArray)j1.SelectToken(token);
                string combined_string = mytoken[0].ToString();
                if (mytoken.Count > 1)
                {
                    for (int i = 1; i < mytoken.Count; i++)
                    {
                        combined_string += " " + mytoken[i].ToString();
                    }
                }
                return combined_string;
            }
            return null;
        }
    }
}
