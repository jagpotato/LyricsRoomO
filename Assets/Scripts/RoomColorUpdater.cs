using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomColorUpdater : MonoBehaviour
{
  [SerializeField] private GameObject floor;
  [SerializeField] private GameObject ceiling;
  [SerializeField] private GameObject wallFront;
  [SerializeField] private GameObject wallBack;
  [SerializeField] private GameObject wallRight;
  [SerializeField] private GameObject wallLeft;
  [SerializeField] private Material defaultFloorMaterial;
  [SerializeField] private Material defaultCeilingMaterial;
  [SerializeField] private Material defaultWallFrontMaterial;
  [SerializeField] private Material defaultWallBackMaterial;
  [SerializeField] private Material defaultWallRightMaterial;
  [SerializeField] private Material defaultWallLeftMaterial;
  private Renderer floorRenderer;
  private Renderer ceilingRenderer;
  private Renderer wallFrontRenderer;
  private Renderer wallBackRenderer;
  private Renderer wallRightRenderer;
  private Renderer wallLeftRenderer;

  private LyricsManager lyricsManager;
  private LyricsData[] lyrics;
  private string[] materialNames;

	private int colorEmptyCount = 0;

  private GameObject[] flyingObjects;

  // Use this for initialization
  void Start()
  {
    floorRenderer = floor.GetComponent<Renderer>();
    ceilingRenderer = ceiling.GetComponent<Renderer>();
    wallFrontRenderer = wallFront.GetComponent<Renderer>();
    wallBackRenderer = wallBack.GetComponent<Renderer>();
    wallRightRenderer = wallRight.GetComponent<Renderer>();
    wallLeftRenderer = wallLeft.GetComponent<Renderer>();
    InitRoomMaterials();

    lyricsManager = GameObject.Find("LyricsManager").GetComponent<LyricsManager>();
    lyrics = lyricsManager.Lyrics;
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void UpdateRoomMaterials(string color)
  {
    flyingObjects = GameObject.FindGameObjectsWithTag("LyricsObject").Where(x => x.GetComponent<LyricsObject>().IsFlying == true).ToArray();
    if (color == "")
    {
			if (colorEmptyCount > 15) {
				InitRoomMaterials();
        foreach (GameObject g in flyingObjects) {
          g.GetComponent<Renderer>().material = GetResourcesMaterial("LyricsFlying");
        }
				colorEmptyCount = 0;
			}
			colorEmptyCount++;
    }
    else
    {
      materialNames = color.Split(',');
      floorRenderer.material = GetResourcesMaterial(materialNames[0]);
      ceilingRenderer.material = GetResourcesMaterial(materialNames[1]);
      wallBackRenderer.material = GetResourcesMaterial(materialNames[2]);
      wallRightRenderer.material = GetResourcesMaterial(materialNames[3]);
      wallLeftRenderer.material = GetResourcesMaterial(materialNames[4]);

      for (int i = 0; i < flyingObjects.Length; i++) {
        if (i % 5 == 0) flyingObjects[i].GetComponent<Renderer>().material = GetResourcesMaterial(materialNames[0]);
        else if (i % 5 == 1) flyingObjects[i].GetComponent<Renderer>().material = GetResourcesMaterial(materialNames[1]);
        else if (i % 5 == 2) flyingObjects[i].GetComponent<Renderer>().material = GetResourcesMaterial(materialNames[2]);
        else if (i % 5 == 3) flyingObjects[i].GetComponent<Renderer>().material = GetResourcesMaterial(materialNames[3]);
        else flyingObjects[i].GetComponent<Renderer>().material = GetResourcesMaterial(materialNames[4]);
      }

			colorEmptyCount = 0;
    }
  }
	private void InitRoomMaterials () {
		floorRenderer.material = defaultFloorMaterial;
    ceilingRenderer.material = defaultCeilingMaterial;
    wallFrontRenderer.material = defaultWallFrontMaterial;
    wallBackRenderer.material = defaultWallBackMaterial;
    wallRightRenderer.material = defaultWallRightMaterial;
    wallLeftRenderer.material = defaultWallLeftMaterial;
	}
  private Material GetResourcesMaterial(string materialName)
  {
    return Resources.Load("Materials/" + materialName) as Material;
  }
}
