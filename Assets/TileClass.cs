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
    public bool Semisolid = false;
    public bool Slope = false;
    public bool Wall = false;
    public bool Tree = false;
    [Header("Siblings")]
    public List<string> siblings = new List<string>();
}

