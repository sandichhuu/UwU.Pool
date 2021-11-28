using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace UwU
{
    public class Pool<T> : IPool
    {
        public ItemEvent onNewItemCreated;
        public ItemEvent onItemReturnToPool;
        public ItemEvent onItemRequested;

        public delegate void ItemEvent(T item);

        private Queue<int> free;
        private List<int> busy;
        private Dictionary<int, PoolItem> itemDomain;

        private T sample;

        public void Initialize(T sample, int capacity = 16)
        {
            this.sample = sample;
            this.free = new Queue<int>(capacity);
            this.busy = new List<int>(capacity);
            this.itemDomain = new Dictionary<int, PoolItem>();

            var instances = SpawnInstanceBuck(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var poolItem = BuildNewPoolItem(instances[i]);
                var poolItemHash = poolItem.instanceId;

                this.free.Enqueue(poolItemHash);
                this.itemDomain.Add(poolItemHash, poolItem);
            }
        }

        public T Request()
        {
            var itemHash = -1;
            PoolItem freeItem = null;

            if (this.free.Count > 0)
            {
                itemHash = this.free.Dequeue();
                freeItem = this.itemDomain[itemHash];
            }

            if (freeItem == null)
            {
                freeItem = BuildNewPoolItem(SpawnInstance());
                itemHash = freeItem.instanceId;
                this.itemDomain.Add(itemHash, freeItem);

                Debug.LogWarning("Warning ! There are no free item, add new item !");
            }

            this.busy.Add(itemHash);

            var requestedItem = (T)freeItem.obj;
            this.onItemRequested?.Invoke(requestedItem);
            return requestedItem;
        }

        public void ReturnItem(T item)
        {
            var itemHash = item.GetHashCode();
            var itemBusyIndex = this.busy.IndexOf(itemHash);

            if (itemBusyIndex >= 0)
            {
                var poolItem = this.itemDomain[itemHash];

                this.busy.RemoveAt(itemBusyIndex);
                this.free.Enqueue(itemHash);

                this.onItemReturnToPool?.Invoke(item);
            }
            else
            {
                Debug.LogWarning($"The item with [uid: {itemHash}] is not in using, recheck please !");
            }
        }

        private PoolItem BuildNewPoolItem(T obj)
        {
            this.onNewItemCreated?.Invoke(obj);
            return new PoolItem(obj);
        }

        private T[] SpawnInstanceBuck(int quantity)
        {
            var instances = new T[quantity];

            if (this.sample is Component component)
            {
                for (var i = 0; i < quantity; i++)
                    instances[i] = Object.Instantiate(component).GetComponent<T>();
            }
            else
            {
                instances = DeepCloneBuck(this.sample, quantity);
            }

            return instances;
        }

        private T SpawnInstance()
        {
            T instance;

            if (this.sample is Component component)
            {
                instance = Object.Instantiate(component).GetComponent<T>();
            }
            else
            {
                instance = DeepClone(this.sample);
            }

            return instance;
        }

        private T DeepClone(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        private T[] DeepCloneBuck(T obj, int quantity)
        {
            var instances = new T[quantity];

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);

                for (var i = 0; i < quantity; i++)
                {
                    stream.Position = 0;
                    instances[i] = (T)formatter.Deserialize(stream);
                }
            }

            return instances;
        }

        object IPool.RequestUnsafe()
        {
            return Request();
        }

        public void ReturnItemToPool(object objectToReturn)
        {
            ReturnItem((T)objectToReturn);
        }
    }

    internal sealed class PoolItem
    {
        public int instanceId;
        public object obj;

        public PoolItem(object obj)
        {
            SetItem(obj);
        }

        public void SetItem(object obj)
        {
            this.obj = obj;
            this.instanceId = obj.GetHashCode();
        }
    }

    internal sealed class PoolOfPoolItem : Pool<PoolItem>
    {
    }

    public interface IPool
    {
        object RequestUnsafe();

        void ReturnItemToPool(object objectToReturn);
    }
}