using LowoUN.Util;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Sample_Pool : MonoBehaviour {
    [SerializeField] GameObject cube; //测试预制体
    [SerializeField] int initDefaultCapacity = 10; //初次实例化分配的List容器大小
    [SerializeField] int poolMaxSize = 10000; //对象池 最大容量

    //已下用于测试ObjectPool三个属性
    [Header ("测试 >>> 通过IMGUI 模拟触发Cube测试对象的生成与销毁")]
    [SerializeField] int CountActive; //已知每调用一次Release方法就会-1，Get方法+1
    [SerializeField] int CountAll; //已知每调用一次Create方法就会+1
    [SerializeField] int CountInactive; //已知每调用一次Release方法就会+1，Get方法-1,最大值为 poolMaxSize
    [SerializeField] int currentNumber; //显示当前存在的Cube数量

    ObjectPool<GameObject> cubePool; //对象池
    GameObject[] allCubes; // 当前所有从对象池Get出的对象

    void Awake () {
        //构建对象池 ParticleSystem
        PoolManager.Instance.CreatePool_ParticleSystem ();

        //构建对象池 GameObject
        cubePool = PoolManager.Instance.CreatePool (cube, initDefaultCapacity, poolMaxSize);

        UpdateInfo ();
    }

    void Get_One () {
        var obj = cubePool.Get ();
        obj.transform.position = new Vector3 (Random.Range (-5, 5), Random.Range (0, 5), 5);

        UpdateInfo ();
    }

    //随机选出一个Cube然后删除
    void Release_One () {
        allCubes = GameObject.FindGameObjectsWithTag ("TEST");
        if (allCubes.Length > 0) {
            GameObject c = allCubes[Random.Range (0, allCubes.Length)];
            cubePool.Release (c);
        }

        UpdateInfo ();
    }

    void Release_All () {
        allCubes = GameObject.FindGameObjectsWithTag ("TEST");
        Debug.Log ($"allEnemies length: {allCubes.Length}");
        for (int i = 0; i < allCubes.Length; i++) {
            cubePool.Release (allCubes[i]);
        }

        UpdateInfo ();
    }

    void UpdateInfo () {
        CountAll = cubePool.CountAll;
        CountActive = cubePool.CountActive;
        CountInactive = cubePool.CountInactive;
        currentNumber = CountActive;
        Debug.Log ($"cubePool CountAll:{cubePool.CountAll},CountActive:{cubePool.CountActive},CountInactive:{cubePool.CountInactive},currentNumber:{currentNumber}");
        if (cubePool.CountAll > poolMaxSize)
            throw new System.Exception ("pool current elements count is greater than pool MaxSize setting");
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 24;

        if (GUI.Button (new Rect (10, 120, 160, 40), "Get"))
            Get_One ();
        if (GUI.Button (new Rect (180, 120, 160, 40), "Release One"))
            Release_One ();
        if (GUI.Button (new Rect (360, 120, 160, 40), "Release All"))
            Release_All ();
        if (GUI.Button (new Rect (540, 120, 160, 40), "Destroy Pool")) {
            PoolManager.Instance.DestroyPool (cubePool);
            UpdateInfo ();
        }

        if (PoolManager.Instance.Pool_ParticleSystem != null) {
            GUI.skin.label.fontSize = 30;
            GUILayout.Label ("Pool size: " + PoolManager.Instance.Pool_ParticleSystem.CountInactive);
            if (GUI.Button (new Rect (200, 5, 240, 40), "Create Particles")) {
                var amount = Random.Range (1, 10);
                for (int i = 0; i < amount; ++i) {
                    var ps = PoolManager.Instance.Pool_ParticleSystem.Get ();
                    ps.transform.position = Random.insideUnitSphere * 10;
                    ps.Play ();
                }
            }
        }

        if (GUI.Button (new Rect (10, 200, 160, 40), "Pool Init()"))
            PoolManager.Instance.Init ();
        if (GUI.Button (new Rect (180, 200, 160, 40), "Pool End()")) {
            Release_All();
            PoolManager.Instance.End ();
        }
    }
}