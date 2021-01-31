using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Want : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderers;
    public Vector2 spaceForOne;
    public float sizeForOne;
    public List<Vector2> spacesForTwo;
    public float sizeForTwo;
    public List<Vector2> spacesForThree;
    public float sizeForThree;
    public void SetSpaces(int num){
        if(num == 4){
            return;
        }
        if(num == 1){
            spriteRenderers[0].transform.localPosition = spaceForOne;
            spriteRenderers[0].transform.localScale = Vector3.one*sizeForOne;
            return;
        }
        if(num == 2){
            for(var i = 0; i < num;i++){
                spriteRenderers[i].transform.localPosition = spacesForTwo[i];
                spriteRenderers[i].transform.localScale = Vector3.one*sizeForTwo;
            }
            return;
        }
        if(num == 3){
            for(var i = 0; i < num;i++){
                spriteRenderers[i].transform.localPosition = spacesForThree[i];
                spriteRenderers[i].transform.localScale = Vector3.one*sizeForThree;
            }
            return;
        }
    }
}
