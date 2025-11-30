using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Ingredient
{
    public string itemName;
    public int amount;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "SPU/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public List<Ingredient> ingredients;
    public Ingredient result;
}