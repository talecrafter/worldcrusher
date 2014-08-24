using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(NodeDisplay))]
public class NodeDisplayEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUILayout.Space(10.0f);

		if (GUILayout.Button("Log Connections")) {
			(target as NodeDisplay).node.LogConnections();
		}
		if (GUILayout.Button("Set to Player"))
		{
			(target as NodeDisplay).node.SetFaction(FactionType.Player);
		}
		if (GUILayout.Button("Set to Enemy"))
		{
			(target as NodeDisplay).node.SetFaction(FactionType.Enemy);
		}
	}


}