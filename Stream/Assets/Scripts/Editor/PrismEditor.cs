using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Prism))]
public class PrismEditor : Editor {
	private Prism _prism;
	private SerializedProperty _layerMesh;
	private SerializedProperty _layers;
	private bool _layersFoldout;

	void OnEnable() {
		_prism = target as Prism;
		_layerMesh = serializedObject.FindProperty("layerMesh");
		_layers = serializedObject.FindProperty("layers");
	}

	void OnSceneGUI() {
		var interpolation = 10;
		var dt = 1f / interpolation;
		var splines = _prism.UpdateSplines();
		foreach (var spl in splines) {
			var vertices = new Vector3[(spl.counter - 1) * interpolation + 1];
			var counter = 0;
			var iLastLayer = spl.counter - 1;
			for (var iLayer = 0; iLayer < iLastLayer; iLayer++) {
				for (var i = 0; i < interpolation; i++) {
					var p = spl.Interpolate(iLayer, dt * i);
					vertices[counter++] = p;
				}
			}
			var lastP = spl.Interpolate(iLastLayer - 1, 1f);
			vertices[counter++] = lastP;
			Handles.DrawPolyLine(vertices);
		}
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.PropertyField(_layerMesh);
		_layersFoldout = EditorGUILayout.Foldout(_layersFoldout, "Layers");
		if (_layersFoldout) {
			_layers.arraySize = EditorGUILayout.IntField("Size", _layers.arraySize);
			for (var i = 0; i < _layers.arraySize; i++) {
				EditorGUILayout.PropertyField(_layers.GetArrayElementAtIndex(i));
			}
		}

		if (EditorGUI.EndChangeCheck()) {
			serializedObject.ApplyModifiedProperties();
		}
	}
}
