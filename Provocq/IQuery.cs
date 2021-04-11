namespace Provocq
{
    public interface IQuery<T, TResult>
    {
        TResult Execute(T context);
    }
}