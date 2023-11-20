using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private ContainerCounter container;
    private Animator animator;

    private void Awake()
    {
        container = GetComponentInParent<ContainerCounter>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        container.OnGrabContainer += Container_OnGrabContainer;
    }

    private void Container_OnGrabContainer(object sender, System.EventArgs e)
    {
        animator.SetTrigger("OpenClose");
    }
}
