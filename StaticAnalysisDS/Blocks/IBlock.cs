namespace StaticAnalysisDS
{
    internal interface IBlock
    {
        bool IsFinished();
        void NextStep();
    }
}