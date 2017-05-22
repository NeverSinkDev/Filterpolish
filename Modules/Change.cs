using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilterPolish.Modules
{
    public class Change
    {
        public string ident { get; set; }
        
        public string meta { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string UntieredTier { get; set; }

        public bool Changes
        {
            get
            {
                var oldV = this.Translate(OldValue);

                if (this.NewValue == null)
                {
                    return false;
                }

                var newV = this.Translate(NewValue);

                if (oldV != newV)
                {
                    return true;
                }

                return false;
            }
        }

        public Change(string ident, string value, string untieredTier = "")
        {
            this.ident = ident;
            this.OldValue = value;
            this.UntieredTier = untieredTier;
        }

        public string CompileChanges()
        {
            if (this.Changes)
            {
                var oldV = this.Translate(OldValue);
                var newV = this.Translate(NewValue);
                return "[**" + ident + "**]" +"|"+ oldV + "|" + newV;
            }
            else
            {
                return "";
            }
        }

        public string CompileChangesArrows()
        {
            if (this.Changes)
            {
                var oldV = this.Translate(OldValue);
                var newV = this.Translate(NewValue);
                return "[**" + ident + "**] " + oldV + " --> " + newV;
            }
            else
            {
                return "";
            }
        }

        private string Translate(string t)
        {
            if (t == "" || t== null || t == "*untiered*")
            {
                if (this.UntieredTier != null && this.UntieredTier != "")
                {
                    return this.UntieredTier;
                }

                return "*untiered*";
            }

            if (t.Contains("Uniques-Fated"))
            {
                return "Fated";
            }


            var str = Regex.Match(t, @"[T]{1}\d{1}").Value;

            if (str != null && str != "")
            {
                return t.Substring(t.IndexOf(str));
            }

            return t;
        }
    }
}
