using UnityEngine;
using System.Collections;

public class BoatBase : MonoBehaviour {


	public SkinnedMeshRenderer blendShape;
	protected float lerpTimer, lerpDuration=1f, blendFloatValue, angleWRTWind, lastAngleWRTWind;

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
			mast.transform.rotation = Quaternion.Lerp(Quaternion.identity, transform.rotation, 0.5f);
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

	protected void Jibe(float negative) {
		print ("JIBE");
		isJibing = true;
		lerpTimer = Time.time;
		lerpStart = mast.transform.rotation;
		lerpEnd = mast.transform.rotation * Quaternion.Euler(0,negative*180f,0);
		
	}
}
