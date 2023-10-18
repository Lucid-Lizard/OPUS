using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public RuleTile RuleTile;
    public Sprite tileSprite;
<<<<<<< Updated upstream
    public bool AutoTile = false;
    [Header("Siblings")]
    public List<string> siblings = new List<string>();
=======
>>>>>>> Stashed changes
}

