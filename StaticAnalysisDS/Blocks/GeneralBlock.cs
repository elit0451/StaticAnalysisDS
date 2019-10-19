using StaticAnalysisDS.Intreperters;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StaticAnalysisDS
{
    internal class GeneralBlock : IBlock
    {
        private Queue<string> _commands;
        private Regex _regex;
        private IIntreperter _intreperter;

        public GeneralBlock(Queue<string> commands, IIntreperter intreperter)
        {
            _commands = commands;
            _intreperter = intreperter;
        }

        public void NextStep()
        {
            if (_commands.Count <= 0)
                return;

            string nextCommand = _commands.Dequeue();
            Console.WriteLine(nextCommand);

            switch (nextCommand.Split(' ')[0])
            {
                case "DEF":
                    _intreperter.DefineVar(nextCommand.Substring(nextCommand.IndexOf("DEF") + 4));
                    break;
                case "LET":
                    _intreperter.SetVar(nextCommand.Substring(nextCommand.IndexOf("LET") + 4));
                    break;
            }
        }
        public bool IsFinished()
        {
            return _commands.Count <= 0;
        }
    }
}