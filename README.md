# UωU ❤❤❤ UNITY OBJECT POOL

![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) <strong>PRODUCT FROM VIETNAM ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) LOVE FROM VIETNAMESE</strong> ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true)


Why use ?
- Simple, easy to use.
- No misuse MonoBehaviour.

### HOW TO USE ?

- Create a pool
```csharp
public class CubePool : Pool<CubeBehaviour>
{ 
}
```

```csharp
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
}
```

- Request a object from pool
```csharp
public CubeBehaviour RequestObjectFromPool()
{
    return this.cubePool.Request();
}
```

- Return object to pool
```csharp
public class CubeBehaviour : MonoBehaviour
{
    private IPool pool;

    public void SetPool(IPool pool)
    {
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        this.pool.ReturnItemToPool(this);
    }
}
```

Easy to use, right ?

# (づ｡◕‿‿◕｡)づ 

## IF THESE HELP YOU FINISH PROJECT, PLEASE DONATE ME A COFFEE CUP

PAYPAL: https://paypal.me/sandichhuu

ヾ(＠＾▽＾＠)ﾉ THANK YOU

.

.

.

.

.

.

.

.

###### ಠ_ಠ DONATE OR LUCKY -1000
