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
        FireWeapon.OnDamageTaken += HandleHealthUpdate;
        _currentHealth = int.Parse(_healthBarText.text);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        FireWeapon.OnDamageTaken -= HandleHealthUpdate;
    }

    #endregion

    #region Private Methods
    private void HandleHealthUpdate(int damage)
    {
        _healthBarText.text = (_currentHealth - damage).ToString();
    }
    #endregion

}
