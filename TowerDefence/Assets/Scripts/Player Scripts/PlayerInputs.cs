using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private PlayerActionsAsset _PlayerActionAsset;
    public static PlayerInputs Instance;

    private void Awake()
    {
        _PlayerActionAsset = new PlayerActionsAsset();
        if (Instance == null)
            Instance = this;
    }
    private void OnEnable()
    {
        _PlayerActionAsset.Enable();
    }
    private void OnDisable()
    {
        _PlayerActionAsset.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _PlayerActionAsset.Player.Move.ReadValue<Vector2>();
    }
    
    public bool IsShooting()
    {
        return _PlayerActionAsset.Player.Fire.triggered;
    }

   /* public float SwitchingWeapons()
    {
        return _PlayerActionAsset.Player.WeaponChange.ReadValue<float>();
    }*/
   /* public bool PlayerJumpedThisFrame()
    {
        return _PlayerActionAsset.Player.Jump.triggered;
    }
    public bool ISSpriting()
    {
        return _PlayerActionAsset.Player.Sprint.IsPressed();
    }
    

    public float GetMouseScroll()
    {
        return _PlayerActionAsset.Player.WeaponChange.ReadValue<float>();
    }

    public bool IsCouching()
    {
        return _PlayerActionAsset.Player.Crouch.IsPressed();
    }

    internal bool GetAimButton()
    {
        return _PlayerActionAsset.Player.Aim.IsPressed();
    }*/
}
