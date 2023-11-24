namespace UwU.Pool
{
    public interface IPoolReturnItem
    {
        void ReturnItem<T>(T item) where T : PoolElement;
    }
}