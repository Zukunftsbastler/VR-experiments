using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_TourData))]
public class SO_TourData_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SO_TourData tourData = (SO_TourData)target;
        if(GUILayout.Button("Update TourData"))
        {
            string resourcePath = Application.dataPath + "/VR-Experiment/360°/Resources";
            string[] directories = Directory.GetDirectories(resourcePath, "Tour_*", SearchOption.TopDirectoryOnly);

            tourData.tourData = new List<TourData>();

            foreach(string directory in directories)
            {
                string folderName = directory.Substring(resourcePath.Length + 1);
                SO_DisplayData[] displayData = Resources.LoadAll(folderName + "/DisplayData", typeof(SO_DisplayData)).Cast<SO_DisplayData>().ToArray();
                SO_HotspotData[] hotspotData = Resources.LoadAll(folderName + "/HotspotData", typeof(SO_HotspotData)).Cast<SO_HotspotData>().ToArray();

                tourData.tourData.Add(new TourData(folderName, displayData, hotspotData));
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(tourData);
#endif
        }
    }
}
