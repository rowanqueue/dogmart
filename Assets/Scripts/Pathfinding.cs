using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{

}
public class Location{
    public Vector2Int node;
    public float gscore;
    public float hscore;
    public float fscore => gscore+hscore;
    public Location parent;
    public Location(Vector2Int node, float gscore, float hscore){
        this.node = node;
        this.gscore = gscore;
        this.hscore = hscore; 
    }
    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;
        Location c = obj as Location ;
        if ((System.Object)c == null)
            return false;
        return node == c.node;
    }
    public bool Equals(Location c)
    {
        if ((object)c == null)
            return false;
        return node == c.node;
    }
}
public class AStarSearch
{
    public Vector2Int finalGoal;
    public Stack<Vector2Int> steps = new Stack<Vector2Int>();
    public Dictionary<Vector2Int,Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
    public Dictionary<Vector2Int,float> costSoFar = new Dictionary<Vector2Int, float>();
    int[] numbers;
    Vector2Int[] directions = new Vector2Int[]{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    //new Vector2Int[]{Vector2Int.up,new Vector2Int(1,1),Vector2Int.right, new Vector2Int(1,-1),Vector2Int.down,new Vector2Int(-1,-1),Vector2Int.left, new Vector2Int(-1,1)};
    static public float Heuristic(Vector2Int a, Vector2Int b){
        return Vector2Int.Distance(a,b);
    }
    public AStarSearch(Vector2Int start, Vector2Int goal){
        if(goal == new Vector2Int(1,1) || goal == new Vector2Int(1,0)){
            return;
        }
        numbers = new int[directions.Length];
        for(var i = 0; i < numbers.Length;i++){
            numbers[i] = i;
        }
        finalGoal = goal;
        if(start == goal){
            return;
        }
        /*if(Services.GameController.nodes.Contains(goal) == false){
            return;
        }*/

        List<Location> openList = new List<Location>();
        List<Location> closedlist = new List<Location>();
        openList.Add(new Location(start,0,Heuristic(start,goal)));
        do{
            openList.Sort((s1, s2) => s1.fscore.CompareTo(s2.fscore));
            var currentLocation = openList[0];
            closedlist.Add(currentLocation);
            openList.Remove(currentLocation);
            if(currentLocation.node == goal){
                //YOU FOUND IT
                steps = new Stack<Vector2Int>();
                while(ReferenceEquals(currentLocation.parent,null) == false){
                    //while you have a parent, add to steps
                    steps.Push(currentLocation.node);
                    currentLocation = currentLocation.parent;
                }
                break;
            }
            ShuffleNumbers();
            for(var i = 0; i < numbers.Length; i++){
                if(Services.Grid.InGrid(currentLocation.node) == false){
                    continue;
                }
                Vector2Int currentNode = currentLocation.node;
                Vector2Int neighbor = currentNode + directions[numbers[i]];

                if(Services.Grid.InGrid(neighbor) == false){continue;}
                bool hasOtherAtPosition = false;
                foreach(Pet p in Services.PetManager.pets){
                    if(p.gridPosition == neighbor || p.nextPosition == neighbor){
                        hasOtherAtPosition = true;
                        break;
                    }
                }
                bool hasPeg = false;
                foreach(Peg p in Services.PetManager.pegs){
                    if(p.gridPosition == neighbor){
                        hasPeg = true;
                        break;
                    }
                }
                Location newLocation = new Location(neighbor,0,Heuristic(neighbor,goal)*(hasOtherAtPosition ? 2f : 1)*(hasPeg ? 3f : 1));
                if(closedlist.Contains(newLocation)){continue;}
                int index = openList.IndexOf(newLocation);
                if(index == -1){
                    //its not in the open list
                    newLocation.parent = currentLocation;
                    newLocation.gscore = currentLocation.gscore+1;
                    openList.Add(newLocation);
                }else{
                    newLocation = openList[index];
                    if(currentLocation.gscore+1 < newLocation.gscore){
                        newLocation.gscore = currentLocation.gscore+1;
                    }
                }
            }

        }while(openList.Count > 0);
    }
    void ShuffleNumbers(){
        for (int i = 0; i < numbers.Length; i++)
        {
            int rnd = Random.Range(0, numbers.Length);
            int temp = numbers[rnd];
            numbers[rnd] = numbers[i];
            numbers[i] = temp;
        }
    }
}
