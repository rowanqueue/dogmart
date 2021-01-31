using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    public int numColors;
    public int numPatterns;
    public Sprite[] circleBoys;
    public Sprite[] squareLads;
    public Sprite[] triGuys;
    public Sprite[] pegs;
    public Sprite[] baits;
    public Sprite[] baitSplotches;
    public Sprite[] baitButtons;

    public Sprite[] customerBodies;
    public Sprite[] customerEyes;
    public Sprite[] customerMouths;
    public Sprite[] customerNoses;

    public Sprite[] wantShapes;
    public Sprite[] wantPatterns;
    public Sprite[] wantColors;

    public Sprite[] reactions;




    public Sprite GetVisual(Pet pet){
        Sprite[] boys;
        switch(pet.traits.shape){
            case Shape.Circle:
                boys = circleBoys;
                break;
            case Shape.Square:
                boys = squareLads;
                break;
            case Shape.Triangle:
                boys = triGuys;
                break;
            default:
                boys = new Sprite[1];
                break;
        }
        if(boys.Length == 1){return null;}
        Debug.Log(pet.traits.hue+" : "+pet.traits.pattern);
        int index = ((int)pet.traits.hue-1)*numPatterns;
        index+=((int)pet.traits.pattern-1);
        return boys[index];
    }
}
