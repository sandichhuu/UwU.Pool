namespace UwU.Pool
{
    public abstract class PoolElement : CustomBehaviour
    {
        private IPoolReturnItem poolReturnItem;
        
        internal void SetupPool(IPoolReturnItem poolReturnItem)
        {
            this.poolReturnItem = poolReturnItem;
        }

        public void ReturnToPool()
        {
            this.poolReturnItem.ReturnItem(this);
        }
    }
}