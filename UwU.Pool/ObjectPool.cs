using Collections.Pooled;
using System.Linq;
using UnityEngine;

namespace UwU.Pool
{
    public class ObjectPool<T> : IPoolReturnItem where T : PoolElement
    {
        public StartEvent onNewItemCreated;
        public ItemEvent onItemRequested;
        public ItemEvent onItemReturned;

        private PooledQueue<T> queue;
        private T prefab;

        public void Initialize(T prefab, int capacity = 16)
        {
            this.prefab = prefab;
            this.queue = new PooledQueue<T>(capacity);

            var instances = Create(capacity);
            for (var i = 0; i < capacity; i++)
            {
                this.queue.Enqueue(instances[i]);
            }
        }

        public T Request()
        {
            T result;

            if (this.queue.Count > 0)
            {
                result = this.queue.Dequeue();
            }
            else
            {
                result = Create();
            }

            this.onItemRequested?.Invoke(result);
            return result;
        }

        public void ReturnItem<PoolItemType>(PoolItemType instance) where PoolItemType : PoolElement
        {
            var item = instance as T;

#if UNITY_EDITOR
            if (this.queue.Contains(item))
            {
                Debug.LogError("Item is already in pool ! Cannot return item.");
            }
#endif
            this.queue.Enqueue(item);
            this.onItemReturned?.Invoke(item);
        }

        private T[] Create(int quantity)
        {
            var instances = new T[quantity];
            for (var i = 0; i < quantity; i++)
            {
                var instance = Create();
                instance.SetupPool(this);

                instances[i] = instance;
            }

            return instances;
        }

        private T Create()
        {
            var clone = Object.Instantiate(this.prefab.gameObject);
            var instance = clone.GetComponent<T>();
            instance.SetupPool(this);

            this.onNewItemCreated?.Invoke(this, instance);
            return instance;
        }

        public delegate void StartEvent(ObjectPool<T> fromPool, T item);
        public delegate void ItemEvent(T item);
    }
}