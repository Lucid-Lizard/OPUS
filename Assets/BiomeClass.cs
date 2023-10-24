using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BiomeClass", menuName = "Biome Class")]

public class BiomeClass : ScriptableObject
{
    public string biomeName;
    
    public Color biomeColor;
   
    public TileAtlas tileAtlas;

    [Header("Noise Settings")]
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public Texture2D caveNoiseTexture;
    public int dirtLayerHeight = 15;

    [Header("Generation Settings")]
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;
    public float startGenX;
    public float endGenX;
    public float startGenY;
    public float endGenY;



    [Header("Foliage")]
    public Sprite[] foliageSprites;
    public int foliageChance = 6;


    [Header("Trees")]
    public int treeChance = 10;
    public int minTreeHeight = 4;
    public int maxTreeHeight = 6;

    [Header("Ore Settings")]
    public OreClass[] ores;

}
