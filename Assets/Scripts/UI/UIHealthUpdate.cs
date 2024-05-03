using TMPro;
using UnityEngine;

public class UIHealthUpdate : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text _healthBarText;
    private int _currentHealth;

    #endregion

    #region Default Unity Methods
    // Start is called before the first frame update
    void Awake()
    {
        
        _currentHealth = int.Parse(_healthBarText.text);
    }


    #endregion

    #region Private Methods
    public void HandleHealthUpdate(string Health)
    {
        _healthBarText.text = Health;
    }
    #endregion

}
