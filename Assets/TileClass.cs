using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public Sprite tileSprite;
    public Texture2D tileTexture;
    public bool AutoTile = false;
    [Header("Sublings")]
    public List<string> siblings = new List<string>();
}

