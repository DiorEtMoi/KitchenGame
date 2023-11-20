using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footTimer;
    private float footTimeMax  = .1f;
    private void Awake()
    {
        player = GetComponent<Player>(); 
    }
    private void Update()
    {
        footTimer -= Time.deltaTime;
        if(footTimer < 0) 
        {
           if(player.IsPlayerWalking())
            {
                footTimer = footTimeMax;
                SoundManager.instance.PlaySoundFootStep(player.transform.position, .5f);
            }
        }
    }
}
