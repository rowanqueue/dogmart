using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public PolygonCollider2D collider2D;
    List<Pet> burningPets;
    Bounds bounds;
    public float burnTime;
    // Start is called before the first frame update
    void Start()
    {
        bounds = collider2D.bounds;
        burningPets = new List<Pet>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = burningPets.Count-1;i>=0;i--){
            Pet pet = burningPets[i];
            if(pet.dead == false){
                if(Time.time >burnTime*0.5f + pet.burnStartTime){
                    pet.dead = true;
                }
            }
            pet.Update();
            if(Time.time > burnTime+pet.burnStartTime){
                pet.Destroy();
                burningPets.Remove(pet);
            }
            
        }
        if(Input.GetMouseButtonUp(0)){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(bounds.Contains(mousePos)){
                if(Services.GameController.holdingSomething){
                    switch(Services.GameController.whatHolding){
                        case 1://pet
                            Services.PetManager.pets.Remove(Services.GameController.heldPet);
                            burningPets.Add(Services.GameController.heldPet);
                            Services.GameController.heldPet.burnStartTime = Time.time;
                            break;
                        case 2://peg
                            break;
                        case 3://food
                            break;
                    }
                }
            }
        }
    }
}
