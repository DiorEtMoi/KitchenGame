using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private void Awake()
    {
        stoveCounter = GetComponentInParent<StoveCounter>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateEvent e)
    {
        bool isState = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Cooked;
        if (isState)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
