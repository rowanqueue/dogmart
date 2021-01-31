using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Hue{
    None,
    Green,
    Red,
    Yellow
}
public enum Shape{
    None,
    Circle,
    Square,
    Hex,
    Triangle
}
public enum Pattern{
    None,
    Solid,
    Spotted,
    Frosted,
    Striped
}
public enum Bait{
    None,
    Mayo,
    Watermelon,
    Fish,
    IceCream
}
public enum State{
    Resting,
    Moving
}

public class Pet
{
    public bool dead;
    public State state;//0: resting, 1: moving
    public bool held;
    public Vector2Int gridPosition;
    public Vector2Int goal;
    public Vector2Int nextPosition;
    AStarSearch search;
    public GameObject gameObject;
    public SpriteRenderer spriteRenderer;
    float nextMovementAllowed;
    float restEndTime;
    float specialNumber;
    float birthTime;
    float lifeSpan;

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
        traits.shape = Shape.Circle;
        specialNumber = Random.Range(0f,10f);
        birthTime = Time.time;
        lifeSpan = Services.GameController.defaultLifeSpan;
        state = State.Moving;
        CreateVisual();
        GetGoal();
        while(search.steps.Count == 0){
            GetGoal();
        }
        
    }
    void CreateVisual(){
        gameObject = GameObject.Instantiate(Services.GameController.petPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
        gameObject.transform.position-=Vector3.up;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = Services.Visuals.GetVisual(this);
    }

    public void Update(){
        if(held){
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x+Mathf.Sin(Time.time*15f)*0.1f,gameObject.transform.position.y+Mathf.Sin(Time.time*10f)*0.05f,0f);
            return;
        }
        if(Time.time >= birthTime+lifeSpan){
            dead = true;
        }
        if(dead){ 
            gameObject.transform.position += ((Vector3)Services.Grid.GridToReal(gridPosition)-gameObject.transform.position)*Services.GameController.petSpeed;
            spriteRenderer.transform.localEulerAngles += (new Vector3(0,0,90)-spriteRenderer.transform.localEulerAngles)*0.1f;
            spriteRenderer.color = Color.gray;
            return;
        }
        Vector2Int targetPosition = gridPosition;
        if(state == State.Resting){
            if(Time.time >= restEndTime){
                state = State.Moving;
            }
        }
        if(state == State.Moving && Time.time >= nextMovementAllowed){
            if(goal != new Vector2Int(-1,-1)){
                //we are currently moving to something
                if(Vector2.Distance(gameObject.transform.position,nextPosition) < Services.GameController.petMinRangeToFinish){
                    //go to next one
                    nextMovementAllowed = Time.time+Random.Range(Services.GameController.petWaitMin,Services.GameController.petWaitMax);
                    gridPosition = nextPosition;
                    if(search.steps.Count > 0){
                        nextPosition = search.steps.Pop();
                    }else{
                        goal = new Vector2Int(-1,-1);
                    }
                    
                }
                targetPosition = nextPosition;
                if(nextPosition == new Vector2Int(-1,-1)){
                    targetPosition = gridPosition;
                }
            }else{
                if(Random.value < 0.5f){
                    state = State.Resting;
                    restEndTime = Time.time+Random.Range(0.5f,2f);
                }else{
                    GetGoal();
                }
                
            }
        }
        if(Services.Grid.InGrid(targetPosition) == false){
            targetPosition = gridPosition;
        }
        Vector3 target =  (Vector3)Services.Grid.GridToReal(targetPosition) + new Vector3(0,Mathf.Cos((Time.time*(10f+specialNumber*0.05f))+specialNumber)*0.1f);
        gameObject.transform.position += (target-gameObject.transform.position)*Services.GameController.petSpeed;
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
    public void SetGoal(Vector2Int g){
        goal = g;
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
        this.hue = (Hue)Random.Range(1,4);
        this.shape = (Shape)Random.Range(1,5);
        this.pattern = (Pattern)Random.Range(1,5);
        this.bait = (Bait)Random.Range(1,5);
    }
    public override string ToString(){
        string s = hue+", "+shape+", "+pattern+", "+bait;
        return s;
    }
    public float CompareTrait(Traits request){
        float value = 0;
        if(request.hue != Hue.None){
            if(request.hue == hue){
                value+=Services.GameController.positiveTraitTradeGain;
            }else{
                value+=Services.GameController.negativeTraitTradeLoss;
            }
        }
        if(request.shape != Shape.None){
            if(request.shape == shape){
                value+=Services.GameController.positiveTraitTradeGain;
            }else{
                value+=Services.GameController.negativeTraitTradeLoss;
            }
        }
        if(request.pattern != Pattern.None){
            if(request.pattern == pattern){
                value+=Services.GameController.positiveTraitTradeGain;
            }else{
                value+=Services.GameController.negativeTraitTradeLoss;
            }
        }
        if(request.bait != Bait.None){
            if(request.bait == bait){
                value+=Services.GameController.positiveTraitTradeGain;
            }else{
                value+=Services.GameController.negativeTraitTradeLoss;
            }
        }
        return value;
    }
}
