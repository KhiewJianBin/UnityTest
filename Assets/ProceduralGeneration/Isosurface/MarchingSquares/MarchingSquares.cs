using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Refrences:
/// https://en.wikipedia.org/wiki/Marching_squares
/// https://jamie-wong.com/2014/08/19/metaballs-and-marching-squares/
/// https://catlikecoding.com/unity/tutorials/marching-squares/
/// 
/// 
/// </summary>
public class MarchingSquares : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField, Range(0, 100)] int gridSize = 100;
    [SerializeField, Range(0, 10)] float gridResolution = 1;
    [SerializeField, Range(0, 1)] float gridPointSize = 0.5f; // Percentage of gridResolution

    [Header("Noise")]
    [SerializeField] float noiseResolution = 0.1f;
    [SerializeField] float noiseOffset;

    [Header("Sampling")]
    public float binarythreshold = 0.5f;
    public bool UseSampling = true;


    [SerializeField] MeshFilter meshFilter;

    float[,] dataGrid;
    List<Vector3> vertices = new();
    List<int> triangles = new();


    void OnDrawGizmos()
    {
        dataGrid = new float[gridSize + 1, gridSize + 1];

        BuildDataGrid();

        var pos = transform.position;
        var pointSize = gridPointSize / 2 * gridResolution;
        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                var offset = new Vector3(x, y, 0) * gridResolution;
                Gizmos.color = new Color(dataGrid[x, y], dataGrid[x, y], dataGrid[x, y]);
                Gizmos.DrawSphere(pos+offset, pointSize);
            }
        }

        if (meshFilter == null) return;

        MarchSquares(pos);

        Mesh mesh = new();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        meshFilter.mesh = mesh;
    }


    void MarchSquares(Vector3 vertexOffset,bool interpolate = true)
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < gridSize; x++)
        for (int y = 0; y < gridSize; y++)
        {
            var bl = dataGrid[x, y] > binarythreshold ? 0 : 1;
            var br = dataGrid[x + 1, y] > binarythreshold ? 0 : 1;
            var tr = dataGrid[x + 1, y + 1] > binarythreshold ? 0 : 1;
            var tl = dataGrid[x, y + 1] > binarythreshold ? 0 : 1;

            var bitflag = bl * 8 + br * 4 + tr * 2 + tl * 1;

            var verticesLocal = new Vector3[6];
            var trianglesLocal = new int[6];
            var vertexCount = vertices.Count;

            switch (bitflag)
            {
                case 0:
                    return;
                case 1:
                    verticesLocal = new Vector3[]
                    {
                new Vector3(0, 1f),
                new Vector3(0, 0.5f),
                new Vector3(0.5f, 1)
                    };

                    trianglesLocal = new int[]
                    { 2, 1, 0};

                    break;
                case 2:
                    verticesLocal = new Vector3[]
                    { new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 1) };

                    trianglesLocal = new int[]
                    { 0, 1, 2};
                    break;
                case 3:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 0, 2, 3};
                    break;
                case 4:
                    verticesLocal = new Vector3[]
                    { new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                    trianglesLocal = new int[]
                    { 0, 1, 2};
                    break;
                case 5:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 3, 4, 5};
                    break;
                case 6:
                    verticesLocal = new Vector3[]
                    { new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(1, 1), new Vector3(1, 0) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 0, 2, 3};
                    break;
                case 7:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(0, 0.5f) };

                    trianglesLocal = new int[]
                    { 2, 3, 1, 3, 4, 1, 4, 0, 1};
                    break;
                case 8:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0.5f), new Vector3(0, 0), new Vector3(0.5f, 0) };

                    trianglesLocal = new int[]
                    { 2, 1, 0};
                    break;
                case 9:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(0, 1) };

                    trianglesLocal = new int[]
                    { 1, 0, 2, 0, 3, 2};
                    break;
                case 10:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 0), new Vector3(1, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 5, 4, 3};
                    break;
                case 11:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 0) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 0, 2, 3, 4, 0, 3};
                    break;
                case 12:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(1, 0), new Vector3(1, 0.5f), new Vector3(0, 0.5f) };

                    trianglesLocal = new int[]
                    { 0, 3, 2, 0, 2, 1};
                    break;
                case 13:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f), new Vector3(1, 0) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 0, 2, 3, 0, 3, 4};
                    break;
                case 14:
                    verticesLocal = new Vector3[]
                    { new Vector3(1, 1), new Vector3(1, 0), new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };

                    trianglesLocal = new int[]
                    { 0, 1, 4, 1, 3, 4, 1, 2, 3};
                    break;
                case 15:
                    verticesLocal = new Vector3[]
                    { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0) };

                    trianglesLocal = new int[]
                    { 0, 1, 2, 0, 2, 3};
                    break;
            }

            foreach (Vector3 vert in verticesLocal)
            {
                Vector3 newVert = new((vert.x + vertexOffset.x) * gridResolution, (vert.y + vertexOffset.y) * gridResolution, 0);
                vertices.Add(newVert);
            }

            foreach (int triangle in trianglesLocal)
            {
                triangles.Add(triangle + vertexCount);
            }
        }
    }

    int GetHeight(float value)
    {
        return value < 0.5f ? 0 : 1;
    }

    float SamplingFunction(Vector2 position)
    {
        float r = 1f;
        float x = position.x;
        float y = position.y;

        return (r * r - x * x - y * y);
    }

    void BuildDataGrid()
    {
        if (UseSampling)
        {
            for (int x = 0; x <= gridSize; x++)
            {
                for (int y = 0; y <= gridSize; y++)
                {
                    var pos = new Vector2(x, y);
                    dataGrid[x, y] = SamplingFunction(pos);
                }
            }
        }
        else
        {
            for (int x = 0; x <= gridSize; x++)
            {
                for (int y = 0; y <= gridSize; y++)
                {
                    dataGrid[x, y] = Mathf.PerlinNoise(x * noiseResolution, y * noiseResolution);
                }
            }
        }
    }
}
