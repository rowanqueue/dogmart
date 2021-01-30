using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    public List<Pet> pets;
    public void Initialize(){
        pets = new List<Pet>();
    }
    public void AddPet(Vector2Int gridPos){
        Pet pet = new Pet(gridPos);
        pets.Add(pet);
    }

    public void Update(){
        foreach(Pet pet in pets){
            pet.Update();
        }
    }
}
