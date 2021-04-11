namespace Provocq
{
    public interface IPersistor<T>
    {
        public T Load();

        public void Save(T dataContext);
    }
}
