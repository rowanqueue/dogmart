using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    public bool happy;//else sad
    public SpriteRenderer spriteRenderer;
    public float targetSize;
    public float killHeight;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = Services.Visuals.reactions[happy ? 1 :0];
        if(transform.localScale.z <= targetSize*0.99f){
            transform.localScale += (Vector3.one*targetSize-transform.localScale)*0.05f;
            return;
        }
        transform.localPosition += (new Vector3(transform.localPosition.x,killHeight+2)-transform.localPosition)*0.025f;
        if(transform.localPosition.y >= killHeight){
            Destroy(this.gameObject);
        }
    }
}
