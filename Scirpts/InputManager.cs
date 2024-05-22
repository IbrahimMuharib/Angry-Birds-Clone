using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    private InputAction mousePositionAction;
    private InputAction mouseAction;

    public static Vector2 MousePosition;
    public static bool WasLeftMouseButtonPressed;
    public static bool WasLeftMouseButtonReleased;
    public static bool IsLeftMouseButtonPressed;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        mousePositionAction = playerInput.actions["MousePosition"];
        mouseAction = playerInput.actions["Mouse"];
    }

    void Update()
    {
        MousePosition = mousePositionAction.ReadValue<Vector2>();

        WasLeftMouseButtonPressed = mouseAction.WasPressedThisFrame();
        WasLeftMouseButtonReleased = mouseAction.WasReleasedThisFrame();
        IsLeftMouseButtonPressed = mouseAction.IsPressed();
    }

}
