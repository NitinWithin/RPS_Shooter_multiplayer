using UnityEngine;

public class UIToggle : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject toggleObject;
    [SerializeField] private GameObject[] toggleObjects;
    [SerializeField] private GameObject[] toggleOffObjects;
    [SerializeField] private GameObject[] toggleOnObjects;
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
        if (toggleOffObjects == null) return;

        foreach (GameObject obj in toggleOffObjects)
        {
            if(obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }

    public void ToggleOnObejcts()
    {
        if (toggleOnObjects == null) return;

        foreach (GameObject obj in toggleOnObjects)
        {
            if(!obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
    }

    #endregion
}
