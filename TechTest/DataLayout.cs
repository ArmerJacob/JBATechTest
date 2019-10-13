using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest
{
    class DataLayout
    {
        List<int> Xref;
        List<int> Yref;
        List<string> Date;
        List<int> Value;

        public DataLayout()
        {
            Xref = new List<int>();
            Yref = new List<int>();
            Date = new List<string>();
            Value = new List<int>();
        }

        public void AddtoXref(int pInput)
        {
            Xref.Add(pInput);
        }

        public void AddtoYref(int pInput)
        {
            Yref.Add(pInput);
        }

        public void AddtoDate(string pInput)
        {
            Date.Add(pInput);
        }

        public void AddtoValue(int pInput)
        {
            Value.Add(pInput);
        }

        public List<int> GetXref()
        {
            return Xref;
        }

        public List<int> GetYref()
        {
            return Yref;
        }

        public List<string> GetDate()
        {
            return Date;
        }

        public List<int> GetValue()
        {
            return Value;
        }
    }
}
