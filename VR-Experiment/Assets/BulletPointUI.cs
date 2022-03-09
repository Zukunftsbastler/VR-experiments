using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPointUI : MonoBehaviour
{
    private const string SPOILER_PREVENTION = "[...]";

    [SerializeField] private Text _textfield;
    [SerializeField] private ContentSizeFitter _textfieldFitter;
    [SerializeField] private Image _bulletPoint;

    public string Text { get; set; }

    public void UpdateVisuals()
    {
        int siblingIndex = transform.GetSiblingIndex() - 1;

        gameObject.SetActive(siblingIndex < 3);

        if(gameObject.activeSelf)
        {
            //Set text
            if(siblingIndex == 0)
            {
                _textfield.text = Text;
            }
            else if(siblingIndex == 1)
            {
                _textfield.text = SPOILER_PREVENTION;
            }
            else if(siblingIndex > 1)
            {
                _textfield.text = "";
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_textfield.rectTransform);

            //Set transparency
            float alphaValue = Mathf.Max(0f, 1f - (siblingIndex / 3.0f));
            _textfield.color = new Color(_textfield.color.r, _textfield.color.b, _textfield.color.b, alphaValue);
            _bulletPoint.color = new Color(_bulletPoint.color.r, _bulletPoint.color.g, _bulletPoint.color.b, alphaValue);

            //Set height
            float height = _textfield.rectTransform.rect.height;
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(height));
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private IEnumerator SetHeightDelayed()
    {
        _textfieldFitter.SetLayoutVertical();
               
        yield return new WaitForEndOfFrame();        

        float height = _textfield.rectTransform.rect.height;
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(height));

    }
}
