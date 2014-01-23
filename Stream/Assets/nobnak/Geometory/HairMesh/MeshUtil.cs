using UnityEngine;

namespace nobnak.Geometory.HairMesh {

	public static class MeshUtil {
		public static Vector3 Sample(this Mesh m) {
			var vertices = m.vertices;
			var triangles = m.triangles;
			var i = 3 * Random.Range(0, triangles.Length / 3);
			var v0 = vertices[triangles[i]];
			var v1 = vertices[triangles[i + 1]];
			var v2 = vertices[triangles[i + 2]];

			float u, v;
			SampleUV(out u, out v);
			var w = 1f - (u + v);

			return u * v1 + v * v2 + w * v0; 
		}

		public static void SampleUV(out float u, out float v) {
			u = Random.value;
			v = Random.value;
			if ((u + v) > 1f) {
				u = 1f - u;
				v = 1f - v;
			}
		}
	}

}