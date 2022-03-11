using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    [SerializeField] private Image _avatarPreview;
    [SerializeField] private Text _title;
    [SerializeField] private Text _message;
    [Space]
    [SerializeField] private float _upTime;

    private void Start()
    {
        StartCoroutine(DestroyDelayed());
    }

    private IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(_upTime);

        Destroy(gameObject);
    }

    public void DisplayMessage(Sprite avatarPreview, string title, string message)
    {
        _avatarPreview.sprite = avatarPreview;
        _title.text = title;
        _message.text = message;
    }
}
