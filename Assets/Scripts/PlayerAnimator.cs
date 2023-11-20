using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING = "isWalking";

    private Animator _animator;
    private Player _player;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }
    public void Update()
    {
        if (IsOwner)
        {
            _animator.SetBool(IS_WALKING, _player.IsPlayerWalking());
        }
    }
}
