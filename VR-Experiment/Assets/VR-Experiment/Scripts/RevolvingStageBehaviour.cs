using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingStageBehaviour : MonoBehaviour
{
    [Tooltip("Distance local to this transform.")]
    [SerializeField] private float _hoverHeight;
    [Tooltip("Distance the product hoves up and down relative to the hover height.")]
    [SerializeField] private float _hoverAmplitude;
    [Tooltip("One cycle (up, down up) equals 360. \nSpeed in cycles per second.")]
    [SerializeField] private float _hoverSpeed;
    [Tooltip("One full rotation equals 360. \nSpeed in rotation per seconds.")]
    [SerializeField] private float _rotationSpeed;
    
    private GameObject _activeProduct;
    private float _hoverFrequenz;

    public bool HasActiveItem => _activeProduct != null;
    public GameObject ActiveProduct {
        get 
        {
            return _activeProduct;
        } 
        
        set 
        {
            _activeProduct = value;
            //_activeProduct.transform.parent = transform;
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
            _activeProduct.transform.position = transform.position + Vector3.up * hoverHeight;
        }
    }
}
