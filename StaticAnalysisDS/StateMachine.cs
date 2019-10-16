using StaticAnalysisDS.Intreperters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    public class StateMachine
    {
        private Queue<IBlock> _blocks;
        private IBlock _currentBlock;
        private State _state;
        private int _totalLines;

        public StateMachine(string filePath)
        {
            Queue<string> commands = new Queue<string>();
            string line = "";
            _state = new State();
            _totalLines = 0;

            IIntreperter intreperter = new VSSLIntreperter(_state);

            using (StreamReader reader = new StreamReader(filePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    commands.Enqueue(line.ToUpper().Trim());
                }
            }

            _blocks = BlockGenerator.Generate(commands, _state, intreperter);
            _currentBlock = null;
        }

        public void NextStep()
        {
            if (_blocks.Count <= 0 && _currentBlock is null)
                throw new Exception("Program finished");

            if (_currentBlock is null)
                _currentBlock = _blocks.Dequeue();
            _currentBlock.NextStep();

            if (_currentBlock.IsFinished())
                _currentBlock = null;
        }

        public void PrintCurrentState()
        {
            Dictionary<string, int?> integers = _state.GetIntegers();
            Dictionary<string, bool?> booleans = _state.GetBooleans();

            Console.WriteLine("NAME\tVALUE");
            foreach (KeyValuePair<string, int?> ints in integers)
                Console.WriteLine(ints.Key + "\t" + ints.Value);
            foreach (KeyValuePair<string, bool?> bools in booleans)
                Console.WriteLine(bools.Key + "\t" + bools.Value);
        }
    }
}
