using System;
using UnityEngine;

namespace UwU.Pool.Example
{
    public class CubeManager : MonoBehaviour
    {
        private CubePool cubePool;
        [SerializeField] private CubeBehaviour cubePrefab = default;

        private float timeCounter = 0f;

        private void Awake()
        {
            this.cubePool = new CubePool();
            this.cubePool.onNewItemCreated += OnNewItemCreated;
            this.cubePool.onItemRequested += OnItemRequested;
            this.cubePool.onItemReturnToPool += OnItemReturnToPool;

            this.cubePool.Initialize(this.cubePrefab, 10);
        }

        private void OnItemRequested(CubeBehaviour item)
        {
            // Do something hentai when item is requested
        }

        private void OnItemReturnToPool(CubeBehaviour item)
        {
            // Do something hentai when item return to pool
        }

        private void OnNewItemCreated(CubeBehaviour item)
        {
            item.SetPool(this.cubePool);
        }

        private void Update()
        {
            this.timeCounter += Time.deltaTime;

            if (this.timeCounter >= 1.0f)
            {
                this.timeCounter = 0f;
                var cubeInstance = RequestObjectFromPool();

                cubeInstance.gameObject.SetActive(true);
                cubeInstance.transform.position = Vector3.zero;
            }
        }

        public CubeBehaviour RequestObjectFromPool()
        {
            return this.cubePool.Request();
        }
    }
}