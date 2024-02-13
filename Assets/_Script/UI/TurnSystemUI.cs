using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnButton;
    [SerializeField] TextMeshProUGUI turnNumberText;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() => 
        {
            TurnSystem.Instance.NextTurn();
        });    
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }
}