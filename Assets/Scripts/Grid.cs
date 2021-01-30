using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public bool[,] grid;
    public float gridSize = 1.0f;
    public Vector2Int size = new Vector2Int(15,9);
    public void Initialize(){
        grid = new bool[size.x,size.y];
    }
    public bool InGrid(Vector2Int pos){
        if(pos.x < 0 || pos.x >= size.x || pos.y < 0 || pos.y >= size.y){
            return false;
        }
        return true;
    }
    public Vector2Int RandomPosition(){
        return new Vector2Int(Random.Range(0,size.x),Random.Range(0,size.y));
    }
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
        result.x = (gridPos.x)*gridSize;
        result.y = (gridPos.y)*gridSize;
        return result;
    }
}
