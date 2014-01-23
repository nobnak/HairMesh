using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace nobnak.Geometory.HairMesh {

	[ExecuteInEditMode]
	public class Prism : MonoBehaviour {
		public int resolution = 10;
		public GameObject[] hairLayers;
		public Mesh layerMesh;
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
			if (resolution <= 0)
				resolution = 1;

			if (hairLayers.Length >= 2) {
				if (drawCorners) {
					Handles.color = Color.black;
					var vertices = layerMesh.vertices;
					for (var i = 0; i < vertices.Length; i++) {
						var p = vertices[i];
						var spl = GetSplineInWorld(p);
						DrawSplineInScene (spl);
					}
				}
			}
			if (Selection.activeGameObject == this.gameObject) {
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

		public Spline GetSplineInWorld(Vector3 p) {
			var spline = new Spline();
			foreach (var face in hairLayers) {
				var tr = face.transform;
				var pWorld = tr.TransformPoint(p);
				spline.Add(pWorld);
			}
			return spline;
		}
		public Spline GetSplineInLocal(Vector3 p) {
			var spline = new Spline();
			foreach (var face in hairLayers) {
				var tr = face.transform;
				var pWorld = tr.TransformPoint(p);
				var pLocal = transform.InverseTransformPoint(pWorld);
				spline.Add(pLocal);
			}
			return spline;
		}

		public void DrawSplineInScene (Spline spl) {
			var dt = 1f / resolution;
			var vertices = spl.Discretize(resolution);
			Handles.DrawPolyLine(vertices);
		}
	}

}