using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioReferenceSO : ScriptableObject
{
    public AudioClip[] chops;
    public AudioClip[] deliveryFails;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip stovePickUp;
    public AudioClip[] trash;
    public AudioClip[] warning;

}
