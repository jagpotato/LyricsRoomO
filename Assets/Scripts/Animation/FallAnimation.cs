using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAnimation : MonoBehaviour
{
	private Rigidbody rb;
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
		Destroy(GetComponent<FallAnimation>());
	}
}
