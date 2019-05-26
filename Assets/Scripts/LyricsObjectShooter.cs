using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyricsObjectShooter : MonoBehaviour
{
  // private Transform[] lyricsObjectTransforms;
  private Rigidbody[] lyricsObjectRigidbodies;
  private BoxCollider[] lyricsObjectColliders;
  private LyricsObject[] lyricsObjectScripts;
  private Vector3 direction;
	[SerializeField]
  private float thrust = 5f;
  private Material flyingMaterial;

  void Start()
  {
    flyingMaterial = Resources.Load("Materials/LyricsFlying") as Material;
  }

  public void AddRandomDirectionForceToLyricsObjects(GameObject lyricsString)
  {
    lyricsObjectRigidbodies = lyricsString.GetComponentsInChildren<Rigidbody>();
    lyricsObjectColliders = lyricsString.GetComponentsInChildren<BoxCollider>();
    lyricsObjectScripts = lyricsString.GetComponentsInChildren<LyricsObject>();
    for (int i = 0; i < lyricsObjectRigidbodies.Length; i++)
    {
      direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, -0.1f));
      direction.Normalize();
			lyricsObjectRigidbodies[i].AddForce(direction * thrust, ForceMode.Impulse);
      // lyricsObjectRigidbodies[i].useGravity = true;
      lyricsObjectScripts[i].IsFlying = true;
      lyricsObjectColliders[i].enabled = true;
      // lyricsObjectScripts[i].gameObject.transform.localScale *= 0.5f;
      lyricsObjectScripts[i].GetComponent<Renderer>().material = flyingMaterial;
    }
  }
}
