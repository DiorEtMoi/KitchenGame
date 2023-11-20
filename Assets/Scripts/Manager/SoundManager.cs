using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [SerializeField] private AudioReferenceSO audioClipRef;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        DeliveryManager.instance.OnSuccessRecipe += Instance_OnSuccessRecipe;
        DeliveryManager.instance.OnFailedRecipe += Instance_OnFailedRecipe;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnDropKitchenObject += TrashCounter_OnDropKitchenObject;
        BaseCounter.OnPlacedKitchenObject += BaseCounter_OnPlacedKitchenObject;
        Player.OnAnyPickUpSomeThing += Instance_OnPickUpSomething;     
    }

   
    private void BaseCounter_OnPlacedKitchenObject(object sender, System.EventArgs e)
    {
        BaseCounter bas = sender as BaseCounter;
        PlaySound(audioClipRef.objectDrop, bas.transform.position, 1f);
    }

    private void TrashCounter_OnDropKitchenObject(object sender, System.EventArgs e)
    {
        TrashCounter trash = sender as TrashCounter;
        PlaySound(audioClipRef.trash, trash.transform.position, 1f);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cutting = sender as CuttingCounter;
        PlaySound(audioClipRef.chops, cutting.transform.position, 1f);
    }

    private void Instance_OnPickUpSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRef.objectPickUp, player.transform.position, 1f);
    }

    private void Instance_OnFailedRecipe(object sender, System.EventArgs e)
    {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRef.deliveryFails, deliverCounter.transform.position, 1f);

    }

    private void Instance_OnSuccessRecipe(object sender, System.EventArgs e)
    {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;

        PlaySound(audioClipRef.deliverySuccess, deliverCounter.transform.position, 1f);
    }

    private void PlaySound(AudioClip[] clips, Vector3 position, float volume = .5f)
    {
        AudioSource.PlayClipAtPoint(clips[Random.Range(0,clips.Length)],position, volume);
    }
    public void PlaySoundFootStep(Vector3 position,float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipRef.footstep[Random.Range(0, audioClipRef.footstep.Length)], position);
    }
}
