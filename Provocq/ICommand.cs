namespace Provocq
{
    public interface ICommand<T>
    {
        void Execute(T context);
    }
}
