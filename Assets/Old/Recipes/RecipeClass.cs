using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "newRecipeClass", menuName = "Recipe Class")]
public class RecipeClass : ScriptableObject
{
    [Header("Crafting Things")]
    public TileClass.CraftingType RecipeType;
    [Header("Ingredients")]
    public ItemClass[] Ingredients;
    public int[] Quants;
    [Header("Result")]
    public ItemClass Result;
    public int ResultQuant;
}

