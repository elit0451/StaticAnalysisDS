using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAnalysisDS
{
    public class State
    {
        public Dictionary<string, HashSet<bool>> BooleanVariables { get; set; }
        public Dictionary<string, HashSet<int>> IntegerVariables { get; set; }
        public State()
        {
            BooleanVariables = new Dictionary<string, HashSet<bool>>();
            IntegerVariables = new Dictionary<string, HashSet<int>>();
        }

        internal bool IsVarBoolean(string varName)
        {
            return BooleanVariables.ContainsKey(varName);
        }

        internal void SetBoolean(string varName, bool value)
        {
            if (BooleanVariables.ContainsKey(varName))
                BooleanVariables[varName].Add(value);
        }

        internal void SetInteger(string varName, int value)
        {
            if (IntegerVariables.ContainsKey(varName))
                IntegerVariables[varName].Add(value);
            Console.WriteLine(value);
        }
    }
}
