using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public Tilemap worldTileMap;
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
    private BiomeClass curBiome;


    void Start()
    {
       
        seed = Random.Range(-10000, 10000);
        DrawTextures();

        CreateChunks();
        GenerateTerrain();

        
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
                    tile = tileAtlas.stone;
                } else
                {
                    tile = tileAtlas.dirt;
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
                    
                    

                    /*if (y >= height - 1)
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
                    }*/
                }

                
                
            }
        }
    }


    /*void GenerateTree(int x, int y, BiomeClass biomeType)
    {
        int treeHeight = Random.Range(biomeType.minTreeHeight, biomeType.maxTreeHeight);

        GameObject TreeParent = new GameObject();

        Vector2[] GrasslandTops = new Vector2[]
        {
            new Vector2(x - 2, y + treeHeight),
            new Vector2(x - 2, y + treeHeight + 1),

            new Vector2(x - 1, y + treeHeight),
            new Vector2(x - 1, y + treeHeight + 1),
            new Vector2(x - 1, y + treeHeight + 2),
            new Vector2(x - 1, y + treeHeight + 3),

            new Vector2(x, y + treeHeight),
            new Vector2(x, y + treeHeight + 1),
            new Vector2(x, y + treeHeight + 2),
            new Vector2(x, y + treeHeight + 3),

            new Vector2(x + 1, y + treeHeight),
            new Vector2(x + 1, y + treeHeight + 1),
            new Vector2(x + 1, y + treeHeight + 2),
            new Vector2(x + 1, y + treeHeight + 3),

            new Vector2(x + 2, y + treeHeight),
            new Vector2(x + 2, y + treeHeight + 1)
        };

        float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize);
        chunkCoord /= chunkSize;
        TreeParent.transform.parent = worldChunks[(int)chunkCoord].transform;
        TreeParent.name = biomeType.biomeName + " Tree";
        TreeParent.transform.position = new Vector2(x, y);

        Debug.Log("GenTree: " + treeHeight);
        if (biomeType.biomeName == "Grassland" || biomeType.biomeName == "Forest" || biomeType.biomeName == "Desert")
        {
            Debug.Log("GenTree: Biome is " + biomeType.biomeName) ;
            for (int i = 0; i < treeHeight; i++)
            {
                Debug.Log("GenTree: About to log");
                PlaceTile(biomeType.tileAtlas.log, x, y + i, TreeParent);
                Debug.Log("GenTree: did log");
            }
        }
        else if (biomeType.biomeName == "Tundra")
        {

            PlaceTile(biomeType.tileAtlas.log, x, y, TreeParent);
            PlaceTile(biomeType.tileAtlas.log, x, y+2, TreeParent);
            PlaceTile(biomeType.tileAtlas.log, x, y+4, TreeParent);

        } 
        
        if (biomeType.biomeName == "Grassland" || biomeType.biomeName == "Forest")
        {
            bool AutumnShould;
            float AutumnChance = Random.Range(0, 3);
            Debug.Log(AutumnChance);
            if (AutumnChance >= 1)
            {
                
                AutumnShould = true;
            } else
            {
                AutumnShould = false;
            }
            foreach (Vector2 position in GrasslandTops)
            {
                
                 PlaceTile(biomeType.tileAtlas.leaf, (int)position.x, (int)position.y, TreeParent, AutumnShould);
                
                
            }
        } else if (biomeType.biomeName == "Tundra")
        {
            PlaceTile(biomeType.tileAtlas.leaf, x - 2, y + 1, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x - 1, y + 1, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x, y + 1, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x + 1, y + 1, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x + 2, y + 1, TreeParent);


            PlaceTile(biomeType.tileAtlas.leaf, x - 1, y + 3, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x, y + 3, TreeParent);
            PlaceTile(biomeType.tileAtlas.leaf, x + 1, y + 3, TreeParent);

            PlaceTile(biomeType.tileAtlas.leaf, x, y + 5, TreeParent);
            

        }
        
    }*/

    public void PlaceTile(TileClass Tile, int x, int y, GameObject Parent, bool Autumn = false)
    {
        if (worldTiles.ContainsKey(new Vector2(x,y)))
        {
            Debug.Log("World contains tile");
            return;
        } else
        {
            /*GameObject newTile = new GameObject();

            if(Parent == null)
            {
                float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize);
                chunkCoord /= chunkSize;
                newTile.transform.parent = worldChunks[(int)chunkCoord].transform;
            } else
            {
                newTile.transform.parent = Parent.transform;
            }

            newTile.AddComponent<SpriteRenderer>();
            
            newTile.name = Tile.name;
            newTile.transform.position = new Vector2(x, y);
            if (Tile.AutoTile)
            {
                newTile.AddComponent<AutoTiling>();
                newTile.GetComponent<AutoTiling>().TileType = Tile.name;
                newTile.GetComponent<AutoTiling>().Tile = newTile;
                newTile.GetComponent<AutoTiling>().AddSiblings(Tile.siblings);
                newTile.GetComponent<AutoTiling>().worldTiles = worldTiles;
                newTile.GetComponent<AutoTiling>().randomTileSprite = Random.Range(0, 3);
                if (Autumn)
                {
                    newTile.GetComponent<AutoTiling>().Autumn = true;
                }
                
            }
            else
            {
                newTile.GetComponent<SpriteRenderer>().sprite = Tile.tileSprite;
            }*/

            if (!worldTiles.ContainsKey(new Vector2(x, y)))
            {

                worldTileMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                worldTiles.Add(new Vector2(x,y), Tile);

                
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
}
