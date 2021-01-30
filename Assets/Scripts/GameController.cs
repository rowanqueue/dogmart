﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject petPrefab;
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
