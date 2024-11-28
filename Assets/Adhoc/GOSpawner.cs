using UnityEngine;

public class GOSpawner : MonoBehaviour
{
    public GameObject Prefab;

    public virtual GameObject SpawnPrefab(
        Vector3? inSpawnPos = null,
        Quaternion? inSpawnRot = null,
        Vector3? inSpawnScale = null)
    {
        if (Prefab)
        {
            GameObject newPrefab = Instantiate(Prefab);
            newPrefab.transform.SetParent(transform);
            if (inSpawnPos.HasValue)
            {
                newPrefab.transform.position = inSpawnPos.Value;
            }
            if (inSpawnRot.HasValue)
            {
                newPrefab.transform.rotation = inSpawnRot.Value;
            }
            if (inSpawnScale.HasValue)
            {
                newPrefab.transform.localScale = inSpawnScale.Value;
            }
            return newPrefab;
        }
        Debug.LogWarning(name + " Spawner Has no Prefab");
        return null;
    }
}