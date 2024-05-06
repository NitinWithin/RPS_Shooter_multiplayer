using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGameObject : MonoBehaviour
{
    #region Variables

    #endregion

    #region Default Unity Methods

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Team Manager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    #region Private Methods

    #endregion

    #region Public methods

    #endregion

    #region PUN callbacks

    #endregion

    #region Playfab callbacks

    #endregion
}
