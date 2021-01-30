using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject petPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeServices();
        for(var i = 0; i < 50;i++){
            Services.PetManager.AddPet(Services.Grid.RandomPosition());
        }
    }
    void InitializeServices(){
        Services.GameController = this;
        Services.Grid = new Grid();
        Services.PetManager = new PetManager();
        Services.PetManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(Services.Grid.MouseGridPosition());
        }
        Services.PetManager.Update();
    }
}
