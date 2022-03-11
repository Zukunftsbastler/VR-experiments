using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateAndJoinRoom))]
public class CreateAndJoinRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateAndJoinRoom roomJoin = (CreateAndJoinRoom)target;
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Editor Connect"))
            {
                PlayerWrapper.Instance.SetRole(VR_Experiment.Enums.Role.Visitor);
                PlayerWrapper.Instance.SetAvatar(Resources.Load<GameObject>("Alpha_Avatar_1"), true);
                roomJoin.JoinRoom();
            }
        }
    }
}
