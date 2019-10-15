using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAnalysisDS
{
    public class State
    {
        public Dictionary<string, bool?> BooleanVariables { get; set; }
        public Dictionary<string, int?> IntegerVariables { get; set; }
        public State()
        {
            BooleanVariables = new Dictionary<string, bool?>();
            IntegerVariables = new Dictionary<string, int?>();
        }

        internal bool IsVarBoolean(string varName)
        {
            return BooleanVariables.ContainsKey(varName);
        }

        internal void SetBoolean(string varName, bool v)
        {
            if (BooleanVariables.ContainsKey(varName))
                BooleanVariables[varName] = v;
        }

        internal void SetInteger(string varName, int v)
        {
            if (IntegerVariables.ContainsKey(varName))
                IntegerVariables[varName] = v;
        }

        internal void DefineIntegerVar(string varName)
        {
            IntegerVariables.Add(varName, null);
        }

        internal void DefineBooleanVar(string varName)
        {
            BooleanVariables.Add(varName, null);
        }

        internal int? GetIntegerValue(string operand1)
        {
            if (IntegerVariables.ContainsKey(operand1))
                return IntegerVariables[operand1];

            throw new Exception("Item not present in the state");
        }

        internal bool? GetBooleanValue(string operand1)
        {
            if (BooleanVariables.ContainsKey(operand1))
                return BooleanVariables[operand1];

            throw new Exception("Item not present in the state");
        }

        internal bool VarExists(string varName)
        {
            return BooleanVariables.ContainsKey(varName) || IntegerVariables.ContainsKey(varName);
        }
        internal Dictionary<string, int?> GetIntegers()
        {
            return IntegerVariables;
        }
        internal Dictionary<string, bool?> GetBooleans()
        {
            return BooleanVariables;
        }

    }
}
