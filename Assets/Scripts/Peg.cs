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
    SpriteRenderer spriteRenderer;
    public List<Pet> pets;
    public List<LineRenderer> ropes;

    public Peg(Vector2Int gridPos){
        this.gridPosition = gridPos;
        gameObject = GameObject.Instantiate(Services.GameController.pegPrefab,(Vector2)gridPosition,Quaternion.identity,Services.GameController.transform) as GameObject;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        ropes = new List<LineRenderer>();
        ropes.Add(gameObject.transform.GetChild(1).GetComponent<LineRenderer>());
        ropes[0].positionCount = 5;
        ropes[0].enabled = false;
        pets = new List<Pet>();
    }
    public void Update(){
        if(held){
            spriteRenderer.sortingOrder = 100;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x+Mathf.Sin(Time.time*15f)*0.1f,gameObject.transform.position.y+Mathf.Sin(Time.time*10f)*0.05f,0f);
            return;
        }
        gameObject.transform.position = (Vector2)gridPosition;
        if(installed){
            spriteRenderer.sprite = Services.Visuals.pegs[1];
        }else{
            spriteRenderer.sprite = Services.Visuals.pegs[0];
        }
        spriteRenderer.sortingOrder = (5*(Services.Grid.size.y-gridPosition.y));
        if(installed == false){
            if(held == false){
                gameObject.transform.position = new Vector2(13.5f,2.8f);
            }
            return;
        }
        for(int i = 0; i < pets.Count;i++){
            ropes[i].enabled = true;
            Pet pet = pets[i];
            ropes[i].SetPosition(0,gameObject.transform.position);
            ropes[i].SetPosition(ropes[i].positionCount-1,pet.gameObject.transform.position);
            Vector2 between = pet.gameObject.transform.position-gameObject.transform.position;
            between = Vector2.Perpendicular(between).normalized*0.4f;
            ropes[i].SetPosition(1,Vector2.Lerp(ropes[i].GetPosition(0),ropes[i].GetPosition(ropes[i].positionCount-1)+Mathf.Cos(Time.time)*(Vector3)between,0.25f));
            ropes[i].SetPosition(2,Vector2.Lerp(ropes[i].GetPosition(0),ropes[i].GetPosition(ropes[i].positionCount-1)+Mathf.Cos(Time.time+10f)*(Vector3)between,0.5f));
            ropes[i].SetPosition(3,Vector2.Lerp(ropes[i].GetPosition(0),ropes[i].GetPosition(ropes[i].positionCount-1)+Mathf.Cos(Time.time+5f)*(Vector3)between*-1f,0.75f));
            if(Vector2.Distance(pet.gameObject.transform.position,gameObject.transform.position) > maxLength){
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
