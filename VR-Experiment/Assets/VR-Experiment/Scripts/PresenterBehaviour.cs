using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresenterBehaviour : MonoBehaviour
{
    [SerializeField] private float _hoverAmplitude;
    [Tooltip("Distance local to this transform.")]
    [SerializeField] private float _hoverHeight;
    [Tooltip("One cycle (up, down up) equals 360. \nSpeed in cycles per second.")]
    [SerializeField] private float _hoverSpeed;
    [Tooltip("One full rotation equals 360. \nSpeed in rotation per seconds.")]
    [SerializeField] private float _rotationSpeed;

    private GameObject _activeItem;
    private float _hoverFrequenz;

    public bool HasActiveItem => _activeItem != null;
    public GameObject ActiveItem {
        get 
        {
            return _activeItem;
        } 
        
        set 
        {
            _activeItem = value;
            _activeItem.transform.parent = transform;
        } 
    }

    void Update()
    {
        if(HasActiveItem)
        {
            //spin platform
            float yRotation = transform.eulerAngles.y + _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(yRotation, Vector3.up);

            //hover item
            _hoverFrequenz += _hoverSpeed * Mathf.Deg2Rad * Time.deltaTime;
            float hoverHeight = _hoverHeight + _hoverAmplitude * Mathf.Sin(_hoverFrequenz);
            _activeItem.transform.position = transform.position + Vector3.up * hoverHeight;
        }
    }
}
