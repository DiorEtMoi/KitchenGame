using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe SO")]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> recipes;
    public string RecipeName;
}
