using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public PolygonCollider2D collider2D;
    List<Pet> burningPets;
    Bounds bounds;
    public float burnTime;
    public GameObject flamePrefab;
    public Vector2 flamePosition;
    List<Transform> flames;
    List<Vector3> flamePositions;
    List<float> flameEndTime;
    float lastFlameCreated = 0;
    public float flameSize = 1;
    // Start is called before the first frame update
    void Start()
    {
        bounds = collider2D.bounds;
        burningPets = new List<Pet>();
        flames = new List<Transform>();
        flameEndTime = new List<float>();
        flamePositions = new List<Vector3>();
        lastFlameCreated = 0;
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
        if(burningPets.Count > 0 && flames.Count < 10 && lastFlameCreated < Time.time-0.25f){
            lastFlameCreated = Time.time;
            flames.Add((GameObject.Instantiate(flamePrefab,flamePosition+Random.insideUnitCircle*0.5f,Quaternion.identity,transform) as GameObject).transform);
            //flames[flames.Count-1]
            flamePositions.Add(flames[flames.Count-1].position);
            flameEndTime.Add(Time.time+1f);
        }
        for(int i = flames.Count-1;i>=0;i--){
            flames[i].position = flamePositions[i]+new Vector3(Mathf.Cos((Time.time*20f)+flamePositions[i].magnitude*flameEndTime[i])*0.1f,Mathf.Sin((Time.time*15f)+flamePositions[i].magnitude*flameEndTime[i])*0.1f);
            float targetSize = flameSize;
            if(Time.time >= flameEndTime[i]){
                targetSize = 0;
                if(Vector3.Distance(flames[i].localScale,Vector3.zero) < 0.1f){
                    Destroy(flames[i].gameObject);
                    flames.RemoveAt(i);
                    flameEndTime.RemoveAt(i);
                    flamePositions.RemoveAt(i);
                    continue;
                }
            }
            flames[i].localScale += ((Vector3.one*targetSize)-flames[i].localScale)*0.1f;
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
