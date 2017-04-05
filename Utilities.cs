using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OlyCommonClasses
{
    public class Utilities
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        const string letters2 = "abcdfghjkmnpqrstvwxz";
        //string[,] letter2_to_int = { { "a","b","c","d","f","g","h","j","k","m","n","p","q","r","s","t","v","w","x","z"},
        //                             { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19"} };
        public static int _i(char c)
        {
            return ((char)c - 97);
        }
        public static string To_Oid(string oid_int)
        {
            //string letters = "abcdefghijklmnopqrstuvwxyz";
            //string letters2 = "abcdfghjkmnpqrstvwxz";
            string tooid = " ";
            //int oidint;
            int lets;
            int first;
            int second;
            int residue;

            if (int.TryParse(oid_int, out int oidint))
            {
                if (oidint < 10000) // character or item
                {
                    return oid_int;
                }
                else
                {
                    if (oidint < 50000) // location
                    {
                        oidint -= 10000;
                        lets = oidint / 100;
                        residue = oidint % 100;
                        first = lets / 20;
                        second = lets % 20;
                        tooid = (letters2.Substring(first,1) + letters2.Substring(second,1) + residue.ToString("D2"));
                        return tooid;
                    }
                    else
                    {
                        if (oidint < 56760) // CCN
                        {
                            oidint -= 50000;
                            lets = oidint / 10;
                            residue = oidint % 10;
                            first = lets / 26;
                            second = lets % 26;
                            tooid = (letters.Substring(first,1) + letters.Substring(second,1) + residue.ToString("D1"));
                            return tooid;
                        }
                        else
                        {
                            if (oidint < 58760) // CNN
                            {
                                oidint -= 56760;
                                residue = oidint % 100;
                                first = oidint / 100;
                                tooid = (letters2.Substring(first,1) + residue.ToString("D2"));
                                return tooid;
                            }
                            else
                            {
                                if (oidint < 59000)
                                {
                                    return oid_int;
                                }
                                else
                                {
                                    if (oidint < 79000) // CNNN
                                    {
                                        oidint -= 59000;
                                        residue = oidint % 1000;
                                        first = oidint / 1000;
                                        return (letters2.Substring(first,1) + residue.ToString("D3"));
                                    }
                                    else // storms, controlled units, etc
                                    {
                                        return oid_int;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                tooid = "Error";
                return tooid;
            }
        }
        public static string To_Int(string oid)
        {
            string[,] letter2_to_int = { { "a", "0" }, { "b", "1" },{ "c", "2" }, { "d", "3" }, { "f", "4" }, { "g", "5" }, { "h", "6" }, { "j", "7" }, { "k", "8" }, { "m", "9" }, { "n", "10" }, { "p", "11" }, { "q", "12" }, { "r", "13" }, { "s", "14" }, { "t", "15" }, { "v", "16" }, { "w", "17" }, { "x", "18" }, { "z", "19" } };

            if (Regex.IsMatch(oid, "^[a-z][a-z][0-9]$")) //CCN
            {
                //str(_i(oid[0])*26*10 + _i(oid[1])*10 + int(oid[2]) + 50000)
                return Convert.ToString((_i(Convert.ToChar(oid.Substring(0, 1))) * 26 * 10) + (_i(Convert.ToChar(oid.Substring(1, 1))) * 10) + (Convert.ToInt32(oid.Substring(2, 1)) + 50000));
            }
            else
            {
                if (Regex.IsMatch(oid, "^[a-z][a-z][0-9][0-9]$")) //CCNN
                {
                    //str(letter2_to_int[oid[0]]*20*100 + letter2_to_int[oid[1]]*100 + int(oid[2:]) + 10000)
                    Int32 Byte1 = 0;
                    Int32 Byte2 = 0;
                    for (int i = 0; i < letter2_to_int.GetLength(0); i++)
                    {
                        if (letter2_to_int[i,0] == oid.Substring(0, 1))
                        {
                            Byte1 = Convert.ToInt32(letter2_to_int[i, 1]) * 20 * 100;
                            break;
                        }
                    }
                    for (int i = 0; i < letter2_to_int.GetLength(0); i++)
                    {
                        if (letter2_to_int[i, 0] == oid.Substring(1, 1))
                        {
                            Byte2 = Convert.ToInt32(letter2_to_int[i, 1]) * 100;
                            break;
                        }
                    }
                    return Convert.ToString(Byte1 + Byte2 + Convert.ToInt32(oid.Substring(2,2)) + 10000);
                }
                else
                {
                    if (Regex.IsMatch(oid, "^[a-z][0-9][0-9]$")) //CNN
                    {
                        //str(letter2_to_int[oid[0]]*100 + int(oid[1:]) + 56760)
                        Int32 Byte1 = 0;
                        for (int i = 0; i < letter2_to_int.GetLength(0); i++)
                        {
                            if (letter2_to_int[i, 0] == oid.Substring(0, 1))
                            {
                                Byte1 = Convert.ToInt32(letter2_to_int[i, 1]) * 100;
                                break;
                            }
                        }
                        return Convert.ToString(Byte1 +  Convert.ToInt32(oid.Substring(1, 2)) + 56760);
                    }
                    else
                    {
                        if (Regex.IsMatch(oid, "^[a-z][0-9][0-9][0-9]$")) //CNNN
                        {
                            //str(letter2_to_int[oid[0]]*1000 + int(oid[1:]) + 59000)
                            Int32 Byte1 = 0;
                            for (int i = 0; i < letter2_to_int.GetLength(0); i++)
                            {
                                if (letter2_to_int[i, 0] == oid.Substring(0, 1))
                                {
                                    Byte1 = Convert.ToInt32(letter2_to_int[i, 1]) * 1000;
                                    break;
                                }
                            }
                            return Convert.ToString(Byte1 + Convert.ToInt32(oid.Substring(1, 3)) + 59000);
                        }
                        else
                        {
                            if (Regex.IsMatch(oid, "^[0-9]{1,5}$")) //N through NNNNN
                            {
                                //str(oid)
                                return Convert.ToString(oid);
                            }
                            else
                            {
                                if (Regex.IsMatch(oid, "^1[0-9][0-9][0-9][0-9][0-9]$")) //1NNNNN
                                {
                                    //str(oid)
                                    return Convert.ToString(oid);
                                }
                                else
                                {
                                    return "Error";
                                }
                            }
                        }
                    }
                }
            }
        }
        public static string Format_Anchor(string target)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<a href=\"");
            outline.Append(target);
            outline.Append(".html\">[");
            outline.Append(target);
            outline.Append("]</a>");
            return outline.ToString();
        }
        public static string Format_Anchor2(string target, string label)
        {
            StringBuilder outline = new StringBuilder();
            outline.Append("<a href=\"");
            outline.Append(target);
            outline.Append(".html\">");
            outline.Append(Utilities.Format_Loc_Type(label));
            outline.Append("</a>");
            return outline.ToString();
        }
        public static string Format_Loc_Type(string label)
        {
            if (label.Contains("poppy"))
            {
                label = label.Replace("poppy ", "p.");
            }
            else
            {
                if (label.Contains("sacred"))
                {
                    label = label.Replace("sacred grove", "s.grov");
                }
                else
                {
                    if (label.Contains("rocky"))
                    {
                        label = label.Replace("rocky ", "r.");
                    }
                    else
                    {
                        if (label.Contains("battlefield"))
                        {
                            label = label.Replace("battlefield", "btfd.");
                        }
                        else
                        {
                            if (label.Contains("sand"))
                            {
                                label = label.Replace("sand ", "s.");
                            }
                            else
                            {
                                if (label.Contains("graveyard"))
                                {
                                    label = label.Replace("graveyard", "gvyd.");
                                }
                                else
                                {
                                    if (label.Contains("port"))
                                    {
                                        label = label.Replace("port ", "p.");
                                    }
                                    else
                                    {
                                        if (label.Contains("yew"))
                                        {
                                            label = label.Replace(" grove", "");
                                        }
                                        else
                                        {
                                            if (label.Contains("pasture"))
                                            {
                                                label = label.Replace("pasture", "past.");
                                            }
                                            else
                                            {
                                                if (label.Contains("circle of"))
                                                {
                                                    label = label.Replace("circle of ", "c.");
                                                }
                                                else
                                                {
                                                    if (label.Contains("ring of"))
                                                    {
                                                        label = label.Replace("ring of ", "r.");
                                                    }
                                                    else
                                                    {
                                                        if (label.Contains("enchanted forest"))
                                                        {
                                                            label = label.Replace("enchanted forest", "e.forst");
                                                        }
                                                        else
                                                        {
                                                            if (label.Contains("mallorn"))
                                                            {
                                                                label = label.Replace("mallorn grove", "m.grov");
                                                            }
                                                            else
                                                            {
                                                                if (label.Contains("faery"))
                                                                {
                                                                    label = label.Replace("faery ", "f.");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return label;
        }
        public static string Xlate_Rank(int rank_id)
        {
            switch (rank_id)
            {
                case 10: return "lord";
                case 20: return "knight";
                case 30: return "baron";
                case 40: return "count";
                case 50: return "earl";
                case 60: return "marquess";
                case 70: return "duke";
                case 80: return "king";
                default: return "undefined rank";
            }
        }
        public static string Xlate_Experience(int experience_id)
        {
            switch (experience_id)
            {
                case 1: return "apprentice";
                case 2: return "journeyman";
                case 3: return "adept";
                case 4: return "master";
                case 5: return "grand master";
                default: return "undefined";
            }
        }
        public static string Xlate_Loyalty(int loy_kind, int loy_rate)
        {
            switch (loy_kind)
            {
                case 0: return "Unsworn";
                case 1: return "Contract-" + loy_rate;
                case 2: return "Oath-" + loy_rate;
                case 3: return "Fear-" + loy_rate;
                case 4: return "Npc";
                case 5: return "Summon";
                default: return "Undefined";
            }
        }
        public static int Chase_Pledge_Chain(int in_char_id, List<Character> _characters)
        {
            if (_characters.Find(x=>x._CharId == in_char_id)._CM_Pledged_To != 0)
            {
                int return_char_id = Chase_Pledge_Chain(_characters.Find(x => x._CharId == in_char_id)._CM_Pledged_To, _characters);
                return return_char_id;
            }
            return in_char_id;
        }
    }
}
