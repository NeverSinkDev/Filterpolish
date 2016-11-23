using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace FilterPolish
{
    public class Line
    {
        static char[] _charsToTrim = { '*', '\t', '\'', ' ' };

        public string Raw = "";
        public string Rebuilt = "";
        public string TypeLine;
        public string TypeIdent;

        public string Intro = "";
        public string Identifier = "";
        public string Oper = "";
        public string Value = "";
        public string Outtro = "";
        public string Comment = "";

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

        /// <summary>
        /// Standard consturctor Takes a raw bunch of text and turns it into a line. Needs to be initialized with Init();
        /// </summary>
        /// <param name="text"></param>
        public Line(string text)
        {
            Raw = text;
        }

        /// <summary>
        /// Rebuilds the line from the gathered components. Setting the optional param to true enables, also replaces the Raw value with the rebuilt line.
        /// </summary>
        /// <param name="ApplyToRaw"></param>
        /// <returns></returns>
        public string RebuildLine(bool ApplyToRaw = false)
        {

            this.Rebuilt =
                    Intro +
                    Identifier + (Value != "" ? " " : "") +
                    Oper + (Oper != "" ? " " : "") +
                    Value + ((Comment != "" && (Identifier == "Show" || Identifier == "Hide")) ? " " : "") +
                    Outtro +
                    Comment;


            if (ApplyToRaw)
            {
                this.Raw = this.Rebuilt;
            }

            return this.Rebuilt;
        }

        /// <summary>
        /// Directly applies the component to the Raw 
        /// </summary>
        public void UpdateRaw()
        {
            this.Raw =
                    Intro +
                    string.Join(" ", Attributes) + ((Comment != "" && (Identifier == "Show" || Identifier == "Hide")) ? " " : "")
                    + Outtro + Comment;
        }

        /// <summary>
        /// Calculates the "priority" of the line. This is used in order to sort the lines in an entry
        /// </summary>
        /// <returns></returns>
        public int CalculateLinePriority()
        {
            this.LinePriority = Type.GetLinePriority(this);

            if (this.LinePriority <= 0)
            {
                this.TypeLine = "ERROR";
            }

            return this.LinePriority;
        }

        /// <summary>
        /// Rebuilds the line from the broken down components,  but alos adds debug information.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the rebuilt result is different from the initial input
        /// </summary>
        /// <returns></returns>
        public bool CompareRebuild()
        {
            return (Intro + Identifier + Oper + Value + Comment + Outtro) == Raw;
        }

        /// <summary>
        /// Checks the comments for tags. Those tags are latr used to generate subversions.
        /// </summary>
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
                                this.BuildTags.Add(Comment.Substring(Comment.IndexOf("%") + 1));
                            }
                            else
                            {
                                this.Tags.Add(s);
                            }
                        }
                    }
                    else if (Comment.Contains("%"))
                    {
                        this.BuildTags.Add(Comment.Substring(Comment.IndexOf("%") + 1));
                    }
                }
            }
        }

        /// <summary>
        /// Identifies the line. Breaks it into pieces. WHile this method works, it is very ugly and needs a real rework.
        /// </summary>
        /// <returns></returns>
        public string Identify()
        {
            this.Intro = "";
            this.Identifier = "";
            this.Outtro = "";
            this.Comment = "";
            if (this.Attributes != null) { this.Attributes.Clear(); }
            if (this.Values != null) { this.Values.Clear(); }
            if (SplitString != null) { SplitString.Clear(); }
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
                    this.Intro = "";
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
                    this.Intro = "";
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
                    this.Intro = "\t";
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
                    this.Intro = "\t";
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

        /// <summary>
        /// Tests if the liat is not empty or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(IEnumerable<T> source)
        {
            if (source == null)
                return false; // or throw an exception
            return source.Any();
        }

        /// <summary>
        /// Compares the line in the ENTRY to another one
        /// -1  : line is null
        /// 0   : line has different idents/operas/params
        /// 1   : line has same params, line: NEW, this: ?
        /// 2   : line has same params, comments are the same, no tags
        /// 3   : line has same params, line: STABLE, this: NEW
        /// 4   : line has same params, line: STABLE, this: STABLE
        /// </summary>
        public int CompareLine(Line line, string commentTag = "")
        {

            if (line == null)
            {
                return -1;
            }

            if (line.Identifier != this.Identifier || this.Oper != line.Oper)
            {
                return 0;
            }

            if (!this.Attributes.SequenceEqual(line.Attributes))
            {
                return 0;
            }

            // OLD COMPARISON METHOD, SEEMS TO IGNORE ORDER
            //var d1 = this.Attributes.Except(line.Attributes).ToList();
            //var d2 = line.Attributes.Except(this.Attributes).ToList();
            //if (d1.Any() || d2.Any())
            //{
            //   return 0;
            //}

            if (this.Comment != line.Comment)
            {
                // The comment in the line is uninteresting
                if (line.Comment == "")
                {
                    return 1;
                }

                if (commentTag != "" && line.CommentContains(commentTag))
                {
                    return 1;
                }

                if (commentTag != "" && this.CommentContains(commentTag) && !line.CommentContains(commentTag))
                {
                    return 3;
                }

                if (commentTag != "" && !this.CommentContains(commentTag) && !line.CommentContains(commentTag))
                {
                    return 4;
                }



            }
            return 2;

        }

        public bool CommentContains(string tag)
        {
            if (this.Comment != null && this.Comment.Contains(tag))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Splits the list into the list<string> this.SplitString, Used to break the line into pieces.
        /// Another fairly ugly recursive method, works surprisingly well though.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
                        this.SplitString.Add(text.Substring(0, n).Trim(_charsToTrim));
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

        /// <summary>
        /// Changes the attribute N to value V and applies the result to the raw text. 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="v"></param>
        public void ChangeValueAndApplyToRaw(int n, string v)
        {
            this.Attributes[n] = v;
            UpdateRaw();
        }

        public float GetARGB_Brightness() 
        {
            float f = Color.FromArgb(O, R, G, B).GetBrightness();
            return f;
        }

        public float GetARGB_Hue()
        {
            float f = Color.FromArgb(O, R, G, B).GetHue();
            return f;
        }
    }

}