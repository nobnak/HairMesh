using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Prism))]
public class PrismEditor : Editor {
	private Prism _prism;
	private SerializedProperty _layerMesh;
	private SerializedProperty _layers;

	void OnEnable() {
		_prism = target as Prism;
		_layerMesh = serializedObject.FindProperty("layerMesh");
		_layers = serializedObject.FindProperty("layers");
	}

	void OnSceneGUI() {

	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.PropertyField(_layerMesh);
		EditorGUILayout.PropertyField(_layers);

		if (EditorGUI.EndChangeCheck()) {
			serializedObject.ApplyModifiedProperties();
		}
	}
}
