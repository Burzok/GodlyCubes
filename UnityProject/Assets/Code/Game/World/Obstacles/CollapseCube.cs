using UnityEngine;
using System.Collections;

public class CollapseCube : MonoBehaviour {
	public float timeToDestroy = 10f;

	void Start () {
		float randomSize = Random.Range(0.5f,1.5f);
		Vector3 scale = new Vector3(randomSize,randomSize,randomSize);
		this.transform.localScale = scale;

		Destroy(this.gameObject, timeToDestroy);
	}
}
