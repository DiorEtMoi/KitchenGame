using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticEvent : MonoBehaviour
{
    public void Awake()
    {
        CuttingCounter.ResetStatic();
        BaseCounter.ResetStatic();
        TrashCounter.ResetStatic();
        Player.ResetStatic();
    }
}
