using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newRecipeAtlas", menuName = "Recipe Atlas")]
public class RecipeAtlas : ScriptableObject
{
    public RecipeClass[] Recipes;
}
