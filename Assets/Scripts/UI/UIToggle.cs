using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject toggleObject;
    [SerializeField] private GameObject[] toggleObjects;
    #endregion

    #region Public methods
    public void ToggleObject()
    {
        if (toggleObject == null) return;

        toggleObject.SetActive(!toggleObject.activeSelf);
    }

    public void ToggleObjects()
    {
        if (toggleObjects == null) return;

        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void ToggleOffObejcts()
    {
        if (toggleObjects == null) return;

        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ToggleOnObejcts()
    {
        if (toggleObjects == null) return;

        foreach (GameObject obj in toggleObjects)
        {
            obj.SetActive(true);
        }
    }

    #endregion
}
