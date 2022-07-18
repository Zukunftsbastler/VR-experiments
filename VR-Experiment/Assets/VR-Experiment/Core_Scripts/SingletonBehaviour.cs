using UnityEngine;

namespace VR_Experiment.Core
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

                return _instance;
            }
        }

        public static bool HasInstance => _instance != null;

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this as T;
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }
    }
}
