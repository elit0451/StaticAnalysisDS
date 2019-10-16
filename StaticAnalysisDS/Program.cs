using StaticAnalysisDS.Intreperters;
using System;

namespace StaticAnalysisDS
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
            Console.ReadKey();
        }

        private void Run()
        {
            bool exit = true;
            string path = @"F:\School\DiscreteMath\StaticAnalysisDS\VSSL.txt";

            StateMachine stateMachine = new StateMachine(path);

            while (exit)
            {
                try
                {
                    Console.Clear();
                    stateMachine.NextStep();
                    stateMachine.PrintCurrentState();
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    exit = false;
                    Console.WriteLine("No more commands");
                }
            }


            ////stateMachine.PrintCurrentState();
            //stateMachine.NextStep();
            ////stateMachine.PrintCurrentState();
            //stateMachine.NextStep();
            ////stateMachine.PrintCurrentState();
            //stateMachine.NextStep();
            ////stateMachine.PrintCurrentState();
            //stateMachine.NextStep();
            //stateMachine.NextStep();
            //stateMachine.NextStep();
            ////stateMachine.PrintCurrentState();
            Console.Write("");
        }
    }
}
