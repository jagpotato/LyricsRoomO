using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LyricsObjectsAnimator : MonoBehaviour
{
  private GameObject[] flyingObjects;
	private LyricsObject lyricsObject;

  void Start()
  {
  }

  void Update()
  {
    flyingObjects = GameObject.FindGameObjectsWithTag("LyricsObject").Where(x => x.GetComponent<LyricsObject>().IsFlying == true).ToArray();
    if (Input.GetKeyDown(KeyCode.I))
    {
      foreach (GameObject g in flyingObjects)
      {
        g.GetComponent<LyricsObject>().IsAnimation = true;	
      }
    }
  }

  public void AnimationLyricsObjects(string animationName)
  {
    // switch (animationName)
    // {
    //   case "fall":
    //     FallAnimation();
    //     break;
    //   default: break;
    // }
		foreach (GameObject g in flyingObjects)
    {
			lyricsObject = g.GetComponent<LyricsObject>();
      lyricsObject.IsAnimation = true;
			lyricsObject.AnimationName = animationName;
    }
  }
}
