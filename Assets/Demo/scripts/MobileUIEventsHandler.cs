using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleMobileUIEvents : MonoBehaviour
{
    [SerializeField]
    private List<SimpleButton> buttons;
    [SerializeField] 
    private GBA_Component gbaInstance;
    
    private int keys = 0;

    private void OnEnable()
    {
        foreach (var button in buttons)
        {
            button.OnPress += onKeyPressed;
            button.OnRelease += onKeyReleased;
        }
    }

    private void OnDisable()
    {
        foreach (var button in buttons)
        {
            button.OnPress -= onKeyPressed;
            button.OnRelease -= onKeyReleased;
        }
    }

    private void onKeyPressed(GBA_Component.KEYS key)
    {
        keys |= (int)key;
        gbaInstance?.PushKeys(keys);
    }

    private void onKeyReleased(GBA_Component.KEYS key)
    {
        keys &= (short)~key;
        gbaInstance?.PushKeys(keys);

    }
}
