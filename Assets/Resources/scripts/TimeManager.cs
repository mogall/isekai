using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
	public bool timeEnabled = true;
	public Light sun;
	public float realTimeSecondsDayLength = 120f;
	public int currentDay = 0;
	[Range(0,1)]
	public float currentTimeOfDay = 0.4f;
	public int currentHour;
	public int currentMinute;
	public float timeScale = 1;


	void Update(){
		if (timeEnabled) {
			currentTimeOfDay += (Time.deltaTime / realTimeSecondsDayLength) * timeScale;
			if (currentTimeOfDay > 1) {
				currentTimeOfDay -= 1;
				currentDay++;
			}
			UpdateSun ();
			SetClock ();
			//print ("time is " + currentHour + ":" + currentMinute);
		}
	}

	void UpdateSun(){ //TODO - apply light intensity here and maybe a color ramp for the sky/light?
		sun.transform.localRotation = Quaternion.Euler ((currentTimeOfDay * 360) - 90, -50, 0);
	}
	void SetClock(){
		float hourUnfloored = 24 * currentTimeOfDay;
		currentHour = Mathf.FloorToInt(hourUnfloored);
		currentMinute = Mathf.FloorToInt(60 * (hourUnfloored - currentHour));
	}
}