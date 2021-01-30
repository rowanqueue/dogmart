using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food
{
    public Bait bait;
    public Vector2Int gridPosition;
    public GameObject gameObject;
    public bool held;
    public bool installed;
    public int health = 50;
    public float maxLength = 2.5f;
    public Food(Vector2Int gridPos, Bait bait){
        this.gridPosition = gridPos;
        this.bait = bait;
        gameObject = GameObject.Instantiate(Services.GameController.foodPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
    }
    public void Update(){
        if(held){
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x+Mathf.Sin(Time.time*15f)*0.1f,gameObject.transform.position.y+Mathf.Sin(Time.time*10f)*0.05f,0f);
            return;
        }
        gameObject.transform.position = (Vector2)gridPosition;
        if(installed == false){return;}
        foreach(Pet pet in Services.PetManager.pets){
            if(pet.traits.bait == bait){
                float distance = Vector2.Distance(pet.gameObject.transform.position,gameObject.transform.position);
                if(distance <= maxLength){
                    if(pet.goal != gridPosition){
                        pet.SetGoal(gridPosition);
                    }
                }
                if(pet.gridPosition == gridPosition || distance < 0.75f){
                    health--;
                    pet.GetGoal();
                }
            }
        }
        /*if(installed){
            gameObject.transform.GetChild(0).localEulerAngles = new Vector3(0,0,0);
        }else{
            gameObject.transform.GetChild(0).localEulerAngles = new Vector3(0,0,90);
        }*/

        if(installed == false){return;}
        
        if(health <= 0){
            GameObject.Destroy(gameObject);
            Services.PetManager.foods.Remove(this);
        }
    }
}