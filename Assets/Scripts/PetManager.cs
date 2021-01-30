using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    public List<Pet> pets;
    public List<Peg> pegs;
    public void Initialize(){
        pets = new List<Pet>();
        pegs = new List<Peg>();
    }
    public void AddPet(Vector2Int gridPos){
        Pet pet = new Pet(gridPos);
        pets.Add(pet);
    }
    public void AddPeg(Vector2Int gridPos){
        Peg peg = new Peg(gridPos);
        pegs.Add(peg);
    }

    public void Update(){
        foreach(Pet pet in pets){
            pet.Update();
        }
        for(int i = pegs.Count-1;i>=0;i--){
            pegs[i].Update();
        }
    }
}
