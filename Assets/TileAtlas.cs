using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    [Header("Environment")]
    public TileClass dirt;
    public TileClass dirtw;
    public TileClass grass;
    public TileClass grassw;
    public TileClass stone;
    public TileClass stonew;
    public TileClass log;
    public TileClass root;
    public TileClass branch;
    public TileClass leaf;
    public TileClass snow;
    public TileClass sand;

    [Header("Ores")]
    public TileClass coal;
    public TileClass copper;
}
