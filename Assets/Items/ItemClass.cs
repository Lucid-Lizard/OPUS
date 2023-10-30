using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemClass", menuName = "Item Class")]
public class ItemClass : ScriptableObject
{
    [Header("Item Stuff")]
    public string ItemName;
    public Sprite ItemSprite;
    public bool Placeable;
    public TileClass ItemTile;
    public int MaxStackSize = 99;
    public int DropChance = 0;
    public bool CanBreak;
    public string BreakType = null;
    public int BreakSpeed = 60;
    public int BreakLevel = 1;
    public Vector2 ItemScaleInHand = new Vector2(.05f,.05f);
}
