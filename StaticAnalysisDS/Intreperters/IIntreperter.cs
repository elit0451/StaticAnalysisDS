using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAnalysisDS.Intreperters
{
    public interface IIntreperter
    {
        bool EvaluatePrecondition(string predicate);

        bool CalculateBoolOperation(string operand1, string operand2, string operation);

        bool IsVariable(string varName);

        bool EvaluateIf(string predicate);

        void DefineVar(string command);

        void SetVar(string command);

        int CalculateIntOperation(string operand1, string operand2, string operation);
    }
}
