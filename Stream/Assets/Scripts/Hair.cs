using UnityEngine;
using System.Collections.Generic;
using nobnak.Geometory.HairMesh;

[ExecuteInEditMode]
[RequireComponent(typeof(Prism))]
[RequireComponent(typeof(MeshFilter))]
public class Hair : MonoBehaviour {
	public int nSamples = 10;

	private Prism _prism;
	private Mesh _lineMesh;
	private Vector3[] _roots;
	
	void OnEnable() {
		_prism = GetComponent<Prism>();
		_lineMesh = new Mesh();
		_roots = new Vector3[nSamples];

		var mf = GetComponent<MeshFilter>();
		mf.sharedMesh = _lineMesh;

		var m = _prism.layerMesh;
		for (var i = 0; i < nSamples; i++)
			_roots[i] = m.Sample();
	}

	void OnDisable() {
		DestroyObject(_lineMesh);
	}
	
	void Update() {
		var splResolution = 10;
		var verticesSet = new List<Vector3[]>();
		var vertexCounter = 0;
		foreach (var p in _roots) {
			var spl = _prism.GetSplineInLocal(p);
			var splVertices = spl.Discretize(splResolution);
			verticesSet.Add(splVertices);
			vertexCounter += splVertices.Length;
		}

		var vertices = new Vector3[vertexCounter];
		var indices = new int[2 * (vertexCounter - verticesSet.Count)];

		vertexCounter = 0;
		var indicesCounter = 0;
		foreach (var splVertices in verticesSet) {
			for (var i = 1; i < splVertices.Length; i++) {
				indices[indicesCounter++] = vertexCounter + i - 1;
				indices[indicesCounter++] = vertexCounter + i;
			}
			System.Array.Copy(splVertices, 0, vertices, vertexCounter, splVertices.Length);
			vertexCounter += splVertices.Length;
		}

		_lineMesh.Clear();
		_lineMesh.vertices = vertices;
		_lineMesh.SetIndices(indices, MeshTopology.Lines, 0);
		_lineMesh.RecalculateBounds();
	}
}
