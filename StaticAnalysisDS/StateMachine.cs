using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    public class StateMachine
    {
        private State currentState;
        private Queue<string> commands;
        private int skipElse;
        private bool skipCurrentBlock;
        private int currentBlockBracketCount;
        private Regex regex;

        public StateMachine(string filePath)
        {
            currentState = new State();
            commands = new Queue<string>();
            skipElse = 0;
            skipCurrentBlock = false;
            currentBlockBracketCount = 0;

            string line = "";

            using (StreamReader reader = new StreamReader(filePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    commands.Enqueue(line.ToUpper().Trim());
                }
            }
        }

        public void NextStep()
        {
            string nextCommand = commands.Dequeue();

            if (skipCurrentBlock)
            {
                if (nextCommand.Contains('{'))
                    currentBlockBracketCount++;
                else if (nextCommand.Contains('}'))
                {
                    currentBlockBracketCount--;
                    if (currentBlockBracketCount == 0)
                        skipCurrentBlock = false;
                }
                return;
            }

            switch (nextCommand.Split(' ')[0])
            {
                case "DEF":
                    DefineVar(nextCommand.Substring(nextCommand.IndexOf("DEF") + 4));
                    break;
                case "LET":
                    SetVar(nextCommand.Substring(nextCommand.IndexOf("LET") + 4));
                    break;
                case "IF":
                    EvaluateIf(nextCommand.Substring(nextCommand.IndexOf("IF") + 4, nextCommand.IndexOf(')') - 4));
                    break;
                case "ELSE":
                    if (skipElse > 0)
                    {
                        skipElse--;
                        skipCurrentBlock = true;
                        currentBlockBracketCount = 1;
                    }
                    break;
                case "WHILE":
                    break;
            }
        }

        private void DefineVar(string command)
        {
            regex = new Regex(@"(\w+):\s+(\w+)");
            Match match = regex.Match(command);
            if (match.Success)
            {
                string varName = match.Groups[1].Value;
                string varType = match.Groups[2].Value;

                if (varType == "INTEGER")
                    currentState.DefineIntegerVar(varName);
                else
                    currentState.DefineBooleanVar(varName);
            }
        }
        private void SetVar(string command)
        {
            string value = "";

            regex = new Regex(@"(\w+)\s+=\s(.*)");
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

                        if (currentState.IsVarBoolean(varName))
                            value = CalculateBoolOperation(operand1, operand2, operation).ToString();
                        else
                            value = CalculateIntOperation(operand1, operand2, operation).ToString();
                    }
                }
                else
                {
                    value = expression;
                }

                if (currentState.IsVarBoolean(varName))
                    currentState.SetBoolean(varName, bool.Parse(value));
                else
                    currentState.SetInteger(varName, int.Parse(value));
            }
        }

        private void EvaluateIf(string predicate)
        {
            regex = new Regex(@"(\w+)\s+(.+)\s(\w+)");
            Match match = regex.Match(predicate);

            if (match.Success)
            {
                string operand1 = match.Groups[1].Value;
                string operation = match.Groups[2].Value;
                string operand2 = match.Groups[3].Value;

                bool result = CalculateBoolOperation(operand1, operand2, operation);

                if (result == true)
                    skipElse++;
                else
                {
                    skipCurrentBlock = true;
                    currentBlockBracketCount = 1;
                }

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
    }
}
