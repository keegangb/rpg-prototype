using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _player = null;
    [SerializeField] private Image _healthBar;

    private void Update()
    {
        if(_player == null)
        {
            _healthBar.fillAmount = 0f;
        }

        _healthBar.fillAmount = _player.GetComponent<Health>().health/100f;
    }
}
