using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTouchInputter : MonoBehaviour {
	Vector2 Ldirection;
	private GameObject playerCamera;
	private GameObject playerAvatar;
	private float speed = 0.05f;
	private float thrust = 5f;
	public Vector3 RightAccel {
		get;
		private set;
	}
	public Vector3 LeftAccel {
		get;
		private set;
	}
	public Vector3 RAngleAccel {
		get;
		private set;
	}
	public Vector3 LAngleAccel {
		get;
		private set;
	}
	public bool IsPushA {
		get;
		private set;
	}
	public bool IsPushB {
		get;
		private set;
	}
	public bool IsPushRIndexTrigger {
		get;
		private set;
	}
	public bool IsPushRHandTrigger {
		get;
		private set;
	}

	// Use this for initialization
	void Start () {
		Debug.Log("start");
		// playerCamera = GameObject.Find("OVRCameraRig");
		playerAvatar = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		IsPushA = false;
		IsPushB = false;
		IsPushRIndexTrigger = false;
		IsPushRHandTrigger = false;

		Ldirection = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
		// playerCamera.transform.position += new Vector3(Ldirection.x, 0f, Ldirection.y) * speed;
		playerAvatar.transform.position += new Vector3(Ldirection.x, 0f, Ldirection.y) * speed;

		RightAccel = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
		LeftAccel = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.LTouch);

		if (OVRInput.Get(OVRInput.RawButton.A)) {
			IsPushA = true;
		}
		if (OVRInput.Get(OVRInput.RawButton.B)) {
			IsPushB = true;
		}
		if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {
			IsPushRIndexTrigger = true;
		}
		if (OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
			IsPushRHandTrigger = true;
		}
	}
}
