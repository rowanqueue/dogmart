using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager
{ 
    public List<Customer> queue;
    public List<Customer> holdOver;

    public float payoutPenalty = 0.5f;

    public void Initialize()
    {
        queue = new List<Customer>();
        AddCustomer();
    }

    public void AddCustomer()
    {
        Traits t = new Traits();
        t.RandomTraits();
        Customer cust = new Customer(t,5,queue.Count);
        queue.Add(cust);
        cust = new Customer(t, 10, queue.Count);
        queue.Add(cust);
        cust = new Customer(t, 15, queue.Count);
        queue.Add(cust);
    }

    public void Update()
    {
        foreach (Customer cust in queue)
        {
            cust.Update();
        }
    }
}

public class Customer
{
    public Traits needs;
    public float currentWaitTime = 0;
    public float maxWaitTime = 10;
    public int linePosition;
    public float payout = 50;
    public GameObject gameObject;
    public bool satisfied = false;

    public Customer(Traits need,float maxWait,int linePos)
    {
        this.needs = need;
        this.maxWaitTime = maxWait;
        this.linePosition = linePos;
        CreateVisual();
    }

    public void CreateVisual()
    {
        gameObject = GameObject.Instantiate(Services.GameController.CustomerPrefab, Services.GameController.CustomerLine.transform.position + new Vector3(linePosition,0,0), Quaternion.identity, Services.GameController.CustomerLine.transform);
    }

    public void Update()
    {
        currentWaitTime += Time.deltaTime;
        Vector3 goal = new Vector3(linePosition, 0, 0);
        gameObject.transform.localPosition = gameObject.transform.localPosition + (goal - gameObject.transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);

        if (currentWaitTime > maxWaitTime)
        {
            leaveQueue();
        }
    }

    public void leaveQueue()
    {
        gameObject.SetActive(false);
        for (int i = linePosition + 1;  i < Services.CustomerManager.queue.Count; i++)
        {
            Services.CustomerManager.queue[i].linePosition--;
        }

        Services.CustomerManager.queue.Remove(this);
        Services.CustomerManager.holdOver.Add(this);
        this.payout *= Services.CustomerManager.payoutPenalty;
    }


}

