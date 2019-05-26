using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
	private AudioSource audioSource;
	[SerializeField]
	private float startTime;

	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			audioSource.time = 23f;
			audioSource.Play();
		}
		if (OVRInput.GetDown(OVRInput.RawButton.Start)) {
			audioSource.time = startTime;
			audioSource.Play();
		}
	}
}
