using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg
{
    public Vector2Int gridPosition;
    public GameObject gameObject;
    public bool held;
    public bool installed;
    public int health = 20;
    public int maxPetNumber = 5;
    public float maxLength = 2.5f;
    public List<Pet> pets;
    public List<LineRenderer> ropes;

    public Peg(Vector2Int gridPos){
        this.gridPosition = gridPos;
        gameObject = GameObject.Instantiate(Services.GameController.pegPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
        ropes = new List<LineRenderer>();
        ropes.Add(gameObject.transform.GetChild(1).GetComponent<LineRenderer>());
        pets = new List<Pet>();
    }
    public void Update(){
        if(held){
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x+Mathf.Sin(Time.time*15f)*0.1f,gameObject.transform.position.y+Mathf.Sin(Time.time*10f)*0.05f,0f);
            return;
        }
        gameObject.transform.position = (Vector2)gridPosition;
        if(installed){
            gameObject.transform.GetChild(0).localEulerAngles = new Vector3(0,0,0);
        }else{
            gameObject.transform.GetChild(0).localEulerAngles = new Vector3(0,0,90);
        }

        if(installed == false){return;}
        for(int i = 0; i < pets.Count;i++){
            Pet pet = pets[i];
            ropes[i].SetPosition(0,gameObject.transform.position);
            ropes[i].SetPosition(1,pet.gameObject.transform.position);
            if(Vector2.Distance(pet.gameObject.transform.position,gameObject.transform.position) > maxLength){
                Debug.Log("yank");
                health--;
                pet.nextPosition = pet.gridPosition;
                pet.SetGoal(OppositeEnd(pet));
            }
        }
        if(health <= 0){
            GameObject.Destroy(gameObject);
            Services.PetManager.pegs.Remove(this);
        }
    }
    public Vector2Int OppositeEnd(Pet pet){
        Vector2Int difference = gridPosition - pet.gridPosition;
        difference*=-1;
        Vector2 result = ((Vector2)difference)*0.5f;
        return new Vector2Int(Mathf.FloorToInt(result.x),Mathf.FloorToInt(result.y));

    }
    public void AttachPetToPeg(Pet pet){
        if(installed == false){return;}
        if(pets.Count > maxPetNumber){return;}
        pets.Add(pet);
        if(pets.Count > 1){
            ropes.Add((GameObject.Instantiate(ropes[0].gameObject,gameObject.transform.position,Quaternion.identity,gameObject.transform) as GameObject).GetComponent<LineRenderer>());
        }
    }
}
