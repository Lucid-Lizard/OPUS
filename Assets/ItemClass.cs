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
    public int DropChance = 0;
    public bool CanBreak;
}
