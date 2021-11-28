using UnityEngine;

namespace UwU.Pool.Example
{
    public class CubeBehaviour : MonoBehaviour
    {
        private IPool pool;
        private float timeCounter = 0f;

        public void SetPool(IPool pool)
        {
            this.pool = pool;
        }

        private void OnEnable()
        {
            this.timeCounter = 0f;
        }

        private void FixedUpdate()
        {
            this.timeCounter += Time.fixedDeltaTime;

            if (this.timeCounter >= 1f)
            {
                this.gameObject.SetActive(false);
                ReturnToPool();
            }

            this.transform.Rotate(Vector3.one);
        }

        public void ReturnToPool()
        {
            this.pool.ReturnItemToPool(this);
        }
    }
}