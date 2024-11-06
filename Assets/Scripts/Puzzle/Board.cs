using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField]
	private GameObject tilePrefab; //숫자 타일 프리맵
	[SerializeField]
	private Transform tilePerent; //타일이 배치되는 "Board" 오브젝트의 Transform
	private List<Tile> tileList; //생성한 타일 정보 저장
	private Vector2Int puzzleSize = new Vector2Int(4, 4); //4x4 퍼즐
	private float neighborTileDistance = 102;//인접한 타일 사이의 거리. 
	public Vector3 EmptyTilePositon { set; get; } //빈타일 위치

	private IEnumerator Start()
	{
		tileList = new List<Tile>();
		SpawnTiles();

		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tilePerent.GetComponent<RectTransform>());
		//현제 프레임이 종료될 때까지 대기 
		yield return new WaitForEndOfFrame();

		tileList.ForEach(x => x.SetCorrectPosition());
		StartCoroutine("OnSuffle");
	}
	private void SpawnTiles()
	{
		for (int y = 0; y < puzzleSize.y; ++y)
		{
			for (int x = 0; x < puzzleSize.x; ++x)
			{
				GameObject clone = Instantiate(tilePrefab, tilePerent);
				Tile tile = clone.GetComponent<Tile>();

				tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);
				tileList.Add(tile);
			}
		}
	}
	private IEnumerator OnSuffle()
	{
		float current = 0;
		float percent = 0;
		float time = 0.15f;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / time;

			int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
			tileList[index].transform.SetAsFirstSibling();

			yield return null;
		}
		EmptyTilePositon = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
	}

	public void IsMoveTile(Tile tile)
	{
		if (Vector3.Distance(EmptyTilePositon, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
		{
			Vector3 goalPosition = EmptyTilePositon;
			EmptyTilePositon = tile.GetComponent<RectTransform>().localPosition;
			tile.OnMoveTo(goalPosition);
		}
	}
	public void IsGameOver()
	{
		List<Tile> tiles = tileList.FindAll(x => x.Iscorrected == true);
		if (tiles.Count == puzzleSize.x * puzzleSize.y - 1)
		{
			GameObject.Destroy(transform.parent.parent.gameObject);
		}
	}
}
