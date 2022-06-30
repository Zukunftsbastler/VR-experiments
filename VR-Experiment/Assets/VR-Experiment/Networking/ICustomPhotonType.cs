using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VR_Experiment.Networking
{
    public interface ICustomPhotonType<T>where T : ICustomPhotonType<T>, new()
    {
        /// <summary>
		/// Read all field values from the <see cref="BinaryReader"/>.
		/// Be sure to match the order of <see cref="Write(BinaryWriter)"/>.
		/// </summary>
        public void Read(BinaryReader reader);
        /// <summary>
		/// Write all field values to the <see cref="BinaryWriter"/>.
		/// Be sure to match the order of <see cref="Read(BinaryReader)"/>.
		/// </summary>
        public void Write(BinaryWriter writer);
    }
}
