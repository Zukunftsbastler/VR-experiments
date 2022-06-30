using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VR_Experiment.Networking
{
    [DefaultExecutionOrder(-100)]
    public class CustomPhotonTypeHandler : MonoBehaviour
    {
        public static bool _hasRegisteredCustonTypes = false;

        private void Awake()
        {
            if(_hasRegisteredCustonTypes)
                return;

            Register<VoiceChatSettings>(100);

            _hasRegisteredCustonTypes = true;
        }

        private void Register<T>(byte code) where T : ICustomPhotonType<T>, new()
        {
            bool result = PhotonPeer.RegisterType(typeof(T), code, Serialize, Deserialize);
            if(result == false)
            {
                Debug.LogError($"{gameObject.name} failed to register custom Photon type '{typeof(T)}'.", this);
            }

            byte[] Serialize(object customType)
            {
                T obj = (T)customType;
                using(MemoryStream m = new MemoryStream())
                {
                    using(BinaryWriter writer = new BinaryWriter(m))
                    {
                        obj.Write(writer);
                    }

                    return m.ToArray();
                }
            }

            object Deserialize(byte[] data)
            {
                T obj = new T();
                using(MemoryStream m = new MemoryStream(data))
                {
                    using(BinaryReader reader = new BinaryReader(m))
                    {
                        obj.Read(reader);
                    }

                    return obj;
                }
            }
        }
    }
}
