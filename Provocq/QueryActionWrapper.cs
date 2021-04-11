namespace Provocq
{
    public class QueryActionWrapper<T, TResult> : ActionWrapper<T>
    {
        public TResult Result { get; set; }
    }
}
