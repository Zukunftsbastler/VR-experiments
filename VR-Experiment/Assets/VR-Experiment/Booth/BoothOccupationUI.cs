using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class BoothOccupationUI : MonoBehaviour
{
    [SerializeField] private Button _occupy;
    [SerializeField] private Button _leave;
    [Space]
    [SerializeField] private BoothBehaviour _booth;

    private void OnEnable()
    {
        _occupy.onClick.AddListener(OnOccupyClicked);
        _leave.onClick.AddListener(OnLeaveClicked);
    }

    private void OnDisable()
    {
        _occupy.onClick.RemoveListener(OnOccupyClicked);
        _leave.onClick.RemoveListener(OnLeaveClicked);
    }

    public void Initialize(bool authorization)
    {
        _occupy.interactable = authorization;
        _leave.interactable = false;
    }

    public void UpdateButtons(bool occupyIsActive, bool leaveIsActive)
    {
        _occupy.interactable = occupyIsActive;
        _leave.interactable = leaveIsActive;
    }

    private void OnOccupyClicked()
    {
        _booth.Occupy();
    }

    private void OnLeaveClicked()
    {
        _booth.Leave();
    }
}
