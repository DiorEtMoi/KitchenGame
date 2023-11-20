using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO platesSO;
    private PlatesCounter platesCounter;

    private List<Transform> platesList = new List<Transform>();
    private void Awake()
    {
        platesCounter = GetComponentInParent<PlatesCounter>();
    }
    private void Start()
    {
        platesCounter.OnSpawnPlated += PlatesCounter_OnSpawnPlated;
        platesCounter.OnTakeAPlated += PlatesCounter_OnTakeAPlated;
    }

    private void PlatesCounter_OnTakeAPlated(object sender, System.EventArgs e)
    {
        Transform topObject = platesList[platesList.Count - 1];
        platesList.Remove(topObject);
        Destroy(topObject.gameObject);
    }

    private void PlatesCounter_OnSpawnPlated(object sender, System.EventArgs e)
    {
        Transform platesTransform = Instantiate(platesSO.perfap, platesCounter.getTopPosition());

        float offsetY = 0.1f;
        platesTransform.localPosition = new Vector3 (0, offsetY * platesList.Count, 0);
        platesList.Add(platesTransform);
    }
}
