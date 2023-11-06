using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text partyName;
    public TMP_Text locationName;
    void Start()
    {
        partyName.text = GameManager.Instance.GameState.PlayerParty.PartyName;
    }

    void Update()
    {
        if (GameManager.Instance.GameState.SelectedElectorate != null) 
        {
            locationName.text = GameManager.Instance.GameState.SelectedElectorate.name;
        }
        else
        {
            locationName.text = "Nationwide";
        }
    }
}
