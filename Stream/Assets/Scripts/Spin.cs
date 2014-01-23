using UnityEngine;
using System.Collections;
using nobnak.Geometory.HairMesh;

[RequireComponent(typeof(Prism))]
public class Spin : MonoBehaviour {
	public Vector3 maxRotation = new Vector3(0f, 0f, 180f);
	public float duration = 3f;

	private Prism _prism;

	// Use this for initialization
	void Start () {
		_prism = GetComponent<Prism>();
	}
	
	// Update is called once per frame
	void Update () {
		var t = Mathf.PingPong(Time.timeSinceLevelLoad, duration) / duration;
		var rotation = t * maxRotation;
		var hairLayers = _prism.hairLayers;
		var dr = rotation / (hairLayers.Length - 1);
		for (var i = 0; i < hairLayers.Length; i++) {
			var hairLayer = hairLayers[i];
			hairLayer.transform.localRotation = Quaternion.Euler(i * dr);
		}
	}
}
