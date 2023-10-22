using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public Tilemap worldTileMap;
    public Tilemap worldWallMap;
    public Tilemap worldTreeMap;
    public Tilemap worldSemiSolidMap;
    public BiomeClass[] biomes;

    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;

    [Header("Trees")]    
    public int treeChance = 10;
    public int minTreeHeight = 4;
    public int maxTreeHeight = 6;
    public int TreeCooldown=0;

    

    [Header("Biomes")]
    public float biomeFrequency;
    public Gradient biomeGradient;
    public Texture2D biomeMap;

    [Header("Foliage")]
    public Sprite[] foliageSprites;
    public int foliageChance = 6;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int dirtLayerHeight = 15;
    public float surfaceValue;
    public int worldSize = 100;
    public float heightMultiplier = 4f;
    public int heightAddition = 25;

    [Header("Noise Settings")]
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float seed;
    public Texture2D caveNoiseTexture;

    [Header("Ore Settings")]
    public OreClass[] ores;



    private GameObject[] worldChunks;
    //private List<Vector2> worldTiles = new List<Vector2>();
    public IDictionary<Vector2, TileClass> worldTiles = new Dictionary<Vector2, TileClass>();
    public IDictionary<Vector2, TileClass> worldWalls = new Dictionary<Vector2, TileClass>();
    private BiomeClass curBiome;


    void Start()
    {
       
        seed = Random.Range(-10000, 10000);
        DrawTextures();

        //CreateChunks();
        GenerateWalls();
        GenerateTerrain();
        GenerateExtras();

        
    }

    public void DrawTextures()
    {
        biomeMap = new Texture2D(worldSize, worldSize);
        /*caveNoiseTexture = new Texture2D(worldSize, worldSize);
        ores[0].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[1].spreadTexture = new Texture2D(worldSize, worldSize);*/
        DrawBiomeTexture();

        /*GenerateNoiseTexture(ores[0].rarity, ores[0].size, ores[0].spreadTexture);
        GenerateNoiseTexture(ores[1].rarity, ores[1].size, ores[1].spreadTexture);
        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);*/
        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i].caveNoiseTexture = new Texture2D(worldSize, worldSize);
            for (int o = 0; o < biomes[i].ores.Length; o++)
            {
                biomes[i].ores[o].spreadTexture = new Texture2D(worldSize, worldSize);
            }

            GenerateNoiseTexture(biomes[i].caveFreq, biomes[i].surfaceValue, biomes[i].caveNoiseTexture);

            for (int o = 0; o < biomes[i].ores.Length; o++)
            {
                GenerateNoiseTexture(biomes[i].ores[o].rarity, biomes[i].ores[o].size, biomes[i].ores[o].spreadTexture);
            }
        }




    }

    public void DrawBiomeTexture()
    {

        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed + Mathf.Sin(y) * 0.5f) * biomeFrequency, seed * biomeFrequency);
                Color col = biomeGradient.Evaluate(v);
                biomeMap.SetPixel(x, y, col);


            }
        }
        biomeMap.Apply();

    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];
        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }
    public BiomeClass GetCurrentBiome(int x, int y)
    {
        //change curbiome value here;



        for (int i = 0; i < biomes.Length; i++)
        {
            if (biomes[i].biomeColor == biomeMap.GetPixel(x, y))
            {
                return biomes[i];

            }
        }

        return curBiome;
    }
    public void GenerateTerrain()
    {
        TileClass tile;
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * GetCurrentBiome(x, heightAddition).terrainFreq, seed * GetCurrentBiome(x, heightAddition).terrainFreq) * GetCurrentBiome(x, heightAddition).heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
               

                
                if(y < height - dirtLayerHeight)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.stone;
                } else if (y <  height - 1)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.dirt;
                } else
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.grass;
                    TreeCooldown -= 1;
                }

                /*if (y < height - dirtLayerHeight)
                {

                    if (GetCurrentBiome(x, y).ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y <= GetCurrentBiome(x, y).ores[0].maxSpawnHeight)
                        tile = GetCurrentBiome(x, y).tileAtlas.coal;
                    else if (GetCurrentBiome(x, y).ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y <= GetCurrentBiome(x, y).ores[1].maxSpawnHeight)
                        tile = GetCurrentBiome(x, y).tileAtlas.copper;
                    else
                        tile = GetCurrentBiome(x, y).tileAtlas.stone;

                }
                else if (y < height - 1) 
                {
                    if (GetCurrentBiome(x,y).tileAtlas.snow != null)
                    {
                        if (GetCurrentBiome(x, y).ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y <= GetCurrentBiome(x, y).ores[2].maxSpawnHeight)
                            tile = GetCurrentBiome(x, y).tileAtlas.snow;
                        else
                            tile = GetCurrentBiome(x, y).tileAtlas.dirt;
                    } else
                    {
                        tile = GetCurrentBiome(x, y).tileAtlas.dirt;
                    }
                    
                } else
                {
                    if (GetCurrentBiome(x, y).tileAtlas.snow != null)
                    {
                        if (GetCurrentBiome(x, y).ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y <= GetCurrentBiome(x, y).ores[2].maxSpawnHeight)
                            tile = GetCurrentBiome(x, y).tileAtlas.snow;
                        else
                            tile = GetCurrentBiome(x, y).tileAtlas.grass;
                    }
                    else
                    {
                        tile = GetCurrentBiome(x, y).tileAtlas.grass;
                    }
                    TreeCooldown -= 1;
                }*/
                
                if (GetCurrentBiome(x, y).caveNoiseTexture.GetPixel(x, y).r > GetCurrentBiome(x, y).surfaceValue)
                {
                    
                    PlaceTile(tile, x, y, null);
                    
                    

                    if (y >= height - 1)
                    {
                        int t = Random.Range(0, GetCurrentBiome(x, y).treeChance);
                        if (t == 1)
                        {
                            if (TreeCooldown <= 0)
                            {

                                GenerateTree(x, y + 1, GetCurrentBiome(x, y));
                                TreeCooldown = 6;

                            }

                        }
                    }
                }

                
                
            }
        }
    }

    private void GenerateTree(int x, int y, BiomeClass biomeClass)
    {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(biomeClass.tileAtlas.log, x, y + i, worldTreeMap);

            if (i > 1)
            {
                if (Random.Range(0, 5) <= 1)
                {
                    PlaceTile(biomeClass.tileAtlas.branch, x - 1, y + i, worldTreeMap);
                }
                if (Random.Range(0, 5) <= 1)
                {
                    PlaceTile(biomeClass.tileAtlas.branch, x + 1, y + i, worldTreeMap);
                }
            }
        }

        if(!worldTiles.ContainsKey(new Vector2(x - 1, y)))
        {

            PlaceTile(biomeClass.tileAtlas.root, x -1 , y, worldTreeMap);
        }

        if (!worldTiles.ContainsKey(new Vector2(x + 1, y)))
        {
            PlaceTile(biomeClass.tileAtlas.root, x + 1, y, worldTreeMap);
        }

        PlaceTile(biomeClass.tileAtlas.leaf, x - 2, y + treeHeight, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x - 2, y + treeHeight + 1, worldTreeMap);

        PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 1, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 2, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 3, worldTreeMap);

        PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 1, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 2, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 3, worldTreeMap);

        PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 1, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 2, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 3, worldTreeMap);

        PlaceTile(biomeClass.tileAtlas.leaf, x + 2, y + treeHeight, worldTreeMap);
        PlaceTile(biomeClass.tileAtlas.leaf, x + 2, y + treeHeight + 1, worldTreeMap);
    }


    public void GenerateWalls()
    {
        TileClass tile;
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * GetCurrentBiome(x, heightAddition).terrainFreq, seed * GetCurrentBiome(x, heightAddition).terrainFreq) * GetCurrentBiome(x, heightAddition).heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {



                if (y < height - dirtLayerHeight)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.stonew;
                }
                else if (y < height - 1)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.dirtw;
                }
                else
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.grassw;
                }

                PlaceWall(tile, x, y, null);
            }
        }
    }


    

    public void PlaceTile(TileClass Tile, int x, int y, Tilemap Parent)
    {
        if (worldTiles.ContainsKey(new Vector2(x, y)))
        {
            Debug.Log("World contains tile");
            return;
        } else
        {
            if (Tile != null)
            {

                if (!worldTiles.ContainsKey(new Vector2(x, y)))
                {
                    if (Parent == null)
                    {
                        worldTileMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                        worldTiles.Add(new Vector2(x, y), Tile);
                    }
                    else
                    {
                        Parent.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                        worldTiles.Add(new Vector2(x, y), Tile);
                    }



                }
            }  
            
        }

    }

    public void PlaceWall(TileClass Tile, int x, int y, GameObject Parent, bool Autumn = false)
    {
        if (worldWalls.ContainsKey(new Vector2(x, y)))
        {
            Debug.Log("World contains wall");
            return;
        }
        else
        {
            
            if (!worldTiles.ContainsKey(new Vector2(x, y)))
            {

                worldWallMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                worldWalls.Add(new Vector2(x, y), Tile);


            }

        }

    }
    private void GenerateNoiseTexture(float frequency, float limit, Texture2D noiseTexture)
    {


        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
            }
        }
        noiseTexture.Apply();
    }

    public void GenerateExtras()
    {
        PlaceTile(tileAtlas.red, -3, heightAddition, null);
        PlaceTile(tileAtlas.orange, -3, heightAddition + 1, null);
        PlaceTile(tileAtlas.yellow, -3, heightAddition + 2, null);
        PlaceTile(tileAtlas.green, -3, heightAddition + 3, null);
        PlaceTile(tileAtlas.blue, -3, heightAddition + 4, null);
        PlaceTile(tileAtlas.purple, -3, heightAddition + 5, null);
        PlaceTile(tileAtlas.OakPlanks, -3, heightAddition + 6, null);
        PlaceTile(tileAtlas.OakPlatform, -3, heightAddition + 7, worldSemiSolidMap);
    }
}
