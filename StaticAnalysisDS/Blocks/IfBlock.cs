using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    internal class IfBlock : IBlock
    {
        private Queue<IBlock> _blocksIf;
        private Queue<IBlock> _blocksElse;
        private bool? _inIf;
        private IBlock _currentBlock;
        private State _state;
        private string _predicate;
        private int _totalLinesIf;
        private int _totalLinesElse;
        private int _usedLines;

        public IfBlock(Queue<string> commandsIf, Queue<string> commandsElse, State state)
        {
            _totalLinesIf = commandsIf.Count;
            _totalLinesElse = commandsElse.Count;
            _state = state;
            _predicate = commandsIf.Dequeue();
            // dequeue first line 
            if (commandsElse.Count > 0)
                commandsElse.Dequeue();

            _usedLines = 1;

            _blocksIf = BlockGenerator.Generate(commandsIf, state);
            _blocksElse = BlockGenerator.Generate(commandsElse, state);

        }
        public void NextStep()
        {
            if (_inIf is null)
                _inIf = EvaluateIf(_predicate);

            if (_inIf == true)
            {
                if (_currentBlock is null)
                    _currentBlock = _blocksIf.Dequeue();
            }
            else
            {
                if (_currentBlock is null)
                    _currentBlock = _blocksElse.Dequeue();
            }

            _currentBlock.NextStep();

            if (_currentBlock.IsFinished())
            {
                _usedLines = _currentBlock.CurrentLine();
                _currentBlock = null;
            }
        }
        public bool IsFinished()
        {
            if (_inIf == true)
                return _blocksIf.Count == 0 && _currentBlock is null;
            else
                return _blocksElse.Count == 0 && _currentBlock is null;
        }

        private bool EvaluateIf(string predicate)
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
            int result = _usedLines;

            if (_inIf == false)
                result += _totalLinesIf;

            if (!(_currentBlock is null))
                result += _currentBlock.CurrentLine() + _usedLines;

            return result;
        }

        public int TotalLines()
        {
            if (_inIf == true)
                return _totalLinesIf;
            else
                return _totalLinesElse;
        }
    }
}