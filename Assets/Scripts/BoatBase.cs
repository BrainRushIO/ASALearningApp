﻿using UnityEngine;
using System.Collections;

public class BoatBase : MonoBehaviour {


	public SkinnedMeshRenderer blendShape;
	protected float lerpTimer, lerpDuration=1f, blendFloatValue, angleWRTWind, lastAngleWRTWind;
	public bool rotateMast = false;
	protected bool isJibing = false;
	public GameObject mast;
	protected Quaternion lerpStart, lerpEnd;
	protected Vector3 directionWindComingFrom = new Vector3(0f,0f,1f);
	protected Vector3 boatDirection;

	protected void MastRotation() {
		//handles sail blend shape, jibes, and mast rotation

		lastAngleWRTWind = angleWRTWind;
		directionWindComingFrom = WindManager.s_instance.directionOfWind;
		
		boatDirection = transform.forward;
		angleWRTWind = Vector3.Angle(boatDirection,directionWindComingFrom);
		blendFloatValue = 100;
		if (transform.rotation.eulerAngles.y > 180f ) {
			angleWRTWind = 360-angleWRTWind;
			blendFloatValue = 0;
		}
		
		blendShape.SetBlendShapeWeight(0,blendFloatValue);

		if (float.IsNaN(angleWRTWind)) {
			angleWRTWind=0;
		}
		if ((angleWRTWind >= 180 && lastAngleWRTWind <= 180 
		     && angleWRTWind <190)) {
			if (lastAngleWRTWind!=0){
				Jibe (-1f);
			}
		}
		if(angleWRTWind <= 180 && lastAngleWRTWind >= 180
		   && angleWRTWind > 170) {
			if (lastAngleWRTWind!=0){
				Jibe (1f);
			}
		}

		if (!isJibing) {

//			get the boats z rotation and as a constant value for the start and end quaternions of the lerp to influence the lerp
			mast.transform.localRotation = Quaternion.Lerp (Quaternion.identity, Quaternion.Inverse(transform.localRotation), 0.5f);
		
		}
		
		else if (isJibing) {
			float percentageLerp = (Time.time - lerpTimer)/lerpDuration;
			mast.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, percentageLerp);
			if (percentageLerp > .98) {
				mast.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, 1);
				isJibing = false;
			}
			
		}

		//TODO replace euler angle conditions with WRT to wind conditions in the case of variable wind
		if (transform.rotation.eulerAngles.y  > 355f) {
			blendFloatValue = 50 - (360f-transform.rotation.eulerAngles.y ) * 10f;
			blendShape.SetBlendShapeWeight(0,blendFloatValue);
		}
		else if (transform.rotation.eulerAngles.y  < 5f) {
			blendFloatValue = (transform.rotation.eulerAngles.y) * 10f + 50f;
			blendShape.SetBlendShapeWeight(0,blendFloatValue);
			
		}
		

	}

	void POSMastRotation() {

		//handles sail blend shape, jibes, and mast rotation
		print (angleWRTWind + " ANGLE WRT WIND");
		if (angleWRTWind > 182) {
			lastAngleWRTWind = angleWRTWind;
		} else if (angleWRTWind < 178) {
			lastAngleWRTWind = angleWRTWind;
		}
		directionWindComingFrom = WindManager.s_instance.directionOfWind;
		
		boatDirection = transform.forward;
		angleWRTWind = Vector3.Angle(boatDirection,directionWindComingFrom);
		blendFloatValue = 100;
		if (transform.rotation.eulerAngles.y > 180f ) {
			angleWRTWind = 360-angleWRTWind;
			blendFloatValue = 0;
		}
		
		if (float.IsNaN(angleWRTWind)) {
			angleWRTWind=0;
		}
	
		
	
		//get the boats z rotation and as a constant value for the start and end quaternions of the lerp to influence the lerp
//		if (angleWRTWind > 224f || angleWRTWind < 136) {
//			mast.transform.localRotation = Quaternion.Lerp (Quaternion.identity, Quaternion.Inverse(transform.localRotation), 0.25f);
//
//		}

		if(angleWRTWind > 182 || angleWRTWind < 178) {
			mast.transform.localRotation = Quaternion.Lerp (Quaternion.identity, Quaternion.Inverse(transform.localRotation), 0.5f);
		}

//		if (angleWRTWind ==180 && GameManager.s_instance.hasClickedRun) {
//			GetComponent<Animator>().enabled = false;
//			print(lastAngleWRTWind);
//
//			if (lastAngleWRTWind > 180) {
//				blendShape.SetBlendShapeWeight(0,0f);
//			}
//			else if (lastAngleWRTWind < 180) {
//				blendShape.SetBlendShapeWeight(0,100f);
//
//			}
//		}
			
			
	}

	void Update() {
		if (rotateMast) {
			POSMastRotation();
		}
	}

	protected void Jibe(float negative) {
		isJibing = true;
		lerpTimer = Time.time;
		lerpStart = Quaternion.Inverse(mast.transform.localRotation);
		lerpEnd = Quaternion.Inverse(mast.transform.localRotation * Quaternion.Inverse(Quaternion.Euler(0,negative*180f,0)));
		
	}
}
