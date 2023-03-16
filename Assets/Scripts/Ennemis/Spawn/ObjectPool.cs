using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public GameObject parentOfObject;
    public bool shouldExpand;

}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool, item.parentOfObject.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeSelf && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, item.parentOfObject.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    }
}