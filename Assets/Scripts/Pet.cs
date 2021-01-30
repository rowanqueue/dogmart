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
    public GameObject gameObject;

    public Traits traits;
    public Pet(Vector2Int gridPos, Traits traits){
        this.gridPosition = gridPos;
        this.traits = traits;
        CreateVisual();
    }
    public Pet(Vector2Int gridPos){
        this.gridPosition = gridPos;
        this.traits = new Traits();
        this.traits.RandomTraits();
        CreateVisual();
    }
    void CreateVisual(){
        gameObject = GameObject.Instantiate(Services.GameController.petPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
    }

    public void Update(){
        Debug.Log(traits);
        gameObject.transform.position += ((Vector3)Services.Grid.GridToReal(gridPosition)-gameObject.transform.position)*0.1f;
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
