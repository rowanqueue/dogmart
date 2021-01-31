using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    public List<Pet> pets;
    public List<Peg> pegs;
    public List<Food> foods;
    public float nextPetTime;
    public void Initialize(){
        pets = new List<Pet>();
        pegs = new List<Peg>();
        foods = new List<Food>();
        nextPetTime = Time.time+Services.GameController.startingTimeBetweenPets;
    }
    public void AddPet(Vector2Int gridPos){
        Pet pet = new Pet(gridPos);
        pets.Add(pet);
    }
    public void AddPeg(){
        Peg peg = new Peg(new Vector2Int(Services.Grid.size.x-1,Services.Grid.size.y/2));
        pegs.Add(peg);
    }
    public void AddFood(){
        Food food = new Food(new Vector2Int(0,Services.Grid.size.y/2),Bait.Mayo);
        foods.Add(food);
    }

    public void Update(){
        if(Time.time >= nextPetTime){
            nextPetTime = Time.time+Services.GameController.startingTimeBetweenPets+(Services.GameController.petBonusToSpawnTime*(float)pets.Count);
            AddPet(Services.GameController.petStartingPos);
        }
        for(int i = pets.Count-1;i>=0;i--){
            pets[i].spriteRenderer.sortingOrder = i;
            pets[i].Update();
        }
        for(int i = foods.Count-1;i>=0;i--){
            foods[i].Update();
        }
        for(int i = pegs.Count-1;i>=0;i--){
            pegs[i].Update();
        }
    }
}
