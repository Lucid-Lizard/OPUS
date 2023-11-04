using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    [Header("Tile Things")]
    public string tileName;
    public Sprite tileSprite;
    public RuleTile ruleTile;
    [Header("Loot Table")]
    public ItemClass[] Items;
    public int[] ItemChance;
    [Header("Breakability Stuff")]
    public string TypeToBreak = "Pickaxe";
    public float Toughness = 1.0f;
    public int LvlToBreak = 1;
    [Header("Tile Type")]
    public bool Semisolid = false;
    public bool Slope = false;
    public bool Wall = false;
    public bool Rooted = false;
    public bool StartTree = false;
    public bool tree = false;
    [Header("Multitile")]
    public ItemClass tileItem;
    public bool multiTile = false;
    public TileBase[] multiTiles;
    public Vector2 multiTileSize = new Vector2(1, 1);
    [Header("Crafting")]
    public bool Crafting;
    public CraftingType craftingType;
    
    public enum CraftingType
    {
        Crafting,
        Furnace
    }
}

