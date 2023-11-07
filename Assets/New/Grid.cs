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

    public Sprite Dirt;
    public Sprite Stone;
    public Sprite Grass;

    public float numywumy = 8f;



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
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                int threshold = 50;
                if(y < (worldSize.y / 5) * 3)
                {
                    threshold = 50;
                } else if (y < (worldSize.y / 5) * 4)
                {
                    threshold = 20;
                } else
                {
                    threshold = 5;
                }

                if (Random.Range(0, 100) > threshold)
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
        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        for(int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                int cell = map[x, y];
                if (cell == 1)
                {
                    
                    Vector3 a = new Vector3(x - 0.5f, y + 0.5f, 0);
                    Vector3 b = new Vector3(x + 0.5f, y + 0.5f, 0);
                    Vector3 c = new Vector3(x - 0.5f, y - 0.5f, 0);
                    Vector3 d = new Vector3(x + 0.5f, y - 0.5f, 0);
                    Vector3[] v = new Vector3[] { a, b, c, d };

                    /*Vector2 v1 = new Vector2((x + 1) / (float)worldSize.x * 8, y / (float)worldSize.y * 8);
                    Vector2 v2 = new Vector2(x / (float)worldSize.x * 8, y / (float)worldSize.y * 8);
                    Vector2 v3 = new Vector2((x + 1) / (float)worldSize.x * 8, (y + 1) / (float)worldSize.y * 8);
                    Vector2 v4 = new Vector2(x / (float)worldSize.x * 8, (y + 1) / (float)worldSize.y * 8);*/
                    Vector2 v1 = (((new Vector2(x, y) + Vector2.up) / worldSize) * (Vector2.one * numywumy));
                    Vector2 v2 = (((new Vector2(x, y) + Vector2.zero) / worldSize) * (Vector2.one * numywumy));
                    Vector2 v3 = (((new Vector2(x, y) + Vector2.one) / worldSize) * (Vector2.one * numywumy));
                    Vector2 v4 = (((new Vector2(x, y) + Vector2.right) / worldSize) * (Vector2.one * numywumy));
                    Vector2[] u = new Vector2[] { v1, v2, v3, v4 };
                    

                    int vertCount = vertices.Count;
                    vertices.AddRange(v);
                    uv.AddRange(u);

                    // First triangle
                    triangles.Add(vertCount + 0);
                    triangles.Add(vertCount + 1);
                    triangles.Add(vertCount + 2);

                    // Second triangle
                    triangles.Add(vertCount + 2);
                    triangles.Add(vertCount + 1);
                    triangles.Add(vertCount + 3);
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
        Texture2D texture = new Texture2D(worldSize.x * 8, worldSize.y * 8);


        Color[] colorMap = new Color[(worldSize.x * 8) * (worldSize.y * 8 * 8)];


        for (int x = 0; x < worldSize.x; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < worldSize.y * 8; y++)
            {
                Texture2D tileTexture = new Texture2D(8,8);
                Color color = Color.white;
                if (y >= height - 1)
                    tileTexture = GetTileTexture(Grass, 8, 24);
                else if (y >= height - dirtLayerHeight)
                    tileTexture = GetTileTexture(Dirt,8 ,16 );
                else if (y <= height)
                    tileTexture = GetTileTexture(Stone, 8,16);

                Color[] tilePixels = tileTexture.GetPixels();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        float v = Random.Range(0f, 0.1f);
                        colorMap[(((y * 8) + j) * worldSize.x * 8) + (((x * 8) + i))] = tilePixels[(7 - j) + ((7 - i) * 8)];

                    }
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

    Texture2D GetTileTexture(Sprite sprite, int x, int y)
    {
        var croppedTexture = new Texture2D(8, 8);

        var pixels = sprite.texture.GetPixels(x, y, 8, 8);
                                                

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        croppedTexture.filterMode = FilterMode.Point;
        return croppedTexture;
    }
}
