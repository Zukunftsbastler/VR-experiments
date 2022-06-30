using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VR_Experiment.Networking
{
    public class VoiceChatSettings : ICustomPhotonType<VoiceChatSettings>
    {
        public float volume;
        public float pitch;
        public float spatialBlend;
        public float distance;

        public void Read(BinaryReader reader)
        {
            volume = reader.ReadSingle();
            pitch = reader.ReadSingle();
            spatialBlend = reader.ReadSingle();
            distance = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(volume);
            writer.Write(pitch);
            writer.Write(spatialBlend);
            writer.Write(distance);
        }
    }
}
