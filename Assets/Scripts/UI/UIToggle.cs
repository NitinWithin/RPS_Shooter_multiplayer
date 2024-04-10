using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] _friendlistUI;

    public void ToggleObjects()
    {
        if (_friendlistUI != null)
        {
            foreach(var ui in _friendlistUI)
            {
                ui.SetActive(!ui.activeSelf);
            }
            
        }
    }
}
