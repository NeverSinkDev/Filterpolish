using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish
{
    public class Comparison_Delegates
    {
        public bool ArithmeticDelegateLess(object iParam, object eParam)
        {
            return (int.Parse(iParam.ToString()) < int.Parse(eParam.ToString()));
        }

        public bool ArithmeticDelegateLEqual(object iParam, object eParam)
        {
            return (int.Parse(iParam.ToString()) <= int.Parse(eParam.ToString()));
        }

        public bool ArithmeticDelegateEqual(object iParam, object eParam)
        {
            return (int.Parse(iParam.ToString()) == int.Parse(eParam.ToString()));
        }

        public bool ArithmeticDelegateMore(object iParam, object eParam)
        {
            return (int.Parse(iParam.ToString()) > int.Parse(eParam.ToString()));
        }

        public bool ArithmeticDelegateEMore(object iParam, object eParam)
        {
            return (int.Parse(iParam.ToString()) >= int.Parse(eParam.ToString()));
        }

        public bool ContainsDelegate(object iParam, object eParam)
        {
            return (eParam.ToString().Contains(iParam.ToString()));
        }

        public string GetRarityInteger(string RarityString)
        {
            return Enum.GetName(typeof(Type.Raritiy), RarityString);
        }
    }

}
