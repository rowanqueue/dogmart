using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool hover;
    bool clicked;
    int noMoney;//0: dont' care, 1: go left, 2: go right
    float turnAmount = 15f;
    float rotation = 0f;
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
        if(noMoney != 0){
            if(noMoney == 1){
                rotation += (turnAmount-rotation)*0.2f;
                transform.localEulerAngles = new Vector3(0,0,rotation);
                if(Mathf.Abs(rotation-turnAmount)< 0.1f){
                    noMoney = 2;
                }
            }else if(noMoney == 2){
                rotation += (-turnAmount-rotation)*0.2f;
                transform.localEulerAngles = new Vector3(0,0,rotation);
                if(Mathf.Abs(rotation-(-turnAmount))< 0.1f){
                    noMoney = 0;
                }
            }
        }else{
            rotation += (0-rotation)*0.2f;
            transform.localEulerAngles = new Vector3(0,0,rotation);
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
            noMoney = 1;
            return;
        }
        Services.DayManager.money-=Services.GameController.pegCost;
        clicked = true;
        Services.PetManager.AddPeg();
    }
}
