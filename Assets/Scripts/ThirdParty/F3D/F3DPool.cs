using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class F3DPool : MonoBehaviour {

    public static F3DPool instance;         // Singleton instance

    [Header("VFX Pool")]
    public Transform[] poolItems;           // Effect pool prefabs
    public int[] poolLength;                // Effect pool items count

    // Pooled items collections
    private Dictionary<Transform, Transform[]> pool;

    // Use this for initialization
    void Start ()
    {
        // Singleton instance
        instance = this;

        // Initialize effects pool
        if (poolItems.Length > 0)
        {
            pool = new Dictionary<Transform, Transform[]>();

            for (int i = 0; i < poolItems.Length; i++)
            {
                Transform[] itemArray = new Transform[poolLength[i]];

                for (int x = 0; x < poolLength[i]; x++)
                {
                    Transform newItem = (Transform)Instantiate(poolItems[i], Vector3.zero, Quaternion.identity);
                    newItem.gameObject.SetActive(false);
                    newItem.parent = transform;

                    itemArray[x] = newItem;
                }

                pool.Add(poolItems[i], itemArray);
            }
        }
    }
    
    // Spawn effect prefab and send OnSpawned message
    public Transform Spawn(Transform obj, Vector3 pos, Quaternion rot, Transform parent)
    {
        for (int i = 0; i < pool[obj].Length; i++)
        {
            if(!pool[obj][i].gameObject.activeSelf)
            {
                Transform spawnItem = pool[obj][i];

                spawnItem.parent = parent;
                spawnItem.position = pos;
                spawnItem.rotation = rot;
                
                spawnItem.gameObject.SetActive(true);
                spawnItem.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);

                return spawnItem;
            }
        }

        return null;
    }

    // Despawn effect or audio and send OnDespawned message
    public void Despawn(Transform obj)
    {
        obj.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(false);
    }
}
