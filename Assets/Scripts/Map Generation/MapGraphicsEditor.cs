using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGraphics))]
public class MapGraphicsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGraphics mg = (MapGraphics)target;
        if (GUILayout.Button("Generate"))
        {
            while(mg.transform.childCount != 0)
            {
                GameObject.DestroyImmediate(mg.transform.GetChild(0).gameObject);
            }
            mg.BuildMap();
        }
    }

}
