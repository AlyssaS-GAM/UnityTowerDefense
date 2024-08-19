using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;

    // display current currency for shop
    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }
}
