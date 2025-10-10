using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    public GameObject projectilePrefab;
    public int initialSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        for (int i = 0; i < initialSize; i++)
        {
            var go = Instantiate(projectilePrefab);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Dequeue();
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(projectilePrefab, position, rotation);
        }
        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
