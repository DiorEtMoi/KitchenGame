using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player LocalInstance { get; private set; }
    public static event EventHandler OnAnyPickUpSomeThing;
    public event EventHandler<OnSelectedCounterChangedEventArg> OnSelectedCounterChanged;
    public event EventHandler OnPickUpSomething;
    public static event EventHandler OnAnySpawnPlayer;
    private PlayerVisuals visuals;
    public class OnSelectedCounterChangedEventArg : EventArgs
    {
        public BaseCounter counter;
    }
    public static void ResetStatic()
    {
        OnAnySpawnPlayer = null;
        OnAnyPickUpSomeThing = null;
    }

    [SerializeField] private float speed;
    [SerializeField] private LayerMask counterMask;

    private Vector2 movermentInput;
    private CharacterController _characterController;
    private Vector3 moveDir;
    private Vector3 lastInteract;
    private BaseCounter selectedConter;

    private bool isWalking;

    private KitchenObject kitchenObject;
    [SerializeField]
    private Transform topPosition;

    public void Start()
    {
        GameInput.instance.OnInteractCounter += GameInput_OnInteractCounter;
        GameInput.instance.OnInteractCounterAlternate += GameInput_OnInteractCounterAlternate;

        PlayerData playerData = KitchenObjectNetworkManager.instance.GetPlayerDataFromClientId(OwnerClientId);
        visuals.SetColor(
            KitchenObjectNetworkManager.instance.GetPlayerColor(playerData.colorId)
            );

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        }
    }

    private void Singleton_OnClientDisconnectCallback(ulong clientID)
    {
        if(clientID == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(getKitchenObject());
        }
    }

    private void GameInput_OnInteractCounterAlternate(object sender, EventArgs e)
    {
        if (!KitchenGameManager.instance.isStartGame())
        {
            return;
        }
        if (selectedConter != null)
        {
            selectedConter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractCounter(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.instance.isStartGame())
        {
            return;
        }
       if(selectedConter != null)
        {
            selectedConter.Interact(this);
        }
    }

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        visuals = GetComponent<PlayerVisuals>();
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
            Debug.Log(LocalInstance);
           
        }
        OnAnySpawnPlayer?.Invoke(this, EventArgs.Empty);
    }

    public void ProcessMove()
    {
        movermentInput = GameInput.instance.GetMovermentInput();
        isWalking = moveDir.magnitude > 0;
        moveDir.x = movermentInput.x;
        moveDir.z = movermentInput.y;
        _characterController.Move(moveDir * Time.fixedDeltaTime * speed);
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * speed);
    }
    public void HandleInteraction()
    {
        float distance = 2f;
        if(moveDir != Vector3.zero)
        {
            lastInteract = moveDir;
        }
        if(Physics.Raycast(transform.position, lastInteract, out RaycastHit raycastHit, distance, counterMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                setSelectedCounter(counter);

            }
            else
            {
                setSelectedCounter(null);

            }
        }
        else
        {
            setSelectedCounter(null);
        }
    }
    public void FixedUpdate()
    {
        if (IsOwner)
        {
            ProcessMove();
            HandleInteraction();
        }
    }
   
    public bool IsPlayerWalking()
    {
        return isWalking;
    }
    public void setSelectedCounter(BaseCounter counter)
    {
        this.selectedConter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArg
        {
            counter = counter
        });
    }

    public KitchenObject getKitchenObject()
    {
        return kitchenObject;
    }

    public Transform getTopPosition()
    {
        return topPosition;
    }

    public void setKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickUpSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickUpSomeThing?.Invoke(this, EventArgs.Empty);
        }
    }

    public void clearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
