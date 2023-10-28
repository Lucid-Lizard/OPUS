using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public Texture2D StructureText;
    public Tilemap worldTileMap;
    public Tilemap worldWallMap;
    public Tilemap worldTreeMap;
    public Tilemap worldSemiSolidMap;
    [Header("Biome Stuff")]
    public BiomeClass[] biomes;
    public BiomeClass defaultBiome;

    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;

    [Header("Trees")]
    public int treeChance = 10;
    public int minTreeHeight = 4;
    public int maxTreeHeight = 6;
    public int TreeCooldown = 0;



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
    public IDictionary<Vector2, TileClass> worldTiles;
    public IDictionary<Vector2, TileClass> worldWalls;
    private BiomeClass curBiome;


    void Start()
    {
        IDictionary<Vector2, TileClass> worldTiles = GameManager.Instance.tileEditManager.worldTiles;
        IDictionary<Vector2, TileClass> worldWalls = GameManager.Instance.tileEditManager.worldWalls;
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
        BiomeClass SelectedBiome;
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                SelectedBiome = defaultBiome;
                foreach (BiomeClass Biome in biomes)
                {
                    if (x >= (worldSize * (Biome.startGenX / 100)) && x <= (worldSize * (Biome.endGenX / 100)))
                    {
                        if (y >= (worldSize * (Biome.startGenY / 100)) && y <= (worldSize * (Biome.endGenY / 100)))
                        {
                            SelectedBiome = Biome;
                            break;
                        }
                    }
                }
                //float v = Mathf.PerlinNoise((x + seed + Mathf.Sin(y) * 0.5f) * biomeFrequency, seed * biomeFrequency);
                //Color col = biomeGradient.Evaluate(v);
                biomeMap.SetPixel(x, y, SelectedBiome.biomeColor);


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

        return defaultBiome;
    }
    public void GenerateTerrain()
    {
        TileClass tile;
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * GetCurrentBiome(x, heightAddition).terrainFreq, seed * GetCurrentBiome(x, heightAddition).terrainFreq) * GetCurrentBiome(x, heightAddition).heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {



                if (y < height - dirtLayerHeight)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.stone;
                }
                else if (y < height - 1)
                {
                    tile = GetCurrentBiome(x, y).tileAtlas.dirt;
                }
                else
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

                    //PlaceTile(tile, x, y, null);
                    GameManager.Instance.tileEditManager.PlaceTile(tile, x, y);



                    if (y >= height - 1)
                    {
                        int t = Random.Range(0, GetCurrentBiome(x, y).treeChance);
                        if (t == 1)
                        {
                            if (TreeCooldown <= 0)
                            {
                                if (GetCurrentBiome(x, y).GenTrees)
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
    }

    private void GenerateTree(int x, int y, BiomeClass biomeClass)
    {

        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);
        List<Vector2> tileToTrack = new List<Vector2>();

        GameObject newTree = new GameObject();
        newTree.name = biomeClass.biomeName + " Tree";
        
        newTree.transform.position = new Vector3(x, y, 0);

        for (int i = 0; i < treeHeight; i++)
        {
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.log, x, y + i);
            tileToTrack.Add( new Vector2(x, y + i)); 
            

            if (i > 1)
            {
                if (Random.Range(0, 5) <= 1)
                {
                    GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.branch, x - 1, y + i);
                    //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y + i));
                    tileToTrack.Add(new Vector2(x -1 , y + i));
                }
                if (Random.Range(0, 5) <= 1)
                {
                    GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.branch, x + 1, y + i);
                    //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x + 1, y + i));
                    tileToTrack.Add(new Vector2(x + 1, y + i));
                }
            }
        }
        if (biomeClass.roots)
        {
            if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(x - 1, y)))
            {

                GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.root, x - 1, y);
                //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y));
                tileToTrack.Add(new Vector2(x - 1, y));
            }

            if (!GameManager.Instance.tileEditManager.worldTiles.ContainsKey(new Vector2(x + 1, y)))
            {
                GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.root, x + 1, y);
                //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x + 1, y));
                tileToTrack.Add(new Vector2(x + 1, y));
            }
        }

        if (biomeClass.tops)
        {
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 2, y + treeHeight);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 2, y + treeHeight));
            tileToTrack.Add(new Vector2(x - 2, y + treeHeight));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 2, y + treeHeight + 1);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 2, y + treeHeight + 1));
            tileToTrack.Add(new Vector2(x - 2, y + treeHeight + 1));

            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y + treeHeight));
            tileToTrack.Add(new Vector2(x - 1, y + treeHeight));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 1);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y + treeHeight + 1));
            tileToTrack.Add(new Vector2(x - 1, y + treeHeight + 1));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 2);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y + treeHeight + 2));
            tileToTrack.Add(new Vector2(x - 1, y + treeHeight + 2));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x - 1, y + treeHeight + 3);
            // newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x - 1, y + treeHeight + 3));
            tileToTrack.Add(new Vector2(x - 1, y + treeHeight + 3));

            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight);
            //  newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x, y + treeHeight));
            tileToTrack.Add(new Vector2(x, y + treeHeight));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 1);
            //  newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x, y + treeHeight + 1));
            tileToTrack.Add(new Vector2(x, y + treeHeight + 1));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 2);
            //  newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x, y + treeHeight + 2));
            tileToTrack.Add(new Vector2(x, y + treeHeight + 2));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x, y + treeHeight + 3);
            //  newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x , y + treeHeight + 3));
            tileToTrack.Add(new Vector2(x, y + treeHeight + 3));

            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight);
            // newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x +1, y + treeHeight));
            tileToTrack.Add(new Vector2(x + 1, y + treeHeight));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 1);
            // newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x +1, y + treeHeight + 1));
            tileToTrack.Add(new Vector2(x + 1, y + treeHeight + 1));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 2);
            // newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x +1, y + treeHeight + 2));
            tileToTrack.Add(new Vector2(x + 1, y + treeHeight + 2));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 1, y + treeHeight + 3);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x +1, y + treeHeight + 3));
            tileToTrack.Add(new Vector2(x + 1, y + treeHeight + 3));

            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 2, y + treeHeight);
            // newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x + 2, y + treeHeight));
            tileToTrack.Add(new Vector2(x + 2, y + treeHeight));
            GameManager.Instance.tileEditManager.PlaceTile(biomeClass.tileAtlas.leaf, x + 2, y + treeHeight + 1);
            //newTree.GetComponent<TreeHandler>().trackedTiles.Append<Vector2>(new Vector2(x + 2, y + treeHeight + 1));
            tileToTrack.Add(new Vector2(x + 2, y + treeHeight + 1));
        }



        /*newTree.AddComponent<TreeHandler>();
        newTree.GetComponent<TreeHandler>().origin = new Vector2(x, y);
        newTree.GetComponent<TreeHandler>().trackedTiles = tileToTrack ;
        newTree.GetComponent<TreeHandler>().startTrack = true;
        newTree.GetComponent<TreeHandler>().This = newTree;*/


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

    public void PlaceWall(TileClass Tile, int x, int y, GameObject Parent, bool Autumn = false)
    {
        if (GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(x, y)))
        {
            Debug.Log("World contains wall");
            return;
        }
        else
        {

            if (!GameManager.Instance.tileEditManager.worldWalls.ContainsKey(new Vector2(x, y)))
            {

                GameManager.Instance.tileEditManager.wallMap.SetTile(new Vector3Int(x, y, 0), Tile.ruleTile);
                GameManager.Instance.tileEditManager.worldWalls.Add(new Vector2(x, y), Tile);


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
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.red, -3, heightAddition);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.orange, -3, heightAddition + 1);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.yellow, -3, heightAddition + 2);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.green, -3, heightAddition + 3);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.blue, -3, heightAddition + 4);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.purple, -3, heightAddition + 5);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.OakPlanks, -3, heightAddition + 6);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.OakPlatform, -3, heightAddition + 7);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.stonew, -4, heightAddition);
        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.sand, -4, heightAddition + 1);

        GameManager.Instance.tileEditManager.PlaceTile(tileAtlas.Furnace, -5, heightAddition);
    }

    

    public Texture2D GenerateStructureTexture(int radius,int numWhitePixels, Texture2D texture2D)

    {
        texture2D = new Texture2D(worldSize, worldSize);

        // Set the background color to black
        Color[] pixels = new Color[worldSize * worldSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
        }

        // Generate random white pixels
        for (int i = 0; i < numWhitePixels; i++)
        {
            Vector2Int randomPos = RandomPositionWithinRadius(worldSize, radius);
            int index = randomPos.x + randomPos.y * worldSize;
            pixels[index] = Color.white;
        }

        // Apply the pixel data to the texture
        texture2D.SetPixels(pixels);
        texture2D.Apply();
        return texture2D;
    }

    Vector2Int RandomPositionWithinRadius(int textureSize, int radius)
    {
        Vector2Int randomPos;
        do
        {
            randomPos = new Vector2Int(Random.Range(0, textureSize), Random.Range(0, textureSize));
        } while (Vector2Int.Distance(randomPos, new Vector2Int(textureSize / 2, textureSize / 2)) < radius);
        return randomPos;
    }
}
