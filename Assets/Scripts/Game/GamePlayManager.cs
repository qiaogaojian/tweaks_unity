using System;
using System.Collections.Generic;
using Mega;
using TMPro;
using UnityEngine;

public class GamePlayManager : MonoSingleton<GamePlayManager>
{
    private GameObject   objectRoot;
    private Action       onGameEnd;
    private GamePlayType curGameType = GamePlayType.None;

    public void StartGame(GamePlayType type)
    {
        objectRoot = new GameObject("GameObjectRoot");
        objectRoot.transform.SetParent(transform);

        curGameType = type;
        switch (curGameType)
        {
            case GamePlayType.LootGrid:
                startLootGrid();
                break;
            default:
                Debug.LogError("未选择游戏类型");
                break;
        }
    }


    private void startLootGrid()
    {
        GameObject lootGridRoot = new GameObject("lootGridRoot");
        lootGridRoot.transform.SetParent(objectRoot.transform);

        List<GameObject> lootCelList = new List<GameObject>();
        GameObject       cellObject  = Resources.Load<GameObject>("Prefabs/Game/LootCell");

        int   boardLenth = 10;
        float cellSize   = cellObject.GetComponent<SpriteRenderer>().size.x * 1.1f;

        Vector2 oriPos = new Vector2(-boardLenth / 2f * cellSize + cellSize / 2, boardLenth / 2f * cellSize - cellSize / 2);
        for (int y = 0; y < boardLenth; y++)
        {
            for (int x = 0; x < boardLenth; x++)
            {
                GameObject lootCell = GameObject.Instantiate(cellObject, lootGridRoot.transform);
                lootCelList.Add(lootCell);
                lootCell.transform.position = new Vector3(oriPos.x + cellSize * x, oriPos.y - cellSize * y, 0);
                lootCell.gameObject.name    = $"LootCell {y * boardLenth + x}";
                lootCell.transform.Find("tvLoot").GetComponent<TextMeshPro>().SetText($"{y * boardLenth + x}");
            }
        }

        onGameEnd = () =>
        {
            Destroy(lootGridRoot);
            lootGridRoot = null;
            foreach (var t in lootCelList)
            {
                Destroy(t);
            }
            lootCelList.Clear();
        };
    }

    public void EndGame()
    {
        onGameEnd.Invoke();
    }
}