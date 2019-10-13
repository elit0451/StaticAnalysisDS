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
            string path = @".\VSSL.txt";
            StateMachine stateMachine = new StateMachine(path);
            stateMachine.NextStep();
        }
    }
}
