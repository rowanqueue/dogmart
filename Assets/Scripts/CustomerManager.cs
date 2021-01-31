using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager
{ 
    public List<Customer> queue;
    public List<Customer> holdOver;
    public List<Customer> done;
    public List<Customer> todaysCustomers;

    public int[] amountOfCustomersInTheFirstWeek = {4, 6, 8};
    public int[] customerNeedinFirstWeek = { 1, 2, 3, 3 };

    public int standardAmountOfCustomersPerDay = 10;
    public int standardNeed = 4;
    public float standardWait = 10;

    public float textDist = 0.1f;
    public float customerDist = 1;

    public float payoutPenalty = 0.5f;


    public void Initialize()
    {
        queue = new List<Customer>();
        holdOver = new List<Customer>();
        done = new List<Customer>();
        todaysCustomers = new List<Customer>();
    }

    public Customer AddCustomer(float waitTime)
    {
        Traits t = new Traits();
        int tries = 0;
        int petIndex = Random.Range(0,Services.PetManager.pets.Count-1);
        while(Services.PetManager.pets[petIndex].hasOwner){
            tries++;
            petIndex++;
            if(petIndex >= Services.PetManager.pets.Count){
                petIndex = 0;
            }
            if(tries >= 20){
                break;
            }
        }
        t = Services.PetManager.pets[petIndex].traits;
        Customer cust = new Customer(t,waitTime,100);
        return cust;
    }

    public Customer AddCustomer(int needNum, float waitTime)
    {
        bool petFound = false;
        int petsSearched = 0;
        Traits t = new Traits();
        if (Services.PetManager.pets.Count > 0) {
            while (!petFound)
            {
                int z = Random.Range(0, Services.PetManager.pets.Count - 1);
                if (!Services.PetManager.pets[z].owned)
                {
                    t = Services.PetManager.pets[z].traits;
                    petFound = true;
                    Services.PetManager.pets[z].owned = true;
                }
                else
                {
                    petsSearched++;
                    if (petsSearched > Services.PetManager.pets.Count)
                    {
                        break;
                    }
                }
            }
        }


        if (!petFound)
        {
            if (needNum < 4)
            {
                for (int i = 0; i < needNum; i++)
                {
                    int x = Random.Range(1, 4);
                    if (x == 1 && t.hue == Hue.None)
                    {
                        t.hue = (Hue)Random.Range(1, 4);
                    }
                    else if (x == 2 && t.pattern == Pattern.None)
                    {
                        t.pattern = (Pattern)Random.Range(1, 5);
                    }
                    else if (x == 3 && t.shape == Shape.None)
                    {
                        t.shape = (Shape)Random.Range(1, 4);
                    }
                    else if (x == 4 && t.bait == Bait.None)
                    {
                        t.bait = (Bait)Random.Range(1, 5);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            else
            {
                t.RandomTraits();
            }
        }
        else
        {
            if (needNum < 4)
            {
                for (int i = 0; i < 4-needNum; i++)
                {
                    int x = Random.Range(1, 5);
                    if (x == 1 && t.hue != 0)
                    {
                        t.hue = 0;
                    }
                    else if (x == 2 && t.pattern != 0)
                    {
                        t.pattern = 0;
                    }
                    else if (x == 3 && t.shape != 0)
                    {
                        t.shape = 0;
                    }
                    else if (x == 4 && t.bait != 0)
                    {
                        t.bait = 0;
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }
        //Debug.Log(t);
        Customer cust = new Customer(t, waitTime, 100);
        return cust;
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

    public void CreateTodaysCustomers(int day)
    {
        done.Clear();


        int numOfCusts = standardAmountOfCustomersPerDay;
        if(day < amountOfCustomersInTheFirstWeek.Length)
        {
            numOfCusts = amountOfCustomersInTheFirstWeek[day];
        }

        int needNum = standardNeed;
        if(day < customerNeedinFirstWeek.Length)
        {
            needNum = customerNeedinFirstWeek[day];
        }

        needNum += Random.Range(-1, 1);
        needNum = Mathf.Clamp(needNum, 1, 4);

        float wait = standardWait + needNum * 0.7f;

        for(int i = 0; i < numOfCusts; i++)
        {
            Customer fuck = AddCustomer(needNum, wait);
            todaysCustomers.Add(fuck);
        }

        for(int i = 0; i < holdOver.Count; i++)
        {
            int k = Random.Range(0,todaysCustomers.Count-1);
            todaysCustomers.Insert(k, holdOver[i]);
        }


        holdOver.Clear();
    }
    
    public void LoadQueue()
    {
        if (todaysCustomers.Count > 0)
        {
            Customer cust = todaysCustomers[0];
            if (queue.Count > 0)
            {
                cust.linePosition = queue[queue.Count - 1].linePosition;
            }
            else
            {
                cust.linePosition = 0;
            }
            cust.inQueue = true;
            queue.Add(cust);
            todaysCustomers.Remove(cust);
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
    List<SpriteRenderer> spriteRenderers;
    public bool satisfied = false;
    public bool inQueue = false;
    public GameObject want;
    List<SpriteRenderer> wantSpriteRenderers;
    public Image timer;


    public Customer(Traits need,float maxWait,int linePos)
    {
        this.needs = need;
        this.maxWaitTime = maxWait;
        this.linePosition = linePos;
        inQueue = false;
        CreateVisual();
    }

    public void CreateVisual()
    {
        
        gameObject = GameObject.Instantiate(Services.GameController.CustomerPrefab, Services.GameController.CustomerLine.transform.position + new Vector3(100,0,0), Quaternion.identity, Services.GameController.CustomerLine.transform);
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());
        for(int i = 1; i < gameObject.transform.childCount;i++){
            spriteRenderers.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>());
        }
        spriteRenderers[0].sprite = Services.Visuals.customerBodies[Random.Range(0,4)];
        spriteRenderers[1].sprite = Services.Visuals.customerEyes[Random.Range(0,4)];
        spriteRenderers[2].sprite = Services.Visuals.customerMouths[Random.Range(0,4)];
        spriteRenderers[3].sprite = Services.Visuals.customerNoses[Random.Range(0,4)];

        timer = gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();

        gameObject.SetActive(false);
        GameObject gob = GameObject.Instantiate(Services.GameController.want, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        gob.transform.localPosition = new Vector3(-0.848f, -2.612f, 0);
        wantSpriteRenderers = new List<SpriteRenderer>();
        Want w = gob.GetComponent<Want>();
        foreach(SpriteRenderer s in w.spriteRenderers){
            wantSpriteRenderers.Add(s);
        }
        want = gob;
        gob.SetActive(false);
        if (needs.shape != Shape.None)
        {
            Debug.Log(needs.shape);
            wantSpriteRenderers[totalNeeds].sprite = Services.Visuals.wantShapes[(int)needs.shape-1];
            totalNeeds++;
            
        }
        if(needs.hue != Hue.None)
        {
            wantSpriteRenderers[totalNeeds].sprite = Services.Visuals.wantColors[(int)needs.hue-1];
            totalNeeds++;
        }

        if (needs.pattern != Pattern.None)
        {
            wantSpriteRenderers[totalNeeds].sprite = Services.Visuals.wantPatterns[(int)needs.pattern-1];
            totalNeeds++;
        }
        if (needs.bait != Bait.None)
        { 
            wantSpriteRenderers[totalNeeds].sprite = Services.Visuals.baits[(int)needs.bait-1];
            totalNeeds++;
        }
        w.SetSpaces(totalNeeds);
        for(var i = 0; i < wantSpriteRenderers.Count;i++){
            wantSpriteRenderers[i].enabled = false;
            if(i < totalNeeds){
                wantSpriteRenderers[i].enabled = true;
            }
        }
    }

    public void Update()
    {
        if (inQueue) {
            currentWaitTime += Time.deltaTime;
            linePosition = Services.CustomerManager.queue.IndexOf(this);
            timer.fillAmount = currentWaitTime / maxWaitTime;
        } 
        Vector3 goal = new Vector3(linePosition * Services.CustomerManager.customerDist, 0, 0);
        gameObject.SetActive(inQueue);
        if (inQueue)
        {
            gameObject.transform.localPosition = gameObject.transform.localPosition + (goal - gameObject.transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);


        }
        else
        {
            if (!satisfied)
            {
                gameObject.transform.localPosition = gameObject.transform.localPosition + (goal - gameObject.transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(100, 0, 0);
            }
        }

        if (currentWaitTime > maxWaitTime)
        {
            GameObject reaction = GameObject.Instantiate(Services.GameController.reactionPrefab,gameObject.transform.position,Quaternion.identity) as GameObject;
            leaveQueue();
        }
        if (linePosition == 0 ){
            want.SetActive(true);
            //Vector3 pos = new Vector3(0, 0.05f + (1) * Services.CustomerManager.textDist, 0);
            //want.transform.localPosition = want.transform.localPosition + (pos - want.transform.localPosition) * 0.05f * (Time.deltaTime / 0.016f);
 
        }
        else
        {
            want.SetActive(false);
        }
    }

    public void leaveQueue()
    {
        currentWaitTime = 0;
        this.payout *= Services.CustomerManager.payoutPenalty;

        for (int i = linePosition + 1;  i < Services.CustomerManager.queue.Count; i++)
        {
            Services.CustomerManager.queue[i].linePosition--;
            if(Services.CustomerManager.queue[i].linePosition == 0)
            {
                Services.CustomerManager.queue[i].maxWaitTime += 5f;
            }
        }

        if (!satisfied)
        {
            Services.CustomerManager.holdOver.Add(this);
            linePosition = -100;
            gameObject.transform.position = new Vector3(-100, 0, 0);

        }
        else
        {
            GameObject reaction = GameObject.Instantiate(Services.GameController.reactionPrefab,gameObject.transform.position,Quaternion.identity) as GameObject;
            reaction.GetComponent<Reaction>().happy  =true;
            Services.CustomerManager.done.Add(this);
            Services.DayManager.money += payout;
            GameObject.Destroy(gameObject);
        }
    }

    public void GotPet(Pet pet)
    {
        float happ = pet.traits.CompareTrait(needs);
        Debug.Log(happ);
        if (!pet.dead && happ > 1) {
            
            satisfied = true;
            leaveQueue();
            pet.gameObject.SetActive(false);
            Services.PetManager.pets.Remove(pet);
        }
        else
        {
            satisfied = false;
            leaveQueue();
        }

    }
}

