using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Prism : MonoBehaviour {
	public GameObject[] layers;
	public Mesh layerMesh;
	public Spline[] splines;

	public Spline[] UpdateSplines() {
		var nVertices = layerMesh.vertexCount;
		splines = new Spline[nVertices];
		for (var i = 0; i < nVertices; i++)
			splines[i] = new Spline();
		foreach (var layer in layers) {
			var tr = layer.transform;
			for (var i = 0; i < nVertices; i++) {
				var p = layerMesh.vertices[i];
				p = tr.TransformPoint(p);
				p = transform.InverseTransformPoint(p);
				splines[i].Add(p);
			}
		}
		return splines;
	}

	public Mesh UpdateMesh() {
		var interpolation = 10;
        
		var dt = 1f / interpolation;
		var vertices = new List<Vector3>();
		var segments = new List<int>();
		for (var iSpline = 0; iSpline < splines.Length; iSpline++) {
			var spline = splines[iSpline];
			for (var iLayer = 0; iLayer < (layers.Length - 1); iLayer++) {
				for (var i = 0; i < interpolation; i++) {
					var iVertex = vertices.Count;
					var p = spline.Interpolate(iLayer, dt * i);
					vertices.Add(p);
					segments.Add(iVertex);
					segments.Add(iVertex + 1);
				}
			}
			var pTip = spline.Interpolate(layers.Length - 2, 1f);
			vertices.Add(pTip);
		}

		var mf = GetComponent<MeshFilter>();
		var mesh = mf.sharedMesh == null ? mf.sharedMesh = new Mesh() : mf.sharedMesh;
		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.SetIndices(segments.ToArray(), MeshTopology.Lines, 0);
		mesh.bounds = new Bounds(Vector3.zero, 1e6f * Vector3.one);
		return mesh;
	}
}
