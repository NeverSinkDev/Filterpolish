﻿using System;
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

        public enum Raritiy
        {
            Unknown = 0,
            Normal = 1,
            Magic = 2,
            Rare = 3,
            Unique = 4,
        }

        public enum LineAttributeClass
        {
            Unknown = 0,
            ShapedMap = 1,
            ElderItem = 2,
            ShaperItem = 3,
            Corrupted = 4,
            Identified = 5,
            ElderMap = 6,
            LinkedSockets = 7,
            Sockets = 8,
            Quality = 9,
            MapTier = 10,
            Width = 11,
            Height = 12,
            StackSize = 13,
            GemLevel = 14,
            ItemLevel = 15,
            DropLevel = 16,
            Rarity = 17,
            SocketGroup = 18,
            Class = 19,
            BaseType = 20,
            HasExplicitMod = 21,
            Prophecy = 22
        }

        public enum LineAttributeVisual
        {
            Unknown = 0,
            SetFontSize = 1,
            SetTextColor = 2,
            SetBorderColor = 3,
            SetBackgroundColor = 4,
            PlayAlertSound = 5,
            CustomAlertSound = 6,
            DisableDropSound = 7,
            MinimapIcon = 8,
            PlayEffect = 9
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
                return 50 + GetEnumV(line.Identifier); }
            if (line.TypeLine == "Comment")
            {
                return 05; }
            if (line.TypeLine == "Filler")
            {
                return 90; }
            return -1;
        }

        public static int GetEnumC(string s)
        {
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
