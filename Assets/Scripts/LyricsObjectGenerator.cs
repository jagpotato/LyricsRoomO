using UnityEngine;

/// <summary>
/// 歌詞オブジェクトを生成する
/// </summary>
public class LyricsObjectGenerator : MonoBehaviour
{
	private GameObject instantiatedLyricsObject;
	private Material singingMaterial;
	private GameObject player;

  void Start()
	{
		singingMaterial = Resources.Load("Materials/LyricsDefault") as Material;
		player = GameObject.Find("Player");
	}

	public GameObject InstantiateLyricsObject (GameObject lyricsObject, Vector3 position, GameObject lyricsString)
	{
		instantiatedLyricsObject = Instantiate(lyricsObject, position, Quaternion.Euler(0, 180, 0), lyricsString.transform) as GameObject;
		instantiatedLyricsObject.transform.localScale *= 2.2f;
		instantiatedLyricsObject.tag = "LyricsObject";
		instantiatedLyricsObject.layer = 11;
		instantiatedLyricsObject.AddComponent<Rigidbody>();
		instantiatedLyricsObject.GetComponent<Rigidbody>().useGravity = false;
		instantiatedLyricsObject.AddComponent<BoxCollider>().enabled = false;
		instantiatedLyricsObject.AddComponent<LyricsObject>();
		return instantiatedLyricsObject;
	}

	public void activateLyricsObject (GameObject lyricsObject)
	{
		lyricsObject.SetActive(true);
		lyricsObject.GetComponent<Renderer>().material = singingMaterial;
		// lyricsObject.transform.LookAt(player.transform);
	}
}
