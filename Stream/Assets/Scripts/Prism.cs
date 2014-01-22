using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class Prism : MonoBehaviour {
	public int interpolation = 10;
	public GameObject[] hairLayers;
	public Mesh layerMesh;
	public Material layerMaterial;
	public bool drawCorners;

	void OnEnable() {
#if UNITY_EDITOR
		SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
	}

	void OnDisable() {
#if UNITY_EDITOR
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
	}
	
	void OnSceneGUI(SceneView sceneView) {
		if (layerMesh == null)
			return;
		if (interpolation <= 0)
			interpolation = 1;

		if (hairLayers.Length >= 2) {
			if (drawCorners) {
				Handles.color = Color.black;
				var vertices = layerMesh.vertices;
				for (var i = 0; i < vertices.Length; i++) {
					var p = vertices[i];
					var spl = GetSpline(p);
					DrawSplineInScene (spl);
				}
			}
		}
		if (layerMaterial != null && Selection.activeGameObject == this.gameObject) {
			for (var i = 0; i < hairLayers.Length; i++) {
				var tr = hairLayers[i].transform;
				switch (Tools.current) {
				case Tool.Move:
					tr.position = Handles.PositionHandle(tr.position, tr.rotation);
					break;
				case Tool.Rotate:
					tr.rotation = Handles.RotationHandle(tr.rotation, tr.position);
					break;
				case Tool.Scale:
					tr.localScale = Handles.ScaleHandle(tr.localScale, tr.position, tr.rotation, 5f);
					break;
				}
			}
		}
	}

	public Spline GetSpline(Vector3 p) {
		var spline = new Spline();
		foreach (var face in hairLayers) {
			var tr = face.transform;
			var pWorld = tr.TransformPoint(p);
			spline.Add(pWorld);
		}
		return spline;
	}
	
	public void DrawSplineInScene (Spline spl) {
		var dt = 1f / interpolation;
		var vertices = new Vector3[(spl.counter - 1) * interpolation + 1];
		var counter = 0;
		var iLastLayer = spl.counter - 1;
		for (var iLayer = 0; iLayer < iLastLayer; iLayer++) {
			for (var i = 0; i < interpolation; i++) {
				var p = spl.Interpolate(iLayer, dt * i);
				vertices[counter++] = p;
			}
		}
		var lastP = spl.Interpolate (iLastLayer - 1, 1f);
		vertices [counter++] = lastP;
		Handles.DrawPolyLine(vertices);
	}
}
