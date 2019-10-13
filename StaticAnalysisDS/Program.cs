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
            VSSLInterpreter interpreter = new VSSLInterpreter(path);
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
            interpreter.NextStep();
        }
    }
}
