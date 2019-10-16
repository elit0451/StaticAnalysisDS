using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS.Intreperters
{
    class VSSLIntreperter : IIntreperter
    {
        private State _state;

        public VSSLIntreperter(State state)
        {
            _state = state;
        }

        public bool EvaluatePrecondition(string predicate)
        {
            Regex regex = new Regex(@"\((\w+)\s+(.+)\s(\w+)\)");
            Match match = regex.Match(predicate);
            bool result = false;

            if (match.Success)
            {
                string operand1 = match.Groups[1].Value;
                string operation = match.Groups[2].Value;
                string operand2 = match.Groups[3].Value;

                if (IsVariable(operand1) && _state.IsVarBoolean(operand1))
                    operand1 = _state.GetBooleanValue(operand1).ToString();
                else if (IsVariable(operand1))
                    operand1 = _state.GetIntegerValue(operand1).ToString();

                if (IsVariable(operand2) && _state.IsVarBoolean(operand2))
                    operand2 = _state.GetBooleanValue(operand2).ToString();
                else if (IsVariable(operand2))
                    operand2 = _state.GetIntegerValue(operand2).ToString();

                result = CalculateBoolOperation(operand1, operand2, operation);
            }

            return result;
        }

        public bool CalculateBoolOperation(string operand1, string operand2, string operation)
        {
            int opInt1 = 0, opInt2 = 0;
            bool opBool1 = false, opBool2 = false;
            try
            {
                opInt1 = int.Parse(operand1);
                opInt2 = int.Parse(operand2);
            }
            catch (Exception e)
            {
                opBool1 = bool.Parse(operand1);
                opBool2 = bool.Parse(operand2);
            }

            switch (operation)
            {
                case "AND": return opBool1 && opBool2;
                case "OR": return opBool1 || opBool2;
                // TODO: case "NOT":
                case "<": return opInt1 < opInt2;
                case "<=": return opInt1 <= opInt2;
                case "==": return opInt1 == opInt2;
                case ">=": return opInt1 >= opInt2;
                case ">": return opInt1 > opInt2;
                default: throw new Exception("Illegal syntax!");
            }
        }

        public bool IsVariable(string varName)
        {
            return _state.VarExists(varName);
        }

        public bool EvaluateIf(string predicate)
        {
            Regex regex = new Regex(@"\((\w+)\s+(.+)\s(\w+)\)");
            Match match = regex.Match(predicate);
            bool result = false;

            if (match.Success)
            {
                string operand1 = match.Groups[1].Value;
                string operation = match.Groups[2].Value;
                string operand2 = match.Groups[3].Value;

                if (IsVariable(operand1) && _state.IsVarBoolean(operand1))
                    operand1 = _state.GetBooleanValue(operand1).ToString();
                else if (IsVariable(operand1))
                    operand1 = _state.GetIntegerValue(operand1).ToString();

                if (IsVariable(operand2) && _state.IsVarBoolean(operand2))
                    operand2 = _state.GetBooleanValue(operand2).ToString();
                else if (IsVariable(operand2))
                    operand2 = _state.GetIntegerValue(operand2).ToString();

                result = CalculateBoolOperation(operand1, operand2, operation);
            }

            return result;
        }

        public void DefineVar(string command)
        {
            Regex regex = new Regex(@"(\w+):\s+(\w+)");
            Match match = regex.Match(command);
            if (match.Success)
            {
                string varName = match.Groups[1].Value;
                string varType = match.Groups[2].Value;

                if (varType == "INTEGER")
                    _state.DefineIntegerVar(varName);
                else
                    _state.DefineBooleanVar(varName);
            }
        }

        public void SetVar(string command)
        {
            string value = "";

            Regex regex = new Regex(@"(\w+)\s+=\s(.*)");
            Match match = regex.Match(command);

            if (match.Success)
            {
                string varName = match.Groups[1].Value;
                string expression = match.Groups[2].Value;

                if (expression.Contains(' '))
                {
                    regex = new Regex(@"(\w+)\s+(.)\s(\w+)");
                    Match matchOperands = regex.Match(expression);
                    if (matchOperands.Success)
                    {
                        string operand1 = matchOperands.Groups[1].Value;
                        string operation = matchOperands.Groups[2].Value;
                        string operand2 = matchOperands.Groups[3].Value;

                        if (IsVariable(operand1) && _state.IsVarBoolean(operand1))
                            operand1 = _state.GetBooleanValue(operand1).ToString();
                        else if (IsVariable(operand1))
                            operand1 = _state.GetIntegerValue(operand1).ToString();

                        if (IsVariable(operand2) && _state.IsVarBoolean(operand2))
                            operand2 = _state.GetBooleanValue(operand2).ToString();
                        else if (IsVariable(operand2))
                            operand2 = _state.GetIntegerValue(operand2).ToString();

                        if (_state.IsVarBoolean(varName))
                            value = CalculateBoolOperation(operand1, operand2, operation).ToString();
                        else
                            value = CalculateIntOperation(operand1, operand2, operation).ToString();
                    }
                }
                else
                {
                    value = expression;
                }

                if (_state.IsVarBoolean(varName))
                    _state.SetBoolean(varName, bool.Parse(value));
                else
                    _state.SetInteger(varName, int.Parse(value));
            }
        }

        public int CalculateIntOperation(string operand1, string operand2, string operation)
        {
            int op1 = int.Parse(operand1);
            int op2 = int.Parse(operand2);

            switch (operation)
            {
                case "+": return op1 + op2;
                case "-": return op1 - op2;
                default: throw new Exception("Illegal syntax!");
            }
        }
    }
}
