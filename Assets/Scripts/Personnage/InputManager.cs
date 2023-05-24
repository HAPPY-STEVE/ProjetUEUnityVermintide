using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset inputActionMap;
    private FirstPersonController fpc;

    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActionMap.actionMaps[0].name);
        fpc = FindObjectOfType<FirstPersonController>();
    }


    public void ToggleActionMap(string actionMap)
    {
        inputActionMap.FindActionMap("Player").Disable();
        inputActionMap.FindActionMap("UI").Disable();
        inputActionMap.FindActionMap(actionMap).Enable();
        foreach (InputActionMap a in inputActionMap.actionMaps)
        {
            Debug.Log(a.name + " " + a.enabled);
        }
    }

    public void setFirstPersonController(bool value)
    {
        fpc.enabled = value;
        fpc.gameObject.GetComponent<PlayerInput>().enabled = value; 
    }
}
