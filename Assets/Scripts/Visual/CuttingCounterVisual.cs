using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        cuttingCounter = GetComponentInParent<CuttingCounter>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        cuttingCounter.OnCuttingVisual += CuttingCounter_OnCuttingVisual;
    }

    private void CuttingCounter_OnCuttingVisual(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Cut");
    }

   
}
