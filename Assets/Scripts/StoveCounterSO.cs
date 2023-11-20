using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FryRecipe")]
public class StoveCounterSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float timeToCook;
}
