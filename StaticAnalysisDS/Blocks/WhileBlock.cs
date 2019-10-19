using StaticAnalysisDS.Intreperters;
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
        private IIntreperter _intreperter;

        public WhileBlock(Queue<string> commands, State state, IIntreperter intreperter)
        {
            _intreperter = intreperter;
            _predicate = commands.Dequeue();
            _commands = new Queue<string>(commands);

            _blocks = BlockGenerator.Generate(commands, state, intreperter).ToArray();
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
                _predicateMet = _intreperter.EvaluatePrecondition(_predicate);
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
                    if (_intreperter.EvaluatePrecondition(_predicate) == false)
                        _isFinished = true;
                    else
                    {
                        _blockIndex = 0;
                        Queue<string> q = new Queue<string>(_commands);
                        _blocks = BlockGenerator.Generate(q, _state, _intreperter).ToArray();
                    }
                }
            }
        }

        public bool IsFinished()
        {
            return _isFinished || _predicateMet == false;
        }
    }
}