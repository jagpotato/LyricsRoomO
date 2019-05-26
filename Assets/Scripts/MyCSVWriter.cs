using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MyCSVWriter : MonoBehaviour
{
	private bool isPushA;
  private bool isPushB;
  private bool isPushRIndexTrigger;
  private bool isPushRHandTrigger;
	private OculusTouchInputter oculusTouchInputter;
	private Vector3 rightAccel;
	private AudioSource audioSource;
	private float hitAccelThreshold = 10f;

	StreamWriter sw;
	FileInfo fi;

	void Start ()
	{
		fi = new FileInfo("save.csv");
		oculusTouchInputter = GameObject.Find("OculusTouchInputter").GetComponent<OculusTouchInputter>();
		audioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
		save("playbackTime,action");
	}

	void Update ()
	{
		isPushA = oculusTouchInputter.IsPushA;
    isPushB = oculusTouchInputter.IsPushB;
    isPushRIndexTrigger = oculusTouchInputter.IsPushRIndexTrigger;
    isPushRHandTrigger = oculusTouchInputter.IsPushRHandTrigger;

		if (IsJoggle()) {
      rightAccel = oculusTouchInputter.RightAccel;
      if ((rightAccel.x > 12f || rightAccel.y > 12f || rightAccel.z > 12f)) {
        save(audioSource.time + ",joggle");
      }
    }
    if (IsPushUp()) {
      rightAccel = oculusTouchInputter.RightAccel;
      if (rightAccel.y > 13f) {
        save(audioSource.time + ",pushup");
      }
    }
		// if (IsPunch()) {
		// 	rightAccel = oculusTouchInputter.RightAccel;
		// 	if (Mathf.Abs(rightAccel.x) > hitAccelThreshold || Mathf.Abs(rightAccel.y) > hitAccelThreshold || Mathf.Abs(rightAccel.z) > hitAccelThreshold) {
		// 		save(audioSource.time + ",punch");
		// 	}
		// }
	}

	public void save (string saveString) {
		sw = fi.AppendText();
		sw.WriteLine(saveString);
		sw.Flush();
		sw.Close();
	}

	private bool IsJoggle () {
    return !isPushRIndexTrigger && isPushRHandTrigger;
  }
  private bool IsPushUp () {
    return !isPushA && !isPushB && !isPushRIndexTrigger && !isPushRHandTrigger;
  }
  private bool IsPunch () {
    return isPushRIndexTrigger && isPushRHandTrigger;
  }
}
