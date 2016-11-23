using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class FilterSettings
    {
        public string subVersionName;
        public string versionNumber;
        public int strictLevel = 0;

        public FilterSettings(string name, string versionnumber, int strictness)
        {
            this.subVersionName = name;
            this.versionNumber = versionnumber;
            this.strictLevel = strictness;
        }

        public FilterSettings()
        {
            this.subVersionName = "unnamed";
            this.versionNumber = "0.1";
            this.strictLevel = 0;
        }
    }
}
