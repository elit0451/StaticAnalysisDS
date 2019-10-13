using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StaticAnalysisDS
{
    public class VSSLInterpreter
    {
        private Queue<string> commands;
        private StateMachine stateMachine;
        private bool skipCurrentBlock;
        private int skipElse;
        private int currentBlockBracketCount;

        public VSSLInterpreter(string filePath) 
        {
            commands = new Queue<string>();
            stateMachine = new StateMachine();
            skipCurrentBlock = false;
            skipElse = 0;
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
                    VarDefinition(nextCommand.Substring(nextCommand.IndexOf("DEF") + 4));
                    break;
                case "LET":
                    SetVarValue(nextCommand.Substring(nextCommand.IndexOf("LET") + 4));
                    break;
                case "IF":
                    IfStatement(nextCommand.Substring(nextCommand.IndexOf("IF") + 4, nextCommand.IndexOf(')') - 4));
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

        private void VarDefinition(string command)
        {
            string[] syntax = command.Split(' ');
            string varName = syntax[0].Substring(0, syntax[0].IndexOf(':'));
            string varType = syntax[1];

            stateMachine.DefineVar(varName, varType);
        }

        private void SetVarValue(string command)
        {
            string[] syntax = command.Split(' ');
            string varName = syntax[0];
            string value = "";

            if (syntax.Length > 3)
            {
                string operand1 = syntax[2];
                string operation = syntax[3];
                string operand2 = syntax[4];

                stateMachine.SetVar(varName, operand1, operand2, operation);
            }
            else
            {
                value = syntax[2];

                stateMachine.SetVar(varName, value);
            }
        }
        private void IfStatement(string predicate)
        {
            string[] syntax = predicate.Split(' ');
            string operand1 = syntax[0];
            string operation = syntax[1];
            string operand2 = syntax[2];

            bool result = stateMachine.EvaluateIf(operand1, operand2, operation);

            if (result == true)
                skipElse++;
            else
            {
                skipCurrentBlock = true;
                currentBlockBracketCount = 1;
            }
        }
    }
}
