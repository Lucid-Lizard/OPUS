using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    [Header("Tile Things")]
    public string tileName;
    public Sprite tileSprite;
    public RuleTile ruleTile;
    public ItemClass tileItem;
    [Header("Tile Type")]
    public bool Semisolid = false;
    public bool Slope = false;
    public bool Wall = false;
    public bool Tree = false;

    public bool multiTile = false;
    public Vector2 multiTileSize = new Vector2(1, 1);
}

