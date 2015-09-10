using UnityEngine;
using System.Collections;

public class lineRenderer : MonoBehaviour {
	public GameObject targetStart;
	public GameObject targetEnd;


	// Use this for initialization
	void Start () {
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		Vector3 posStart = new Vector3(targetStart.transform.position.x, targetStart.transform.position.y, targetStart.transform.position.z);
		Vector3 posEnd = new Vector3(targetEnd.transform.position.x, targetEnd.transform.position.y, targetEnd.transform.position.z);
		lineRenderer.SetPosition(0, posStart);
		lineRenderer.SetPosition(1, posEnd);
	
	}
}

