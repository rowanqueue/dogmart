using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public float gridSize = 1.0f;
    public Vector2Int MouseGridPosition(){
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int result = Vector2Int.zero;
        result.x = Mathf.FloorToInt(mousePosition.x/gridSize);
        result.y = Mathf.FloorToInt(mousePosition.y/gridSize);
        return result;
    }
    public Vector2Int RealToGrid(Vector2 pos){
        Vector2Int result = Vector2Int.zero;
        result.x = Mathf.FloorToInt(pos.x/gridSize);
        result.y = Mathf.FloorToInt(pos.y/gridSize);
        return result;
    }
    public Vector2 GridToReal(Vector2Int gridPos){
        Vector2 result = Vector2Int.zero;
        result.x = (gridPos.x+0.5f)*gridSize;
        result.y = (gridPos.y-0.5f)*gridSize;
        return result;
    }
}
