using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIVersionUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        _text.text = Application.version;
    }
}
