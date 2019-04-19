namespace ChurnAnalyzers
{
    public interface IAnalyzer<TResult>
    {
        TResult Execute();
    }
}