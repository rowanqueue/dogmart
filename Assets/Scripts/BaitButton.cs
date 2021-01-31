using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitButton : MonoBehaviour
{
    public Bait bait;
    SpriteRenderer spriteRenderer;
    bool hover;
    bool clicked;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Services.Visuals.baitButtons[(int)bait-1];
    }
    // Start is called before the first frame update
    void Update()
    {
        float targetScale = 1f;
        if(hover){
            targetScale = 1.25f;
        }
        if(clicked){
            targetScale = 0.75f;
            if(Vector3.Distance(transform.localScale,Vector3.one*0.75f) < 0.1f){
                clicked = false;
            }
        }
        transform.localScale +=((Vector3.one*targetScale)-transform.localScale)*0.1f;
    }

    void OnMouseOver(){
        hover = true;
    }
    void OnMouseExit(){
        hover = false;
    }
    void OnMouseDown(){
        bool full = false;
        foreach(Food food in Services.PetManager.foods){
            if(food.installed == false){
                full = true;
                break;
            }
        }
        if(full){return;}
        if(Services.DayManager.money < Services.GameController.baitCost){
            Debug.Log("NO MONEY!!");
            return;
        }
        Services.DayManager.money-=Services.GameController.baitCost;
        clicked = true;
        Services.PetManager.AddFood(bait);
    }
}
