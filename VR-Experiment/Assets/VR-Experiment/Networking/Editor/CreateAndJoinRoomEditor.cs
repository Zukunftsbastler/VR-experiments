using UnityEditor;
using UnityEngine;
using VR_Experiment.Core;

namespace VR_Experiment.Networking
{
    [CustomEditor(typeof(CreateAndJoinRoom))]
    public class CreateAndJoinRoomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CreateAndJoinRoom roomJoin = (CreateAndJoinRoom)target;
            if(EditorApplication.isPlaying)
            {
                if(GUILayout.Button("Editor Connect"))
                {
                    PlayerWrapper.Instance.SetRole(Role.Attendee);
                    PlayerWrapper.Instance.SetAvatar(Resources.Load<GameObject>("Alpha_Avatar_1"), true);
                    roomJoin.JoinRoom();
                }
            }
        }
    }
}
