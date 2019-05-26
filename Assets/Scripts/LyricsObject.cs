using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyricsObject : MonoBehaviour
{
  private float time;
  private Rigidbody rb;
  public bool IsAnimation
  {
    get;
    set;
  }
  public bool IsFlying
  {
    get;
    set;
  }
  public string AnimationName
  {
    get;
    set;
  }
  private Material flyingMaterial;
  private GameObject hand_right;
  private GameObject hand_left;
  private Vector3 hand_position;
  private Vector3 direction;
  private float thrust = 10f;
  private Vector3 rightAccel;
  private Vector3 leftAccel;
  private float hitAccelThreshold = 10f;
  private OculusTouchInputter oculusTouchInputter;
  private bool isPushA;
  private bool isPushB;
  private bool isPushRIndexTrigger;
  private bool isPushRHandTrigger;
  OVRHapticsClip hapticsClip;
  byte[] samples = new byte[8];
  private Vector3 defaultScale;

  private GameObject hitEffectRoom;
  private GameObject hitEffectHand;
  private GameObject explosionEffect;
  private bool isPunched;
  private float explosionTime;

  private LyricsManager lyricsManager;

  private MyCSVWriter csv;
  private AudioSource audioSource;

  void Awake()
  {
    hand_right = GameObject.Find("hand_right");
    hand_left = GameObject.Find("hand_left");
    oculusTouchInputter = GameObject.Find("OculusTouchInputter").GetComponent<OculusTouchInputter>();
    rb = GetComponent<Rigidbody>();
    IsAnimation = false;
    IsFlying = false;
    AnimationName = "";
    flyingMaterial = Resources.Load("Materials/LyricsFlying") as Material;
    for (int i = 0; i < samples.Length; i++)
    {
      samples[i] = 250;
    }
    hapticsClip = new OVRHapticsClip(samples, samples.Length);

    hitEffectRoom = Resources.Load("Prefabs/HitEffect_B") as GameObject;
    hitEffectHand = Resources.Load("Prefabs/HitEffect_A") as GameObject;
    explosionEffect = Resources.Load("Prefabs/Explosion_A") as GameObject;
    isPunched = false;
    explosionTime = 0;
    lyricsManager = GameObject.Find("LyricsManager").GetComponent<LyricsManager>();
    csv = GameObject.Find("CSVWriter").GetComponent<MyCSVWriter>();
    audioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
  }

  void Start()
  {
    defaultScale = transform.localScale;
    // 一定時間後に削除
    if (!lyricsManager.isTutorial) Destroy(this.gameObject, 20f);
  }

  void Update()
  {
    isPushA = oculusTouchInputter.IsPushA;
    isPushB = oculusTouchInputter.IsPushB;
    isPushRIndexTrigger = oculusTouchInputter.IsPushRIndexTrigger;
    isPushRHandTrigger = oculusTouchInputter.IsPushRHandTrigger;

    time += Time.deltaTime;
    if (isPunched) {
      explosionTime += Time.deltaTime;
      if (explosionTime > 1f) {
        Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 1f);
        Destroy(this.gameObject);
      }
    }
    if (IsFlying == false && time > 2.5f) {
      GetComponent<Renderer>().material = flyingMaterial;
    }
    if (IsAnimation == true)
    {
      switch (AnimationName)
      {
        case "fall":
          this.gameObject.AddComponent<FallAnimation>();
          break;
        default: break;
      }
      IsAnimation = false;
    }
    // if (OVRInput.GetDown(OVRInput.RawButton.B))
    // {
    //   rightAccel = oculusTouchInputter.RightAccel;
    //   Debug.Log(rightAccel.y);
    //   if (rightAccel.y > 10f)
    //   {
    //     if (IsFlying == true)
    //     {
    //       rb.AddForce(new Vector3(0f, 20f, 0f), ForceMode.Impulse);
    //     }
    //   }
    // }
    if (IsFlying) {
      transform.localScale = defaultScale * 0.7f;
    }
    if (IsJoggle()) {
      rightAccel = oculusTouchInputter.RightAccel;
      if ((rightAccel.x > 12f || rightAccel.y > 12f || rightAccel.z > 12f) && IsFlying == true) {
        transform.localScale *= 1.2f;
        // csv.save(audioSource.time + ",joggle");
      }
    }
    if (IsPushUp()) {
      rightAccel = oculusTouchInputter.RightAccel;
      if (rightAccel.y > 13f && IsFlying == true) {
        rb.AddForce(new Vector3(0f, Random.Range(0.1f, 0.5f), 0f), ForceMode.Impulse);
        // csv.save(audioSource.time + ",pushup");
      }
    }
  }

  void FixedUpdate ()
  {
    if (IsFlying) rb.AddForce(new Vector3(0f, -1f, 0f), ForceMode.Acceleration);
  }

  void OnCollisionEnter(Collision col)
  {
    if (col.gameObject.tag == "Room") {
      Destroy(Instantiate(hitEffectRoom, transform.position, Quaternion.identity), 1f);
    }
    if (col.gameObject.name == "hand_right" && IsPunch())
    {
      rightAccel = oculusTouchInputter.RightAccel;
      if (Mathf.Abs(rightAccel.x) > hitAccelThreshold || Mathf.Abs(rightAccel.y) > hitAccelThreshold || Mathf.Abs(rightAccel.z) > hitAccelThreshold)
      {
        Destroy(Instantiate(hitEffectHand, transform.position, Quaternion.identity), 1f);
        hand_position = hand_right.transform.position;
        direction = GetDirectionFromAtoB(hand_position, transform.position);
        rb.AddForce(direction * thrust, ForceMode.Impulse);
        OVRHaptics.RightChannel.Mix(hapticsClip);
        isPunched = true;
        csv.save(audioSource.time + ",punch");
      }
    }
    // else if (col.gameObject.name == "hand_left" && IsPunch())
    // {
    //   leftAccel = oculusTouchInputter.RightAccel;
    //   if (Mathf.Abs(leftAccel.x) > hitAccelThreshold || Mathf.Abs(leftAccel.y) > hitAccelThreshold || Mathf.Abs(leftAccel.z) > hitAccelThreshold)
    //   {
    //     hand_position = hand_left.transform.position;
    //     direction = GetDirectionFromAtoB(hand_position, transform.position);
    //     rb.AddForce(direction * thrust, ForceMode.Impulse);
    //     OVRHaptics.LeftChannel.Mix(hapticsClip);
    //   }
    // }
  }

  private Vector3 GetDirectionFromAtoB(Vector3 positionA, Vector3 positionB)
  {
    return (positionB - positionA).normalized;
  }

  private void FallAnimation()
  {
    rb.AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
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
