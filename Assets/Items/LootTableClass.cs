using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "lootTableClass", menuName = "Loot Table")]
public class LootTableClass : ScriptableObject
{
    public ItemClass[] Items;
    public int[] ItemChance;
}


