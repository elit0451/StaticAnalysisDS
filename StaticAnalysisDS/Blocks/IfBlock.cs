using StaticAnalysisDS.Intreperters;
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
        private string _predicate;
        private IIntreperter _intreperter;

        public IfBlock(Queue<string> commandsIf, Queue<string> commandsElse, State state, IIntreperter intreperter)
        {
            _intreperter = intreperter;
            _predicate = commandsIf.Dequeue();
            // dequeue first line 
            if (commandsElse.Count > 0)
                commandsElse.Dequeue();

            _blocksIf = BlockGenerator.Generate(commandsIf, state, intreperter);
            _blocksElse = BlockGenerator.Generate(commandsElse, state, intreperter);
        }

        public void NextStep()
        {
            if (_inIf is null)
            {
                _inIf = _intreperter.EvaluateIf(_predicate);
                Console.WriteLine(_predicate + "\t" + _inIf);
            }

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
                _currentBlock = null;
        }
        public bool IsFinished()
        {
            if (_inIf == true)
                return _blocksIf.Count == 0 && _currentBlock is null;
            else
                return _blocksElse.Count == 0 && _currentBlock is null;
        }
    }
}