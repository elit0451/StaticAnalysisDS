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
            string path = @"C:\Users\elitsa\source\repos\StaticAnalysisDS\StaticAnalysisDS\bin\Debug\VSSL.txt";
            StateMachine stateMachine = new StateMachine(path);
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            Console.WriteLine(stateMachine.CurrentLine());
            stateMachine.NextStep();
            //stateMachine.PrintCurrentState();
            Console.WriteLine(stateMachine.CurrentLine());
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
