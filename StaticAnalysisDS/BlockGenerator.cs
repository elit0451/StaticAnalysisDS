﻿using StaticAnalysisDS.Intreperters;
using System;
using System.Collections.Generic;

namespace StaticAnalysisDS
{
    internal static class BlockGenerator
    {
        internal static Queue<IBlock> Generate(Queue<string> commands, State state, IIntreperter intreperter)
        {
            Queue<IBlock> blocks = new Queue<IBlock>();

            while (commands.Count > 0)
            {
                string line = commands.Peek();

                if (line.StartsWith("IF"))
                    blocks.Enqueue(GetIfBlock(commands, state, intreperter));
                else if (line.StartsWith("WHILE"))
                    blocks.Enqueue(GetWhileBlock(commands, state, intreperter));
                else
                    blocks.Enqueue(GetGeneralBlock(commands, intreperter));
            }

            return blocks;
        }
        private static IBlock GetIfBlock(Queue<string> commands, State state, IIntreperter intreperter)
        {
            Queue<string> instructionsIf = new Queue<string>();
            Queue<string> instructionsElse = new Queue<string>();
            bool blockFinished = false;
            bool inElse = false;
            int bracketsCount = 0;

            while (commands.Count > 0 && blockFinished == false)
            {
                string line = commands.Peek();

                if (line.Contains('{'))
                    bracketsCount++;
                else if (line.Contains('}'))
                    bracketsCount--;

                if (inElse)
                    instructionsElse.Enqueue(commands.Dequeue());
                else
                    instructionsIf.Enqueue(commands.Dequeue());

                if (bracketsCount <= 0)
                {
                    if (inElse || !commands.Peek().Contains("ELSE"))
                        blockFinished = true;
                    else
                        inElse = true;
                }
            }

            IBlock ifBlock = new IfBlock(instructionsIf, instructionsElse, state, intreperter);

            return ifBlock;
        }
        private static IBlock GetWhileBlock(Queue<string> commands, State state, IIntreperter intreperter)
        {
            Queue<string> instructions = new Queue<string>();
            bool blockFinished = false;
            int bracketsCount = 0;

            while (commands.Count > 0 && blockFinished == false)
            {
                string line = commands.Peek();

                if (line.Contains('{'))
                    bracketsCount++;
                else if (line.Contains('}'))
                    bracketsCount--;

                instructions.Enqueue(commands.Dequeue());

                if (bracketsCount <= 0)
                    blockFinished = true;
            }

            IBlock whileBlock = new WhileBlock(instructions, state, intreperter);

            return whileBlock;
        }
        private static IBlock GetGeneralBlock(Queue<string> commands, IIntreperter intreperter)
        {
            Queue<string> instructions = new Queue<string>();
            bool blockFinished = false;

            while (commands.Count > 0 && blockFinished == false)
            {
                string line = commands.Peek();

                if (line.StartsWith("IF") || line.StartsWith("WHILE"))
                    blockFinished = true;
                else if (line.Contains('}') || line == string.Empty)
                    commands.Dequeue();
                else
                    instructions.Enqueue(commands.Dequeue());
            }

            IBlock genBlock = null;

            if (instructions.Count > 0)
                genBlock = new GeneralBlock(instructions, intreperter);

            return genBlock;
        }
    }
}