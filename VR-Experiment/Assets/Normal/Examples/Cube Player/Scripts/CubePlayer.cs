#if NORMCORE

using UnityEngine;

namespace Normal.Realtime.Examples
{
    public class CubePlayer : MonoBehaviour
    {
        public float speed = 5.0f;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        [SerializeField] private XRIDefaultInputActions _playerActions;

        private void Awake()
        {
            _playerActions = new XRIDefaultInputActions();
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void OnEnable()
        {
            _playerActions.Testplayer.Enable();
        }

        private void OnDisable()
        {
            _playerActions.Testplayer.Disable();
        }

        private void Update()
        {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocallySelf)
                return;

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            // Grab the x/y input from WASD / a controller
            //float x = Input.GetAxis("Horizontal");
            //float y = Input.GetAxis("Vertical");
            Vector2 movement = _playerActions.Testplayer.Movement.ReadValue<Vector2>();
            float x = movement.x;
            float y = movement.y;
            // Apply to the transform
            Vector3 localPosition = transform.localPosition;
            localPosition.x += x * speed * Time.deltaTime;
            localPosition.y += y * speed * Time.deltaTime;
            transform.localPosition = localPosition;
        }
    }
}

#endif
