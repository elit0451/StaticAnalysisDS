using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    internal class WhileBlock : IBlock
    {
        private IBlock[] _blocks;
        private string _predicate;
        private State _state;
        private int _blockIndex;
        private IBlock _currentBlock;
        private bool _isFinished;
        private Queue<string> _commands;
        private bool? _predicateMet;

        public WhileBlock(Queue<string> commands, State state)
        {
            _predicate = commands.Dequeue();
            _commands = new Queue<string>(commands);

            _blocks = BlockGenerator.Generate(commands, state).ToArray();
            _state = state;
            _blockIndex = 0;
            _currentBlock = null;
            _isFinished = false;
            _predicateMet = null;
        }

        public void NextStep()
        {
            if (_predicateMet is null)
            {
                _predicateMet = EvaluatePrecondition(_predicate);
                Console.WriteLine(_predicate + "\t" + _predicateMet);
            }

            if (_isFinished || _predicateMet == false)
                return;

            if (_currentBlock is null)
                _currentBlock = _blocks[_blockIndex];

            _currentBlock.NextStep();

            if (_currentBlock.IsFinished())
            {
                _currentBlock = null;
                _blockIndex++;

                if(_blockIndex == _blocks.Length)
                {
                    if (EvaluatePrecondition(_predicate) == false)
                        _isFinished = true;
                    else
                    {
                        _blockIndex = 0;
                        Queue<string> q = new Queue<string>(_commands);
                        _blocks = BlockGenerator.Generate(q, _state).ToArray();
                    }
                }
            }
        }

        public bool IsFinished()
        {
            return _isFinished || _predicateMet == false;
        }
        private bool EvaluatePrecondition(string predicate)
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
    }
}