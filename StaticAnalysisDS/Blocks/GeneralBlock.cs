using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    internal class GeneralBlock : IBlock
    {
        private Queue<string> _commands;
        private State _state;
        private Regex _regex;
        private int _totalLines;
        private int _usedLines;

        public GeneralBlock(Queue<string> commands, State state)
        {
            _totalLines = commands.Count;
            _usedLines = 0;
            _commands = commands;
            _state = state;
        }

        public void NextStep()
        {
            if (_commands.Count <= 0)
                return;

            string nextCommand = _commands.Dequeue();
            _usedLines++;

            if (nextCommand.Contains('}') || nextCommand == string.Empty)
                return;

            switch (nextCommand.Split(' ')[0])
            {
                case "DEF":
                    DefineVar(nextCommand.Substring(nextCommand.IndexOf("DEF") + 4));
                    break;
                case "LET":
                    SetVar(nextCommand.Substring(nextCommand.IndexOf("LET") + 4));
                    break;
            }
        }
        public bool IsFinished()
        {
            return _commands.Count <= 0;
        }

        private void DefineVar(string command)
        {
            _regex = new Regex(@"(\w+):\s+(\w+)");
            Match match = _regex.Match(command);
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
        private void SetVar(string command)
        {
            string value = "";

            _regex = new Regex(@"(\w+)\s+=\s(.*)");
            Match match = _regex.Match(command);

            if (match.Success)
            {
                string varName = match.Groups[1].Value;
                string expression = match.Groups[2].Value;

                if (expression.Contains(' '))
                {
                    _regex = new Regex(@"(\w+)\s+(.)\s(\w+)");
                    Match matchOperands = _regex.Match(expression);
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
        private int CalculateIntOperation(string operand1, string operand2, string operation)
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

        private bool CalculateBoolOperation(string operand1, string operand2, string operation)
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
        private bool IsVariable(string varName)
        {
            return _state.VarExists(varName);
        }

        public int CurrentLine()
        {
            return _usedLines;
        }

        public int TotalLines()
        {
            return _totalLines;
        }
    }
}