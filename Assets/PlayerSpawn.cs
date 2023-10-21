using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject Player;
    public int worldSize;
    public int spawnHeight;
    // Start is called before the first frame update
    void Start()
    {
        worldSize = this.GetComponent<TerrainGeneration>().worldSize;
        float spawnX = this.GetComponent<TerrainGeneration>().worldSize / 2;
        Texture2D world = this.GetComponent<TerrainGeneration>().caveNoiseTexture;
        float spawnY = Mathf.PerlinNoise(((int)(worldSize / 2) + this.GetComponent<TerrainGeneration>().seed) * this.GetComponent<TerrainGeneration>().GetCurrentBiome((int)(worldSize/2), this.GetComponent<TerrainGeneration>().heightAddition).terrainFreq, this.GetComponent<TerrainGeneration>().seed * this.GetComponent<TerrainGeneration>().GetCurrentBiome((int)(worldSize / 2), this.GetComponent<TerrainGeneration>().heightAddition).terrainFreq) * this.GetComponent<TerrainGeneration>().GetCurrentBiome((int)(worldSize/2), this.GetComponent<TerrainGeneration>().heightAddition).heightMultiplier + this.GetComponent<TerrainGeneration>().heightAddition;
        spawnY += spawnHeight;
        Player.transform.position = new Vector3(spawnX, spawnY, 0 );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
