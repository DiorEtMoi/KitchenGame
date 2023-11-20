using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
