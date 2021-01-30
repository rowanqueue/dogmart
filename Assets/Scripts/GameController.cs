using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject petPrefab;
    public GameObject CustomerPrefab;
    public GameObject CustomerLine;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeServices();
        Services.PetManager.AddPet(Vector2Int.zero);
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
        Services.PetManager.Update();
        Services.CustomerManager.Update();
    }
}
