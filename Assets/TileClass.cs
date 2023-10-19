using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public Sprite tileSprite;
    public RuleTile ruleTile;
    public bool AutoTile = false;
    [Header("Siblings")]
    public List<string> siblings = new List<string>();
}

