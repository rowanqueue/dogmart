using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    public List<Pet> pets;
    public List<Peg> pegs;
    public List<Food> foods;
    public void Initialize(){
        pets = new List<Pet>();
        pegs = new List<Peg>();
        foods = new List<Food>();
        for (int i = 0; i < 6; i++)
        {
            //AddPet(new Vector2Int(0,0));
        }
    }
    public void AddPet(Vector2Int gridPos){
        Pet pet = new Pet(gridPos);
        pets.Add(pet);
    }
    public void AddPeg(Vector2Int gridPos){
        Peg peg = new Peg(gridPos);
        pegs.Add(peg);
    }
    public void AddFood(Vector2Int gridPos){
        Food food = new Food(gridPos,Bait.Mayo);
        foods.Add(food);
    }

    public void Update(){
        foreach(Pet pet in pets){
            pet.Update();
        }
        for(int i = foods.Count-1;i>=0;i--){
            foods[i].Update();
        }
        for(int i = pegs.Count-1;i>=0;i--){
            pegs[i].Update();
        }
    }
}
