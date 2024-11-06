using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField]
	private GameObject tilePrefab; //숫자 타일 프리맵
	[SerializeField]
	private Transform tilePerent; //타일이 배치되는 "Board" 오브젝트의 Transform
	private Vector2Int puzzlesize = new Vector2Int(4, 4); //4x4 퍼즐

	private void Start()
	{
		SpawnTiles();
	}
	private void SpawnTiles()
	{
		for (int y = 0; y < puzzlesize.y; ++y)
		{
			for (int x = 0; x < puzzlesize.x; ++x)
			{
				Instantiate(tilePrefab, tilePerent);
			}
		}
	}
}
