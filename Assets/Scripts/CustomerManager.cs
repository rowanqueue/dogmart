using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager
{ 
    public List<Customer> queue;
    public List<Customer> holdOver;
    public List<Customer> done;

    public float textDist = 0.1f;
    public float customerDist = 1;

    public float payoutPenalty = 0.5f;

    public void Initialize()
    {
        queue = new List<Customer>();
        holdOver = new List<Customer>();
        done = new List<Customer>();
        AddCustomer();
    }

    public void AddCustomer()
    {
        Traits t = new Traits();
        t.RandomTraits();
        Customer cust = new Customer(t,5,queue.Count);
        queue.Add(cust);
        cust = new Customer(t, 20, queue.Count);
        queue.Add(cust);
        cust = new Customer(t, 20, queue.Count);
        queue.Add(cust);
    }

    public void Update()
    {
        foreach (Customer cust in queue)
        {
            cust.Update();
        }

        DrainQueue();
    }

    public void DrainQueue()
    {
        foreach (Customer cust in holdOver)
        {
            if (cust.inQueue)
            {
                queue.Remove(cust);
                cust.inQueue = false;
            }
        }

        foreach (Customer cust in done)
        {
                queue.Remove(cust);
        }
    }
}

public class Customer
{
    public Traits needs;
    public int totalNeeds;
    public float currentWaitTime = 0;
    public float maxWaitTime = 10;
    public int linePosition;
    public float payout = 50;
    public GameObject gameObject;
    public bool satisfied = false;
    public bool inQueue = true;
    public List<GameObject> want;

    public Customer(Traits need,float maxWait,int linePos)
    {
        this.needs = need;
        this.maxWaitTime = maxWait;
        this.linePosition = linePos;
        want = new List<GameObject>();
        CreateVisual();
    }

    public void CreateVisual()
    {
        gameObject = GameObject.Instantiate(Services.GameController.CustomerPrefab, Services.GameController.CustomerLine.transform.position + new Vector3(linePosition,0,0), Quaternion.identity, Services.GameController.CustomerLine.transform);

        if(needs.hue != Hue.None)
        {
            totalNeeds++;
            GameObject gob = GameObject.Instantiate(Services.GameController.want, gameObject.transform.position + new Vector3(0, 0.05f + totalNeeds * 0.1f, 0), Quaternion.identity, gameObject.transform);
            gob.GetComponent<TextMesh>().text = "Need:" + (int) needs.hue;
            want.Add(gob); 
        }

        if (needs.pattern != Pattern.None)
        {
            totalNeeds++;
            GameObject gob = GameObject.Instantiate(Services.GameController.want, gameObject.transform.position + new Vector3(0, 0.05f + totalNeeds * 0.1f, 0), Quaternion.identity, gameObject.transform);
            gob.GetComponent<TextMesh>().text = "Need:" + (int)needs.pattern;
            want.Add(gob);
        }

        if (needs.shape != Shape.None)
        {
            totalNeeds++;
            GameObject gob = GameObject.Instantiate(Services.GameController.want, gameObject.transform.position + new Vector3(0, 0.05f + totalNeeds * 0.1f, 0), Quaternion.identity, gameObject.transform);
            gob.GetComponent<TextMesh>().text = "Need:" + (int)needs.hue;
            want.Add(gob);
        }
    }

    public void Update()
    {
        currentWaitTime += Time.deltaTime;
        Vector3 goal = new Vector3(linePosition * Services.CustomerManager.customerDist, 0, 0);
        gameObject.transform.localPosition = gameObject.transform.localPosition + (goal - gameObject.transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);

        if (currentWaitTime > maxWaitTime)
        {
            leaveQueue();
        }
        if (linePosition == 0 ){
            for (int i = 0; i < want.Count; i++) {
                want[i].SetActive(true);
                Vector3 pos = new Vector3(0, 0.05f + (i + 1) * Services.CustomerManager.textDist, 0);
                want[i].transform.localPosition = want[i].transform.localPosition + (pos - want[i].transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);
            }
        }
        else
        {
            for (int i = 0; i < want.Count; i++)
            {
                want[i].SetActive(false);
            }
        }
    }

    public void leaveQueue()
    {
        this.payout *= Services.CustomerManager.payoutPenalty;

        for (int i = linePosition + 1;  i < Services.CustomerManager.queue.Count; i++)
        {
            Services.CustomerManager.queue[i].linePosition--;
        }

        if (!satisfied)
        {
            Services.CustomerManager.holdOver.Add(this);
            gameObject.SetActive(false);
        }
        else
        {
            Services.CustomerManager.done.Add(this);
            GameObject.Destroy(gameObject);
        }
    }

    public void GotPet(Pet pet)
    {
        float happ = pet.traits.CompareTrait(needs);
        satisfied = true;
        leaveQueue();
        pet.gameObject.SetActive(false);

    }
}

