﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject petPrefab;
    public GameObject pegPrefab;

    public GameObject foodPrefab;
    public GameObject CustomerPrefab;
    public GameObject CustomerLine;
    public GameObject want;
    public Camera camera;

    [Header(("Grabbing pets"))]
    
    public bool holdingSomething;
    public int whatHolding;//0: nothing, 1: pet, 2: peg, 3: food
    public Pet heldPet;
    public Peg heldPeg;
    public Food heldFood;
    [Header("Tuning")]
    public float petSpeed = 0.05f;
    public float petWaitMin = 0.15f;
    public float petWaitMax = 0.35f;
    public float petMinRangeToFinish = 0.1f;
    public float positiveTraitTradeGain = 1.5f;
    public float negativeTraitTradeLoss = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeServices();
        for(var i = 0; i < 5;i++){
            Services.PetManager.AddPet(Services.Grid.RandomPosition());
        }
        Services.PetManager.AddPeg(new Vector2Int(0,0));
        Services.PetManager.AddFood(new Vector2Int(Services.Grid.size.x-1,0));
    }

    void InitializeServices(){
        Services.GameController = this;
        Services.Visuals = GetComponentInChildren<Visuals>();
        Services.Grid = new Grid();
        Services.PetManager = new PetManager();
        Services.PetManager.Initialize();
        Services.CustomerManager = new CustomerManager();
        Services.CustomerManager.Initialize();
        Services.DayManager = new DayManager();
        Services.DayManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(Services.Grid.MouseGridPosition());
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)){
            if(holdingSomething == false){
                foreach(Peg peg in Services.PetManager.pegs){
                    if(peg.installed == false && Vector2.Distance(peg.gameObject.transform.position,mousePos) < 0.75f){
                        GrabPeg(peg);
                        break;
                    }
                }
            }
            if(holdingSomething == false){
                foreach(Food food in Services.PetManager.foods){
                    if(food.installed == false && Vector2.Distance(food.gameObject.transform.position,mousePos) < 0.75f){
                        GrabFood(food);
                        break;
                    }
                }
            }
            if(holdingSomething == false){
                foreach(Pet pet in Services.PetManager.pets){
                    if(Vector2.Distance(pet.gameObject.transform.position,mousePos) < 0.75f){
                        GrabPet(pet);
                        break;
                    }
                }
            }
        }
        if(holdingSomething){
            if(Input.GetMouseButtonUp(0)){
                switch(whatHolding){
                    case 1:
                        DropPet();
                        break;
                    case 2:
                        DropPeg();
                        break;
                    case 3:
                        DropFood();
                        break;
                }
            }
        }
        Services.PetManager.Update();
        Services.CustomerManager.Update();
        Services.DayManager.Update();
    }
    void GrabPet(Pet pet){
        holdingSomething = true;
        whatHolding = 1;
        heldPet = pet;
        heldPet.goal = new Vector2Int(-1,-1);
        pet.held = true;
    }

    void DropPet(){
        holdingSomething = false;
        whatHolding = 0;
        heldPet.gridPosition = Services.Grid.MouseGridPosition();
        foreach(Peg peg in Services.PetManager.pegs){
            if(peg.gridPosition == heldPet.gridPosition || Vector2.Distance(peg.gameObject.transform.position,heldPet.gameObject.transform.position) < 0.75f){
                peg.AttachPetToPeg(heldPet);
                break;
            }
        }
        heldPet.held = false;
        Vector3 mousePos = camera.ScreenToWorldPoint((Vector3)Input.mousePosition);
        mousePos.z = 0;
        float dist = Vector3.Distance(mousePos, Services.GameController.CustomerLine.transform.position);
        print("FUCK"+ mousePos + " & " + Services.GameController.CustomerLine.transform.position + "\n" + dist);
        if (dist < 1)
        {
            Services.CustomerManager.queue[0].GotPet(heldPet);
        }
        heldPet = null;

    }
    void GrabPeg(Peg peg){
        holdingSomething = true;
        whatHolding = 2;
        heldPeg = peg;
        peg.held = true;
    }
    void DropPeg(){
        holdingSomething = false;
        whatHolding = 0;
        heldPeg.gridPosition = Services.Grid.MouseGridPosition();
        heldPeg.installed = true;
        heldPeg.held = false;
        heldPeg = null;
    }
    void GrabFood(Food food){
        holdingSomething = true;
        whatHolding = 3;
        heldFood = food;
        food.held = true;
    }
    void DropFood(){
        holdingSomething = false;
        whatHolding = 0;
        heldFood.gridPosition = Services.Grid.MouseGridPosition();
        heldFood.installed = true;
        heldFood.held = false;
        heldFood = null;
    }
}
