﻿using System.Collections.Generic;
using System.Text;

namespace Common.Instrastructure.Helpers
{
    public static class Transliter
    {
        #region dictionary
        private static readonly Dictionary<string, string> TransliterDictionaryRuToEn = new Dictionary<string, string>
        {
            {"а", "a"},
            {"б", "b"},
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "e"},
            {"ё", "yo"},
            {"ж", "zh"},
            {"з", "z"},
            {"и", "i"},
            {"й", "j"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "s"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "h"},
            {"ц", "c"},
            {"ч", "ch"},
            {"ш", "sh"},
            {"щ", "sch"},
            {"ъ", "j"},
            {"ы", "i"},
            {"ь", "j"},
            {"э", "e"},
            {"ю", "yu"},
            {"я", "ya"},
            {"А", "A"},
            {"Б", "B"},
            {"В", "V"},
            {"Г", "G"},
            {"Д", "D"},
            {"Е", "E"},
            {"Ё", "Yo"},
            {"Ж", "Zh"},
            {"З", "Z"},
            {"И", "I"},
            {"Й", "J"},
            {"К", "K"},
            {"Л", "L"},
            {"М", "M"},
            {"Н", "N"},
            {"О", "O"},
            {"П", "P"},
            {"Р", "R"},
            {"С", "S"},
            {"Т", "T"},
            {"У", "U"},
            {"Ф", "F"},
            {"Х", "H"},
            {"Ц", "C"},
            {"Ч", "Ch"},
            {"Ш", "Sh"},
            {"Щ", "Sch"},
            {"Ъ", "J"},
            {"Ы", "I"},
            {"Ь", "J"},
            {"Э", "E"},
            {"Ю", "Yu"},
            {"Я", "Ya"}
        };

        private static readonly Dictionary<string, string> TransliterDictionaryEnToRu = new Dictionary<string, string>
        {
            {"a", "а"},
            {"b", "б"},
            {"v", "в"},
            {"g", "г"},
            {"d", "д"},
            {"e", "е"},
            {"yo", "ё"},
            {"zh", "ж"},
            {"z", "з"},
            {"i", "и"},
            {"j", "й"},
            {"k", "к"},
            {"l", "л"},
            {"m", "м"},
            {"n", "н"},
            {"o", "о"},
            {"p", "п"},
            {"r", "р"},
            {"s", "с"},
            {"t", "т"},
            {"u", "у"},
            {"f", "ф"},
            {"h", "х"},
            {"c", "ц"},
            {"ch", "ч"},
            {"sh", "ш"},
            {"sch", "щ"},
            {"\"", "ъ"},
            {"y", "ы"},
            {"'", "ь"},
            {"je", "э"},
            {"yu", "ю"},
            {"ya", "я"},
            {"A", "А"},
            {"B", "Б"},
            {"V", "В"},
            {"G", "Г"},
            {"D", "Д"},
            {"E", "Е"},
            {"Yo", "Ё"},
            {"Zh", "Ж"},
            {"Z", "З"},
            {"I", "И"},
            {"J", "Й"},
            {"K", "К"},
            {"L", "Л"},
            {"M", "М"},
            {"N", "Н"},
            {"O", "О"},
            {"P", "П"},
            {"R", "Р"},
            {"S", "С"},
            {"T", "Т"},
            {"U", "У"},
            {"F", "Ф"},
            {"H", "Х"},
            {"C", "Ц"},
            {"Ch", "Ч"},
            {"Sh", "Ш"},
            {"Sch", "Щ"},
            {"Y", "Ы"},
            {"Je","Э"},
            {"Yu", "Ю"},
            {"Ya", "Я"}
        };

