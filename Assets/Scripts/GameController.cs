using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject petPrefab;
    public GameObject CustomerPrefab;
    public GameObject CustomerLine;
    public GameObject want;
    public Camera camera;

    [Header(("Grabbing pets"))]
    public Pet heldPet;
    public bool holdingPet;
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
    }

    void InitializeServices(){
        Services.GameController = this;
        Services.Grid = new Grid();
        Services.PetManager = new PetManager();
        Services.PetManager.Initialize();
        Services.CustomerManager = new CustomerManager();
        Services.CustomerManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(Services.Grid.MouseGridPosition());
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)){
            if(holdingPet == false){
                foreach(Pet pet in Services.PetManager.pets){
                    if(Vector2.Distance(pet.gameObject.transform.position,mousePos) < 0.75f){
                        Debug.Log("Grabbed");
                        GrabPet(pet);
                        break;
                    }
                }
            }
        }
        if(holdingPet){
            if(Input.GetMouseButtonUp(0)){
                DropPet();
            }
        }
        Services.PetManager.Update();
        Services.CustomerManager.Update();
    }
    void GrabPet(Pet pet){
        holdingPet = true;
        heldPet = pet;
        heldPet.goal = new Vector2Int(-1,-1);
        pet.held = true;
    }

    void DropPet(){
        holdingPet = false;
        heldPet.gridPosition = Services.Grid.MouseGridPosition();
        heldPet.held = false;
        float dist = Vector3.Distance(camera.ScreenToWorldPoint((Vector3)Input.mousePosition), Services.GameController.CustomerLine.transform.position);
        if (dist < 1)
        {
            Services.CustomerManager.queue[0].GotPet(heldPet);
        }
    }
}
