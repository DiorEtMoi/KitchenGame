using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryCounterVisual : MonoBehaviour
{
     private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveGameobject;
    [SerializeField] private GameObject particleGameObject;

    private void Awake()
    {
        stoveCounter = GetComponentInParent<StoveCounter>();
        stoveGameobject = transform.Find("SizzlingParticles").gameObject;
        particleGameObject = transform.Find("StoveOnVisual").gameObject;
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateEvent e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Cooked;
        stoveGameobject.SetActive(showVisual);
        particleGameObject.SetActive(showVisual);
    }
}
