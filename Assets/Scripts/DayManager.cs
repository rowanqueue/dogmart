using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dayState
{
    Prep,
    Serve,
    End
}

public class DayManager
{
    public float currentDayLength;
    public float[] firstWeekLengths = {60, 90};
    public float standardDayLength = 120;
    public float currentTime;
    public int day;
    public float money;
    public dayState currentState;
    public float prepTime;
    public float prepLenght = 1;

    public float standardTimeBetweenCustomers = 5;
    public float timeBetweenCustomers = 10;
    public float currentTimeBetween = 0;

    public Color dayColor;
    public Color nightColor = Color.black;

    public void Initialize()
    {
        dayColor = Services.GameController.camera.backgroundColor;
        currentTime = 0;
        currentDayLength = firstWeekLengths[0];
        currentState = dayState.Prep;
        timeBetweenCustomers = standardTimeBetweenCustomers;
        money = Services.GameController.startingMoney;
    }

    public void Update()
    {
        Services.GameController.moneyP.text = "$" + money;
        Tick();

        if(currentTimeBetween > timeBetweenCustomers && currentState == dayState.Serve)
        {
            Services.CustomerManager.LoadQueue();
            currentTimeBetween = 0;
            timeBetweenCustomers = standardTimeBetweenCustomers - (Services.CustomerManager.todaysCustomers.Count * 0.1f);
        }

        if (currentTime > currentDayLength && currentState == dayState.Serve)
        {
            EndDay();
        }

        if (prepTime > prepLenght && currentState == dayState.Prep)
        {
            EndPrep();
        }
    }

    public void Tick()
    {
        if (currentState == dayState.Serve)
        {
            Services.GameController.camera.backgroundColor = Color.Lerp(dayColor, nightColor, currentTime / currentDayLength);
            currentTime += Time.deltaTime;
            currentTimeBetween += Time.deltaTime;
        }
        else if (currentState == dayState.Prep)
        {
            Services.GameController.camera.backgroundColor = Color.Lerp(nightColor, dayColor, prepTime / prepLenght);
            prepTime += Time.deltaTime;
        }
    }

    public void EndDay()
    {
        Services.DayManager.money-= Services.PetManager.pets.Count*Services.GameController.petUpkeepPerDay;
        if(Services.DayManager.money < 0){
            Debug.Log("WAAAAAAH i am broke");
        }
        currentState = dayState.End;
        if(day < firstWeekLengths.Length)
        {
            currentDayLength = firstWeekLengths[day];
        }
        else
        {
            currentDayLength = standardDayLength;
        }

        

        currentTime = 0;
        StartNextDay();
    }

    public void EndPrep()
    {
        currentState = dayState.Serve;
        prepTime = 0;
        Debug.Log("PrepTimeEnded");
    }

    public void StartNextDay()
    {
        day++;
        currentState = dayState.Prep;
        Services.CustomerManager.CreateTodaysCustomers(day);
    }
}
