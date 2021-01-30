using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Hue{
    Red,
    Green,
    Blue,
    Yellow
}
public enum Shape{
    Circle,
    Square,
    Hex,
    Triangle
}
public enum Pattern{
    None,
    Striped,
    Spotted,
    Dead
}
public enum Bait{
    Mayo,
    Watermelon,
    Fish,
    IceCream
}

public class Pet
{
    public Vector2Int gridPosition;
    public Vector2Int goal;
    public Vector2Int nextPosition;
    AStarSearch search;
    public GameObject gameObject;
    float nextMovementAllowed;

    public Traits traits;
    public Pet(Vector2Int gridPos, Traits traits){
        this.gridPosition = gridPos;
        this.traits = traits;
        CreateVisual();
        nextMovementAllowed = Time.time;
    }
    public Pet(Vector2Int gridPos){
        this.gridPosition = gridPos;
        this.traits = new Traits();
        this.traits.RandomTraits();
        CreateVisual();
        GetGoal();
        while(search.steps.Count == 0){
            GetGoal();
        }
        
    }
    void CreateVisual(){
        gameObject = GameObject.Instantiate(Services.GameController.petPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
    }

    public void Update(){
        Vector2Int targetPosition = gridPosition;
        if(Time.time >= nextMovementAllowed){
            if(nextPosition != new Vector2Int(-1,-1)){
                //we are currently moving to something
                if(Vector2.Distance(gameObject.transform.position,nextPosition) < 0.1f){
                    //go to next one
                    nextMovementAllowed = Time.time+Random.Range(0.1f,0.3f);
                    gridPosition = nextPosition;
                    if(search.steps.Count > 0){
                        nextPosition = search.steps.Pop();
                    }else{
                        nextPosition = new Vector2Int(-1,-1);
                    }
                    
                }
                targetPosition = nextPosition;
                if(nextPosition == new Vector2Int(-1,-1)){
                    targetPosition = gridPosition;
                }
            }else{
                GetGoal();
            }
        }
        if(Services.Grid.InGrid(targetPosition) == false){
            targetPosition = gridPosition;
        }
        gameObject.transform.position += ((Vector3)Services.Grid.GridToReal(targetPosition)-gameObject.transform.position)*0.05f;
    }
    public void GetGoal(){
        goal = gridPosition + new Vector2Int(Random.Range(-2,3),Random.Range(-2,3));
        while(Services.Grid.InGrid(goal) == false){
            goal = gridPosition + new Vector2Int(Random.Range(-2,3),Random.Range(-2,3));
        }
        search = new AStarSearch(gridPosition,goal);
        if(search.steps.Count > 0){
            nextPosition = search.steps.Pop();
        }
    }

}
public struct Traits{
    public Hue hue;
    public Shape shape;
    public Pattern pattern;
    public Bait bait;
    public void RandomTraits(){
        this.hue = (Hue)Random.Range(0,4);
        this.shape = (Shape)Random.Range(0,4);
        this.pattern = (Pattern)Random.Range(0,4);
        this.bait = (Bait)Random.Range(0,4);
    }
     public override string ToString(){
         string s = hue+", "+shape+", "+pattern+", "+bait;
         return s;
     }
}
