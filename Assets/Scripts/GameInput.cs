using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }
    private InputSystem inputSystem;
    public event EventHandler OnInteractCounter;
    public event EventHandler OnInteractCounterAlternate;
    public event EventHandler OnPauseGame;

    private void Awake()
    {
        instance = this;
        inputSystem = new InputSystem();
        inputSystem.PlayerInput.Enable();
        inputSystem.PlayerInput.Interaction.performed += Interaction_performed;
        inputSystem.PlayerInput.InteractionAlternate.performed += InteractionAlternate_performed;
        inputSystem.PlayerInput.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void InteractionAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractCounterAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractCounter?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovermentInput()
    {
        return inputSystem.PlayerInput.Moverment.ReadValue<Vector2>().normalized;
    }
    public void OnDestroy()
    {
        inputSystem.PlayerInput.Interaction.performed -= Interaction_performed;
        inputSystem.PlayerInput.InteractionAlternate.performed -= InteractionAlternate_performed;
        inputSystem.PlayerInput.Pause.performed -= Pause_performed;
        inputSystem.Dispose();  
    }
}
