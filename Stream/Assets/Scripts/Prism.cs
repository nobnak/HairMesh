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
				splines[i].Add(p);
			}
		}
		return splines;
	}
}
