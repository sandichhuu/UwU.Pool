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
            this.cubePool.Initialize(this.cubePrefab, 1);

            this.cubePool.onNewItemCreated += OnNewItemCreated;
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
                var cubeInstance = this.cubePool.Request();
                cubeInstance.gameObject.SetActive(true);
                cubeInstance.transform.position = Vector3.zero;
            }
        }
    }
}