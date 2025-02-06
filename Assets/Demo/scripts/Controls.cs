using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(GBA_Component))]
public class Controls : MonoBehaviour
{
    private GBA_Component gba;
    public InputAsset input;
    public short keys { get; private set; }

    private void Awake()
    {
        input = new InputAsset();
    }
    private void Start()
    {
        gba = GetComponent<GBA_Component>();  
        
        
    }
    private void OnEnable()
    {
        input.Enable();
        input.controls.gba.performed+= OnKeyboardButtonPressed;
    }

    public void OnKeyboardButtonPressed(InputAction.CallbackContext context)
    {
        
        var control = context.control;
        var value = context.ReadValue<float>();
        if (control.device is Keyboard)
        {
            string controlPath = control.path;
            // Map controlPath to KeyCode
            if (controlPath == "/Keyboard/r")
            {
                keys = 0;
                gba.resetEmulator();
                return;
            }
                short key = MapControlPathToKeyCode(controlPath);
            if(value >= 0.5)
            {
                keys |= key;
            }
            else
            {
                keys &= (short)~key;
            }
           
        }
        gba.PushKeys(keys);
    }

    private short MapControlPathToKeyCode(string controlPath)
    {
       //TODO add all necessary keyboard keys
        switch (controlPath)
        {
            case "/Keyboard/w":
                return (short)GBA_Component.KEYS.UP;
            case "/Keyboard/a":
                return (short)GBA_Component.KEYS.LEFT;
            case "/Keyboard/s":
                return (short)GBA_Component.KEYS.DOWN; 
            case "/Keyboard/d":
                return (short)GBA_Component.KEYS.RIGHT;
            case "/Keyboard/j":
                return (short)GBA_Component.KEYS.A;
            case "/Keyboard/k":
                return (short)GBA_Component.KEYS.B;
            case "/Keyboard/u":
                return (short)GBA_Component.KEYS.L;
            case "/Keyboard/i":
                return (short)GBA_Component.KEYS.R;
            case "/Keyboard/v":
                return (short)GBA_Component.KEYS.START;
            case "/Keyboard/b":
                return (short)GBA_Component.KEYS.SELECT;
           

            // Add more mappings as needed
            default:
                return 0;
        }
    }

    private void OnDisable()
    {
        input.Disable();
        input.controls.gba.performed -= OnKeyboardButtonPressed;
    }

    private void Update()
    {
        gba.PushKeys(keys);
    }
}
