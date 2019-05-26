using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using UnityEditor;  // ゲームの終了処理に使用

/// <summary>
/// 再生時間に合わせて歌詞オブジェクトを生成する
/// </summary>
[RequireComponent(typeof(LyricsObjectGenerator), typeof(LyricsObjectShooter))]
public class LyricsManager : MonoBehaviour
{
  private LyricsObjectGenerator lyricsObjectGenerator;
  private GameObject[] lyricsObjectSpawnPointObjects;
  private Vector3[] lyricsObjectSpawnPoints;
  [SerializeField]
  private GameObject firstSpawnPointObject;
  private int spawnPointId;
  private int prevSpawnPointId;
  public LyricsData[] Lyrics {
    get;
    private set;
  }
  private List<GameObject> lyricsObjects;
  private int lyricsStringId;
  private GameObject lyricsString;
  private List<GameObject> lyricsStrings;

  [SerializeField]
  private AudioSource audioSource;
  public int LyricsDataId {
    get;
    private set;
  }
  private int lyricsObjectId = 0;
  private LyricsObjectShooter lyricsObjectShooter;
  private LyricsObjectsAnimator lyricsObjectsAnimator;
  private RoomColorUpdater roomColorUpdater;

  public bool isTutorial;

  void Awake()
  {
    lyricsObjectGenerator = GetComponent<LyricsObjectGenerator>();
    lyricsObjects = new List<GameObject>();
    lyricsObjectShooter = GetComponent<LyricsObjectShooter>();
    roomColorUpdater = GameObject.Find("RoomColorUpdater").GetComponent<RoomColorUpdater>();
    lyricsObjectsAnimator = GameObject.Find("LyricsObjectsAnimator").GetComponent<LyricsObjectsAnimator>();
    // 歌詞オブジェクトのスポーン地点をVector3[]で取得
    lyricsObjectSpawnPointObjects = GameObject.FindGameObjectsWithTag("LyricsObjectSpawnPoint");
    lyricsObjectSpawnPoints = lyricsObjectSpawnPointObjects.Select(i => i.transform.position).ToArray();
    // 入力歌詞データをjsonファイルから読み込み
    Lyrics = GetLyricsFromJSONFile();
    Debug.Log(Lyrics.Length);
    // 歌詞オブジェクトをInstantiate
    spawnPointId = Array.IndexOf(lyricsObjectSpawnPointObjects, firstSpawnPointObject);
    prevSpawnPointId = spawnPointId;
    lyricsStringId = 0;
    lyricsString = new GameObject("LyricsString" + lyricsStringId);
    lyricsString.tag = "LyricsString";
    lyricsStrings = new List<GameObject>();
    lyricsStrings.Add(lyricsString);
    foreach (LyricsData lyricsData in Lyrics)
    {
      if (lyricsData.lyricsChar == "")
      {
        prevSpawnPointId = spawnPointId;
        lyricsStringId++;
        lyricsString = new GameObject("LyricsString" + lyricsStringId);
        lyricsString.tag = "LyricsString";
        lyricsStrings.Add(lyricsString);
        while (spawnPointId == prevSpawnPointId)
        {
          spawnPointId = UnityEngine.Random.Range(0, lyricsObjectSpawnPoints.Length);
        }
      }
      else
      {
        lyricsObjects.Add(lyricsObjectGenerator.InstantiateLyricsObject(GetPrefabFromLyricsChar(lyricsData.lyricsChar), lyricsObjectSpawnPoints[spawnPointId], lyricsString));
      }
    }
    // 重なっている歌詞オブジェクトをずらす
    ArrangeLyricsObject();
    lyricsStringId = 0;
    LyricsDataId = 0;
  }

  void Update()
  {
    if (LyricsDataId < Lyrics.Length)  // 歌詞データが存在する時
    {
      if (Lyrics[LyricsDataId].lyricsChar == "")  // 改行
      {
        if (lyricsStringId > 0) {
          lyricsObjectShooter.AddRandomDirectionForceToLyricsObjects(lyricsStrings[lyricsStringId - 1]);
        }
        // 前の行の歌詞をランダムに飛ばす
        // lyricsObjectShooter.AddRandomDirectionForceToLyricsObjects(lyricsObjects[lyricsObjectId - 1].transform.root.gameObject);
        lyricsStringId++;
        LyricsDataId++;
      }
      else
      {
        // 再生時間がオブジェクトの出現時間を超えたら
        if (audioSource.time >= ConvertTimeStringToFloat(Lyrics[LyricsDataId].spawnTime))
        {
          // 歌詞オブジェクトを表示
          lyricsObjectGenerator.activateLyricsObject(lyricsObjects[lyricsObjectId]);
          // 飛んでいるオブジェクトをアニメーション
          if (Lyrics[LyricsDataId].action != "")
          {
            lyricsObjectsAnimator.AnimationLyricsObjects(Lyrics[LyricsDataId].action);
          }
          // 部屋の色を変更
          roomColorUpdater.UpdateRoomMaterials(Lyrics[LyricsDataId].color);
          LyricsDataId++;
          lyricsObjectId++;
        }
      }
    }
    // 歌詞を全部出すと終了
    // if (LyricsDataId == Lyrics.Length) EditorApplication.isPlaying = false;
  }

  private LyricsData[] GetLyricsFromJSONFile()
  {
    string json = File.ReadAllText("Assets/Inputs/sugarsong.json");
    InputLyrics il = new InputLyrics();
    JsonUtility.FromJsonOverwrite(json, il);
    return il.lyrics;
  }

  private GameObject GetPrefabFromLyricsChar(string prefabName)
  {
    return (GameObject)Resources.Load("Prefabs/sugarsong/" + prefabName);
  }

  private void ArrangeLyricsObject()
  {
    GameObject[] lyricsStrings = GameObject.FindGameObjectsWithTag("LyricsString");
    List<Transform> lyricsObjectTransforms;
    foreach (GameObject lString in lyricsStrings)
    {
      lyricsObjectTransforms = lString.GetComponentsInChildren<Transform>().ToList();
      lyricsObjectTransforms.RemoveAt(0);
      for (int i = 0; i < lyricsObjectTransforms.Count; i++)
      {
        lyricsObjectTransforms[i].position += new Vector3(i * 1.5f, 0f, 0f);
        lyricsObjectTransforms[i].gameObject.SetActive(false);
      }
    }
  }

  private float ConvertTimeStringToFloat(string time)
  {
    string[] splitTime = time.Split(':');
    return float.Parse(splitTime[0]) * 60 + float.Parse(splitTime[1]) + float.Parse(splitTime[2]) / 100;
  }
}
