using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RDCT_Time : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;

    private void Start()
    {
        TimeManager.onGetStartingTimeMinute += UpdateTime;
        TimeManager.onGetStartingTimeHour += UpdateTime;
    }

    private void Awake()
    {
        TimeManager.onMinuteChange += UpdateTime;
        TimeManager.onHourChange += UpdateTime;
    }
    private void OnDisable()
    {
        TimeManager.onMinuteChange -= UpdateTime;
        TimeManager.onHourChange -= UpdateTime;
    }

    private void UpdateTime()
    {
        TextMeshProUGUI.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
    }
}
