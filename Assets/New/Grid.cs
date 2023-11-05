using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Material terrainMaterial;

    public Vector2Int worldSize = new Vector2Int(100, 100);

    public int[,] map;

    public float terrainFreq;

    public int heightAddition;

    public int heightMultiplier;

    public int dirtLayerHeight;

    int seed;

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        CellularAutomata();

    }

    void CellularAutomata()
    {
        map = new int[worldSize.x, worldSize.y];
        GenerateMap();

        for (int i = 0; i < 5; i++)
            SmoothMap();

        RemoveSecludedCells();
        PlaceMap(map);
        DrawTexture(map);
    }

    public void GenerateMap()
    {

        //TileClass tile;
        for (int x = 0; x < worldSize.x; x++)
        {
            //float height = Mathf.PerlinNoise((x + seed) * GetCurrentBiome(x, heightAddition).terrainFreq, seed * GetCurrentBiome(x, heightAddition).terrainFreq) * GetCurrentBiome(x, heightAddition).heightMultiplier + heightAddition;
            for (int y = 0; y < worldSize.y; y++)
            {


                if (Random.Range(0, 100) > 50 - y / 10)
                    map[x, y] = 1;
                else
                    map[x, y] = 0;



            }
        }
    }

    public void SmoothMap()
    {
        for (int x = 0; x < worldSize.x; x++)
        {

            for (int y = 0; y < worldSize.y; y++)
            {


                if (GetNeighboursCellCount(x, y, map) > 4)
                    map[x, y] = 1;
                else if (GetNeighboursCellCount(x, y, map) < 4)
                    map[x, y] = 0;



            }
        }
    }

    int GetNeighboursCellCount(int x, int y, int[,] map)
    {
        int neighbors = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x + i != -1 && x + i != worldSize.x && y + j != -1 && y + j != worldSize.y)
                {
                    neighbors += map[i + x, j + y];
                }
            }
        }

        neighbors -= map[x, y];

        return neighbors;
    }

    void RemoveSecludedCells()
    {
        for (int x = 1; x < worldSize.x - 1; x++)
        {

            for (int y = 0; y < worldSize.y; y++)
            {
                map[x, y] = (GetNeighboursCellCount(x, y, map) <= 0) ? 0 : map[x, y];
            }
        }
    }

    public void PlaceMap(int[,] map)
    {
        map[0, 0] = 1;
        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        for(int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                int cell = map[x, y];
                if(cell == 1)
                {
                    Vector3 a = new Vector3(x - 0.5f, y + .5f,0);
                    Vector3 b = new Vector3(x + 0.5f, y + .5f, 0);
                    Vector2 c = new Vector3(x - 0.5f, y - .5f, 0);
                    Vector3 d = new Vector3(x + 0.5f, y - .5f, 0);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };

                    Vector2 v1 = new Vector2(0, 0);
                    Vector2 v2 = new Vector2(0, 1);
                    Vector2 v3 = new Vector2(1, 1);
                    Vector2 v4 = new Vector2(1, 0);
                    Vector2[] u = new Vector2[] { v1, v2, v3, v2, v4, v3 };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        uv.Add(u[k]);
                        triangles.Add(triangles.Count);
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        UnityEngine.MeshFilter meshFilter= gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        

        UnityEngine.MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

    }

    void DrawTexture(int[,] map)
    {
        Texture2D texture = new Texture2D(worldSize.x, worldSize.y);
        Color[] colorMap = new Color[worldSize.x * worldSize.y];
        for (int x = 0; x < worldSize.x; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < worldSize.y; y++)
            {
                int cell = map[x, y];
                if (cell == 1)
                {
                    if (y >= height - 1)
                        colorMap[y * worldSize.y + x] = Color.green;
                    else if (y >= height - dirtLayerHeight)
                        colorMap[y * worldSize.y + x] = new Color(0.58f,0.29f,0);
                    else if (y <= height)
                        colorMap[y * worldSize.y + x] = Color.gray;
                }
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        UnityEngine.MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }
}
