using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostPathRecorder : MonoBehaviour {

	public float sampleRate = 0.25f;
	public bool isRecording = false;

	private List<Vector3> recordedPositions;
	public List<Quaternion> recordedRotations;
	private Transform thisTransform;
	private float timer;

	void Start () {
		recordedPositions = new List<Vector3>();
		recordedRotations = new List<Quaternion>();
		thisTransform = GetComponent<Transform>();

		// TODO Get sampleRate from external script
	}

	void Update () {
		if( isRecording) {
			if( timer >= sampleRate ) {
				// Record position and rotation
				recordedPositions.Add( thisTransform.position );
				recordedRotations.Add( thisTransform.rotation );

				// Add the left over faction of time to the timer after reset
				timer = timer % sampleRate;
			}

			timer += Time.deltaTime;
		}
	}

	public void CheckIfUploadData() {
		// TODO Get data from database
	}

	private void ExportRecordedData() {

	}

	public void StartRecording() {
		isRecording = true;
		recordedPositions.Add( thisTransform.position );
		recordedRotations.Add( thisTransform.rotation );
	}

	public void StopRecording() {
		isRecording = false;
	}
}