        private static readonly Dictionary<string, string> TransliterDictionaryKeyboardEnToRu = new Dictionary
            <string, string>
        {
            {"q", "й"},
            {"w", "ц"},
            {"e", "у"},
            {"r", "к"},
            {"t", "е"},
            {"y", "н"},
            {"u", "г"},
            {"i", "ш"},
            {"o", "щ"},
            {"p", "з"},
            {"[", "х"},
            {"]", "ъ"},
            {"a", "ф"},
            {"s", "ы"},
            {"d", "в"},
            {"f", "а"},
            {"g", "п"},
            {"h", "р"},
            {"j", "о"},
            {"k", "л"},
            {"l", "д"},
            {";", "ж"},
            {"'", "э"},
            {"z", "я"},
            {"x", "ч"},
            {"c", "с"},
            {"v", "м"},
            {"b", "и"},
            {"n", "т"},
            {"m", "ь"},
            {",", "б"},
            {".", "ю"},
            {"/", "."},
            {"Q", "Й"},
            {"W", "Ц"},
            {"E", "У"},
            {"R", "К"},
            {"T", "Е"},
            {"Y", "Н"},
            {"U", "Г"},
            {"I", "Ш"},
            {"O", "Щ"},
            {"P", "З"},
            {"A", "Ф"},
            {"S", "Ы"},
            {"D", "В"},
            {"F", "А"},
            {"G", "П"},
            {"H", "Р"},
            {"J", "О"},
            {"K", "Л"},
            {"L", "Д"},
            {"Z", "Я"},
            {"X", "Ч"},
            {"C", "С"},
            {"V", "М"},
            {"B", "И"},
            {"N", "Т"},
            {"M", "Ь"},
        };

        private static readonly Dictionary<string, string> TransliterDictionaryKeyboardRuToEn = new Dictionary
            <string, string>
        {
            {"й", "q"},
            {"ц", "w"},
            {"у", "e"},
            {"к", "r"},
            {"е", "t"},
            {"н", "y"},
            {"г", "u"},
            {"ш", "i"},
            {"щ", "o"},
            {"з", "p"},
            {"х", "["},
            {"ъ", "]"},
            {"ф", "a"},
            {"ы", "s"},
            {"в", "d"},
            {"а", "f"},
            {"п", "g"},
            {"р", "h"},
            {"о", "j"},
            {"л", "k"},
            {"д", "l"},
            {"ж", ";"},
            {"э", "'"},
            {"я", "z"},
            {"ч", "x"},
            {"с", "c"},
            {"м", "v"},
            {"и", "b"},
            {"т", "n"},
            {"ь", "m"},
            {"б", ","},
            {"ю", "."},
            {".", "/"},
            {"Й", "Q"},
            {"Ц", "W"},
            {"У", "E"},
            {"К", "R"},
            {"Е", "T"},
            {"Н", "Y"},
            {"Г", "U"},
            {"Ш", "I"},
            {"Щ", "O"},
            {"З", "P"},
            {"Х", "["},
            {"Ъ", "]"},
            {"Ф", "A"},
            {"Ы", "S"},
            {"В", "D"},
            {"А", "F"},
            {"П", "G"},
            {"Р", "H"},
            {"О", "J"},
            {"Л", "K"},
            {"Д", "L"},
            {"Ж", ";"},
            {"Э", "'"},
            {"Я", "Z"},
            {"Ч", "X"},
            {"С", "C"},
            {"М", "V"},
            {"И", "B"},
            {"Т", "N"},
            {"Ь", "M"},
            {"Б", ","},
            {"Ю", "."}
        };
        #endregion

        private static string Translate(string sourceText, IReadOnlyDictionary<string, string> dictionary)
        {
            var ans = new StringBuilder();
            foreach (var t in sourceText)
            {
                ans.Append(dictionary.ContainsKey(t.ToString()) ? dictionary[t.ToString()] : t.ToString());
            }
            return ans.ToString();
        }

        public static string KeyboardRuToEn(string sourceText)
        {
            return Translate(sourceText, TransliterDictionaryKeyboardRuToEn);
        }

        public static string KeyboardEnToRu(string sourceText)
        {
            return Translate(sourceText, TransliterDictionaryKeyboardEnToRu);
        }

        public static string TranslitRuToEn(string sourceText)
        {
            return Translate(sourceText, TransliterDictionaryRuToEn);
        }

        public static string TranslitEnToRu(string sourceText)
        {
            var ans = new StringBuilder();
            var dictionary = TransliterDictionaryEnToRu;

            for (var i = 0; i < sourceText.Length; i++)
            {
                for (var j = 2; j > -1; j--)
                {
                    if (sourceText.Length >= i + j + 1)
                    {
                        var key = sourceText.Substring(i, j + 1).ToLower();

                        if (dictionary.ContainsKey(key))
                        {
                            ans.Append(dictionary[key]);
                            i += j;
                            break;
                        }
                    }

                    if (j == 0)
                    {
                        ans.Append(sourceText[i].ToString());
                    }
                }
            }

            return ans.ToString();
        }
    }
}
