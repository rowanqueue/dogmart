using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool hover;
    bool clicked;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        foreach(Peg peg in Services.PetManager.pegs){
            if(peg.installed == false){
                full = true;
                break;
            }
        }
        if(full){return;}
        if(Services.DayManager.money < Services.GameController.pegCost){
            Debug.Log("NO MONEY!!");
            return;
        }
        Services.DayManager.money-=Services.GameController.pegCost;
        clicked = true;
        Services.PetManager.AddPeg();
    }
}
