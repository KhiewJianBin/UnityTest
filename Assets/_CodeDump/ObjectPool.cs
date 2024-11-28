//This is a modification of the original code found here: http://forum.unity3d.com/threads/simple-reusable-object-pool-help-limit-your-instantiations.76851/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance = null;   //static reference
    public GameObject[] prefabs;                //Collection of prefabs to be poooled
    GameObject[] poolobjectContainer;           //GameObject containers for each prefabs
    public int defaultBufferAmount = 10;        //Default pooled amount if no amount abaove is supplied
    public int[] amountToBuffer;                //The amount to pool of each object. This is optional
    public bool canGrow = true;                 //Whether or not the pool can grow.

    public List<GameObject>[] pooledObjects;    //The actual collection of pooled objects

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //Optimise prefabs, to eliminated having two or more of the same prefabs in pool using Linq
        prefabs = prefabs.Distinct().ToArray();

        poolobjectContainer = new GameObject[prefabs.Length];
        pooledObjects = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            //Create a container for each pool prefab
            poolobjectContainer[i] = new GameObject(prefabs[i].name + " Container");
            poolobjectContainer[i].transform.parent = transform;

            //Create and Populate the List based on buffer Amount
            pooledObjects[i] = new List<GameObject>();

            //determine the Buffer amount
            int bufferAmount;
            if (i < amountToBuffer.Length)
                bufferAmount = amountToBuffer[i];
            else
                bufferAmount = defaultBufferAmount;

            //Create the Buffer amount of Prefab and place it in the pool
            for (int j = 0; j < bufferAmount; j++)
            {
                GameObject go = Instantiate(prefabs[i]); // Create an instance of the prefab
                go.name = prefabs[i].name; // Set as the same name as prefab
                PoolObject(go); // and Add it into the pool
            }
        }
    }

    public GameObject GetObject(GameObject objectType)
    {
        for (int poolindex = 0; poolindex < prefabs.Length; poolindex++)
        {
            //Find which prefab the object belongs to
            if (prefabs[poolindex].name == objectType.name)
            {
                //Check if there is any object in the pool
                if (pooledObjects[poolindex].Count > 0)
                {
                    GameObject pooledObject = pooledObjects[poolindex][0]; //Obtain the first instance of the object
                    pooledObjects[poolindex].RemoveAt(0); //remove it from the pool
                    pooledObject.transform.parent = null; //remove it from parent
                    return pooledObject; //give to caller
                }
                //Otherwise, if there is no object in pool, check if the pool is allowed to grow...
                else if (canGrow)
                {
                    GameObject go = Instantiate(prefabs[poolindex]);
                    go.name = prefabs[poolindex].name; //give a name
                    return go; //give to caller
                }
                break; //If it is empty and can't grow, break out of the loop
            }
        }
        return null; // GameObject type doesnt not match prefab. this means its not a prefab considered for object pooled
    }

    public void PoolObject(GameObject obj)
    {
        //Find the correct pool for the object to go in to
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == obj.name)
            {
                obj.SetActive(false); //Deactivate it...
                obj.transform.parent = poolobjectContainer[i].transform; //parent it to the apporiate container
                pooledObjects[i].Add(obj); //and add it back to the pool
                return;
            }
        }
    }

    void GrowPool(int poolindex)
    {

    }
}