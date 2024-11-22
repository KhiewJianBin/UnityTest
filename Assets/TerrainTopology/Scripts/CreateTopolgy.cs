//https://github.com/Scrawk/Terrain-Topology-Algorithms
using UnityEngine;

public abstract class CreateTopologyMap : MonoBehaviour
{
    [Header("Path To HeightMap To Use To Topology Map Generation")]
    public string heightmap_resourcePath = "/TerrainTopology/Heights.raw";
    public Material m_material;

    void Start()
    {
        if (m_material == null) return;
        string fileName = Application.dataPath + heightmap_resourcePath;
        float[] heights = Load16BitFloat(fileName);

        int width = 1024;
        int height = 1024;

        m_material.mainTexture = CreateMap(heights, width, height);
    }

    void OnDestroy()
    {
        if (m_material == null) return;
        m_material.mainTexture = null;
    }

    protected abstract Texture2D CreateMap(float[] heights, int width, int height);

    //todo change to extension
    protected float[] Load16BitFloat(string fileName, bool bigendian = false)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(fileName);

        int size = bytes.Length / 2;
        float[] data = new float[size];

        for (int x = 0, i = 0; x < size; x++)
        {
            data[x] = (bigendian) ? (bytes[i++] * 256.0f + bytes[i++]) : (bytes[i++] + bytes[i++] * 256.0f);
            data[x] /= ushort.MaxValue;
        }

        return data;
    }
}
