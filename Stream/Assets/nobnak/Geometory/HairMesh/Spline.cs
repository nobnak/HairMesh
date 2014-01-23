using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace nobnak.Geometory.HairMesh {

	public class Spline {
		public int counter = 0;
		public readonly List<Vector3> points = new List<Vector3>();
		public readonly List<Vector3> tangents = new List<Vector3>();

		public void Add(Vector3 pnew) {
			counter++;
			points.Add(pnew);
			tangents.Add(Vector3.zero);

			if (counter <= 2)
				return;
			tangents[counter - 2] = CalculateTangent(counter - 2);
		}

		public Vector3 Interpolate(float t) {
			var i = (int) t;
			t -= i;
			return Interpolate(i, t);
		}
		public Vector3 Interpolate(int i, float t) {
			if (i < 0) {
				i = 0;
				t = 0f;
			} else if ((i + 1) >= counter) {
				i = points.Count - 2;
				t = 1f;
			}
			t = t > 1f ? 1f : (t < 0f ? 0f : t);

			var t2 = t * t;
			var t3 = t * t2;
			var p = (2f * t3 - 3f * t2 + 1f) * points[i] + (t3 - 2f * t2 + t) * tangents[i] + (-2f * t3 + 3f * t2) * points[i+1] + (t3 - t2) * tangents[i + 1];
			return p;
		}

		public Vector3 CalculateTangent(int i) {
			return 0.5f * (points[i+1] - points[i-1]);
		}

		public Vector3[] Discretize(int resolution) {
			var dt = 1f / resolution;
			var vertices = new Vector3[(counter - 1) * resolution + 1];
			var vertexCounter = 0;
			var iLastLayer = counter - 1;
			for (var iLayer = 0; iLayer < iLastLayer; iLayer++) {
				for (var i = 0; i < resolution; i++) {
					var p = Interpolate(iLayer, dt * i);
					vertices[vertexCounter++] = p;
				}
			}
			var lastP = Interpolate (iLastLayer - 1, 1f);
			vertices [vertexCounter++] = lastP;
			return vertices;
		}
		
	}

}