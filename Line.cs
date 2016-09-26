using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FilterPolish
{
    public class Line
    {
        char[] _charsToTrim = { '*', '\t', '\'', ' ' };
        public string Raw = "";
        public string Rebuilt = "";
        public string TypeLine;
        public string TypeIdent;

        public string Intro = "";
        public string Identifier = "";
        public string Oper = "";
        public string Value = "";
        public string Comment = "";
        public string Outtro = "";

        public List<string> Tags = new List<string>();
        public List<string> BuildTags = new List<string>();

        public List<string> Attributes;

        public List<string> SplitString = new List<string>();
        public List<string> Values = new List<string>();
        public int FontSize;
        public int DropSound;
        public int DropVolume;
        public int R;
        public int G;
        public int B;
        public int O;

        public int LinePriority = -1;

        public int CountAttri;
        public bool Commentfound = false;

        public Line(string text)
        {
            Raw = text;
        }

        public string RebuildLine(bool ApplyToRaw = false)
        {
            
            this.Rebuilt =
                    Intro +
                    Identifier + (Value != "" ? " " : "") +
                    Oper + (Oper != "" ? " " : "") +
                    Value + ((Comment != "" && (Identifier == "Show" || Identifier == "Hide")) ? " " : "") +
                    Comment +
                    Outtro;

            if (ApplyToRaw)
            {
                this.Raw = this.Rebuilt;
            }

            return this.Rebuilt;
        }

        public void UpdateRaw()
        {
            this.Raw =
                    Intro +
                    string.Join(" ", Attributes) + ((Comment != "" && (Identifier == "Show" || Identifier == "Hide")) ? " " : "") +
                    Comment + 
                    Outtro;
        }

        public int CalculateLinePriority()
        {
            this.LinePriority = Type.GetLinePriority(this);

            if (this.LinePriority <= 0)
            {
                this.TypeLine = "ERROR";
            }

            return this.LinePriority;
        }

        public string RebuildLineTypeDebug()
        {
            this.RebuildLine();

            if (this.LinePriority < 0)
            {
                this.CalculateLinePriority();
            }

            return TypeLine +
                   ((this.TypeLine != "AttributeClass") && (this.TypeLine != "AttributeVisual") ? "\t\t\t\t" : "\t\t") +
                   LinePriority + "\t" +
                   this.Rebuilt;
        }

        public bool CompareRebuild()
        {
            return (Intro + Identifier + Oper + Value + Comment + Outtro) == Raw;
        }

        public void LookForCommentTags()
        {
            if (this.Identifier == "Show" || this.Identifier == "Hide")
            {
                if (this.Comment.Length >= 0)
                {
                    int founddolla = this.Comment.IndexOf("$");
                    if (founddolla >= 1)
                    {
                        string[] tags = Comment.Substring(founddolla + 1).Split(',');
                        foreach (string s in tags)
                        {
                            s.Trim();
                            if (s.Contains("%"))
                            {
                                this.BuildTags.Add(Comment.Substring(Comment.IndexOf("%")+1));
                            }
                            else
                            { 
                            this.Tags.Add(s);
                            }
                        }
                    }
                    else if (Comment.Contains("%"))
                    {
                        this.BuildTags.Add(Comment.Substring(Comment.IndexOf("%")+1));
                    }
                }
            }
        }

        public string Identify()
        {
            this.Intro = "";
            this.Identifier = "";
            this.Outtro = "";
            this.Comment = "";
            if (this.Attributes != null){ this.Attributes.Clear(); }
            if (this.Values != null) { this.Values.Clear(); }
            if (SplitString != null){SplitString.Clear(); }
            this.Value = "";
            this.Oper = "";

            string s = Raw;

            //
            // DETECT COMMENT 
            //
            if (s.Contains("#"))
            {
                Commentfound = true;
                string[] splitresult = s.Split('#');
                int n = s.IndexOf("#", StringComparison.Ordinal);
                Comment = s.Substring(n);
                s = s.Substring(0, n);
            }

            //
            // CHECK FOR ANY NON-COMMENT INFORMATION
            //

            Match m = Regex.Match(s, @"(\w+)[A-Za-z0-9]");
            int pos = 0;
            if (m.Success)
            {
                pos = m.Index;
            }
            else
            {
                if (s.Length > 0)
                {
                    Intro = s;
                }

                //
                // HANDLE COMMENT ONLY
                //

                if (Commentfound == true)
                {
                    TypeLine = Type.LineType.Comment.ToString();
                    return TypeLine;
                }

                //
                // HANDLE FILLER LINE
                //

                else
                {
                    TypeLine = Type.LineType.Filler.ToString();
                    Outtro = "";
                    Intro = "";
                    return TypeLine;
                }
            }

            //
            // DETECT INTRO
            //

            Intro = s.Substring(0, pos);
            s = s.Substring(pos);

            //
            // SPLIT ANY SCRIPT INTO STRINGS
            // optimizations possible, but not requiered due to the managable file size

            SplitToList(s);
            Attributes = new List<string>(SplitString);
            CountAttri = Attributes.Count();

            //
            // HANDLE SHOW
            //

            if (IsNotEmpty(Attributes))
            {
                if (Attributes[0].Equals("Show"))
                {
                    TypeLine = Type.LineType.Show.ToString();
                    Identifier = "Show";

                    if (Comment.Contains("$") || (Comment.Contains("%")))
                    {
                        this.LookForCommentTags();
                    }

                    return TypeLine;
                }

                //
                // HANDLE HIDE
                //

                else if (Attributes[0].Equals("Hide"))
                {
                    TypeLine = Type.LineType.Hide.ToString();
                    Identifier = "Hide";

                    if (Comment.Contains("$") || (Comment.Contains("%")))
                    {
                        this.LookForCommentTags();
                    }

                    return TypeLine;
                }

                //
                // HANDLE ATTRIBUTES
                //

                if (!(Type.Match_Attribute_Class(Attributes[0]).Equals("Unknown")))
                {
                    TypeLine = Type.LineType.AttributeClass.ToString();
                    Identifier = Attributes[0];

                    //
                    // HANDLE OPERATORS
                    //

                    if (Type.Match_Operator(Attributes[0]) == true)
                    {
                        Oper = Attributes[1];
                        Values = Attributes.GetRange(2, CountAttri - 1).ToList();
                        Value = string.Join(" ", Values);
                        return TypeLine;

                        //
                        // HANDLE NON-OPERATORS
                        //

                    }
                    else
                    {
                        Values = Attributes.GetRange(1, CountAttri - 1).ToList();
                        Value = string.Join(" ", Values);
                        return TypeLine;
                    }
                }

                //
                // HANDLE VISUAL
                //

                else if (!(Type.Match_Attribute_Visual(Attributes[0]).Equals("Unknown")))
                {
                    TypeLine = Type.LineType.AttributeVisual.ToString();
                    Identifier = Attributes[0];

                    // RGBO
                    if (CountAttri == 5)
                    {
                        R = int.Parse(Attributes[1]);
                        G = int.Parse(Attributes[2]);
                        B = int.Parse(Attributes[3]);
                        O = int.Parse(Attributes[4]);
                        Value = R + " " + G + " " + B + " " + O;
                        return TypeLine;
                    }
                    if (CountAttri == 4)
                    {
                        R = int.Parse(Attributes[1]);
                        G = int.Parse(Attributes[2]);
                        B = int.Parse(Attributes[3]);
                        Value = R + " " + G + " " + B;
                        return TypeLine;
                    }

                    //RGB

                    //SOUND
                    if (!(Identifier.Equals("PlayAlertSound") & (CountAttri == 3)))
                    {
                        if (!(Identifier.Equals("PlayAlertSound") & (CountAttri == 2)))
                        {
                            if (!Identifier.Equals("SetFontSize")) return ("ERROR");
                            FontSize = int.Parse(Attributes[1]);
                            Value = FontSize.ToString();
                            return TypeLine;
                        }

                        //FONTSIZE
                        DropSound = int.Parse(Attributes[1]);
                        Value = DropSound.ToString();
                        return TypeLine;
                    }
                    else
                    {
                        DropSound = int.Parse(Attributes[1]);
                        DropVolume = int.Parse(Attributes[2]);
                        Value = DropSound + " " + DropVolume;
                        return TypeLine;
                    }

                    //SOUND DEFAULT VOLUME
                }
            }

            //UNHANDLED INFORMATION
            return ("ERROR");

        }

        public static bool IsNotEmpty<T>(IEnumerable<T> source)
        {
            if (source == null)
                return false; // or throw an exception
            return source.Any();
        }

        public static bool IsAtLeastThisLong<T>(IEnumerable<T> source, int index)
        {
            if (source != null)
            {
                if (source.Count() >= index)
                    return true;
            }

            return false;
        }

        public void HandleCommentTags()
        {
            
        }

        public void SetAttributes(List<string> change)
        {
            if (this.TypeLine == "Show" || this.TypeLine == "Hide")
            {
                
            }
            else if (this.TypeLine == "AttributeClass")
            {

            }
            else if (this.TypeLine == "AttributeVisual")
            {
                if (this.Identifier == "SetBorderColor" || this.Identifier == "SetTextColor" ||
                    this.Identifier == "SetBackgroundColor")
                {
                    
                }  
            }
        }

        public void AddAttribute()
        {
            
        }

        public bool SplitToList(string text)
        {

            int n = 0;
            if (text.Length > 0)
            {
                if (text.Contains(" "))
                {
                    n = text.IndexOf(" ");
                    if (n > 0)
                    {
                        SplitString.Add(text.Substring(0, n).Trim(_charsToTrim));
                        SplitToList(text.Substring(n));

                    }
                    else
                    {
                        SplitToList(text.Substring(1));
                    }

                }
                else
                {
                    SplitString.Add(text.Substring(0).Trim(_charsToTrim));
                }
            }
            return false;
        }

        public void ChangeValueAndApplyToRaw(int n, string v)
        {
            this.Attributes[n] = v;
            UpdateRaw();
        }
    }

}