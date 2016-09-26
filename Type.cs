using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public static class Type
    {
        public enum EntryType
        {
            Unknown = 0,
            Show = 1,
            Hide = 2,
            Comment = 3,
            Filler = 4,  
        }

        public enum LineType
        {
            Unknown = 0,
            Show = 1,
            Hide = 2,
            AttributeClass = 3,
            AttributeVisual = 4,
            Comment = 5,
            Filler = 6,
            Disabled = 7
        }

        public enum LineAttributeClass
        {
            Unknown = 0,
            Class = 1,
            BaseType = 2,
            Rarity = 3,
            DropLevel = 4,
            ItemLevel = 5,
            Sockets = 6,
            LinkedSockets = 7,
            SocketGroup = 8,
            Quality = 9,
            Width = 10,
            Height = 11,
            Identified = 12
        }

        public enum LineAttributeVisual
        {
            Unknown = 0,
            SetTextColor = 1,
            SetBorderColor = 2,
            SetBackgroundColor = 3,
            SetFontSize = 4,
            PlayAlertSound = 5
        }

        public static bool Match_Operator(string s)
        {
            string[] ope = { "=", ">", ">=", "<", "<=" };
            int i = ope.Length;
            for (int n = 0; n < i; n++)
            {
                if (s.Equals(ope[n]))
                {
                    return true;
                }
            }
            return false;
        }

        public static string Match_Attribute_Class(string s)
        {
            int i = Enum.GetNames(typeof(LineAttributeClass)).Length;
            for (int n=0; n<i; n++)
            {
                if (s.Equals(((LineAttributeClass)n).ToString()))
                {
                    return s;
                }
            }
            return "Unknown";
        }

        public static string Match_Attribute_Visual(string s)
        {
            int i = Enum.GetNames(typeof(LineAttributeVisual)).Length;
            for (int n = 0; n < i; n++)
            {
                if (s.Equals(((LineAttributeVisual)n).ToString()))
                {
                    return s;
                }
            }
            return "Unknown";
        }


        public static int GetLinePriority(Line line)
        {
            if (line.TypeLine == "Show")
            {
                return 11; }
            if (line.TypeLine == "Hide")
            {
                return 12; }
            if (line.TypeLine == "AttributeClass")
            {
                return 20 + GetEnumC(line.Identifier); }
            if (line.TypeLine == "AttributeVisual")
            {
                return 40 + GetEnumV(line.Identifier); }
            if (line.TypeLine == "Comment")
            {
                return 05; }
            if (line.TypeLine == "Filler")
            {
                return 80; }
            return -1;
        }

        public static int GetEnumC(string s)
        {
            int found = 0;
            int i = Enum.GetNames(typeof(LineAttributeClass)).Length;
            for (int n = 0; n < i; n++)
            {
                if (s.Equals(((LineAttributeClass)n).ToString()))
                {
                    return n;
                }
            }
            return -1000;
        }

        public static int GetEnumV(string s)
        {
            int found = 0;
            int i = Enum.GetNames(typeof(LineAttributeVisual)).Length;
            for (int n = 0; n < i; n++)
            {
                if (s.Equals(((LineAttributeVisual)n).ToString()))
                {
                    return n;
                }
            }
            return -1000;
        }

    }
}
