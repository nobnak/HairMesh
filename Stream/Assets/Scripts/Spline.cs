using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (t < 0) {
			i = 0;
			t = 0f;
		} else if ((counter - 1) <= t) {
			i = counter - 2;
			t = 1f;
		}
		t -= i;
		return Interpolate(i, t);
	}
	public Vector3 Interpolate(int i, float t) {
		var t2 = t * t;
		var t3 = t * t2;
		var p = (2f * t3 - 3f * t2 + 1f) * points[i] + (t3 - 2f * t2 + t) * tangents[i] + (-2f * t3 + 3f * t2) * points[i+1] + (t3 - t2) * tangents[i + 1];
		return p;
	}

	public Vector3 CalculateTangent(int i) {
		return 0.5f * (points[i+1] - points[i-1]);
	}
}
