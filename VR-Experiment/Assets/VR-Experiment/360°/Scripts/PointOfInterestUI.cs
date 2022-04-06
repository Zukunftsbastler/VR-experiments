using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointOfInterestUI : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private TourManager _tourManager;
    [Space]
    [SerializeField] private Button _hotspot;
    [SerializeField] private Button _direction;
    [Space]
    [SerializeField] private InputActionProperty _closeInput;

    private Vector3 _activePosition;

    private void Start()
    {
        _hotspot.onClick.AddListener(OnAddHotspotClicked);
        _direction.onClick.AddListener(OnAddDirectionClicked);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _closeInput.action.performed += OnCloseInputPerformed;
    }

    private void OnDisable()
    {
        _closeInput.action.performed -= OnCloseInputPerformed;
    }

    public void ToggleUI(Vector3 position)
    {
        _activePosition = position;
        Vector3 direction = _origin.position - position;

        transform.position = position + direction * 0.1f;
        transform.LookAt(position);

        gameObject.SetActive(true);
    }

    private void OnAddHotspotClicked()
    {
        gameObject.SetActive(false);
        Debug.LogWarning("Adding Hotspots has not been implemented jet.");
    }

    private void OnAddDirectionClicked()
    {
        gameObject.SetActive(false);
        _tourManager.AddDirection(_activePosition);
    }

    private void OnCloseInputPerformed(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }
}
