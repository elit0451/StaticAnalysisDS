using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StaticAnalysisDS
{
    public class StateMachine
    {
        private State currentState;

        public StateMachine()
        {
            currentState = new State();
        }
        public void DefineVar(string varName, string varType)
        {
            //if (varType == "INTEGER")
            //call currentState.SetInteger(varName, );
            //else
            //call currentState.SetBoolean(varName, );
        }
        public void SetVar(string varName, string operand1, string operand2 = "", string operation = "")
        {
            string resultValue = "";

            if (operand2 != "" && operation != "")
            {
                if (currentState.IsVarBoolean(varName))
                    resultValue = CalculateBoolOperation(operand1, operand2, operation).ToString();
                else
                    resultValue = CalculateIntOperation(operand1, operand2, operation).ToString();
            }
            else
            {
                resultValue = operand1;
            }

            if (currentState.IsVarBoolean(varName))
                currentState.SetBoolean(varName, bool.Parse(resultValue));
            else
                currentState.SetInteger(varName, int.Parse(resultValue));
        }

        public bool EvaluateIf(string operand1, string operand2, string operation)
        {
            bool result = CalculateBoolOperation(operand1, operand2, operation);

            return result;
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
