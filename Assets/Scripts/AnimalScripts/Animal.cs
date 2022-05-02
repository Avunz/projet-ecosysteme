using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Animal : Creature
{
    public Array actions = Enum.GetValues(typeof(Enums.CurrentAction));

    protected Enums.Species predator, meal;
    protected Enums.CurrentAction currentAction = Enums.CurrentAction.Idle;

    public Transform model;
    public NavMeshAgent navMeshAgent;
    public Animator animator;

    //public float hungerBar, thirstBar, energyBar, speed, maxHunger, maxThirst, maxEnergy;
    protected int criticalHungerCount = 0, criticalThirstCount = 0, criticalDangerCount = 0, criticalPregnancyDangerCount = 0;
    protected float actionTimer;

    protected string foodTag;

    protected bool inAction = false,
        hasParents = false,
        isPregnant = false,
        hasMated = false,
        hasDestination = false,
        needsTarget = false, isAdult;

    protected Vector3 lastPosition;
    protected GameObject instance, target;

    protected Genes genes;
    // Update is called once per frame
    protected void Start()
    {
        actionTimer = 3f;
        isAlive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = true;

        navMeshAgent.stoppingDistance = 1.2f;
        navMeshAgent.autoBraking = true;
        model = GetComponent<Transform>();
    }

    protected void Update()
    {
        //Update meters
        float timePassed = Time.deltaTime;
        genes.HungerTimer -= timePassed*2;
        //genes.ThirstTimer -= timePassed*3;
        //genes.Energy -= timePassed;
        
        if (target != null&& !DestinationReached())
        {
            navMeshAgent.destination = target.transform.position;
            hasDestination = true;
            inAction = true;
        }
        PerformAction(currentAction);
        if (DestinationReached())
        {
            if (currentAction == Enums.CurrentAction.GoingToEat)
            {
                currentAction = Enums.CurrentAction.Eating;
                PerformAction(currentAction);
                return;

            }
            else if (currentAction == Enums.CurrentAction.GoingToDrink)
            {
                currentAction = Enums.CurrentAction.Drinking;
                PerformAction(currentAction);
                return;

            }
                

            hasDestination = false;
            navMeshAgent.speed = 0;
            inAction = false;

        }
        

        
        if (genes.HungerTimer <= 0 || genes.ThirstTimer<=0)
            isAlive = false;
        

        if (!isAlive)
            Destroy(gameObject);

        
        if (!inAction&&!hasDestination && target == null)
        { 
            
            //Update action
            currentAction = SelectNewAction();
            Debug.Log(currentAction);
            if (currentAction == Enums.CurrentAction.Exploring)
            {
                navMeshAgent.speed = genes.WalkSpeed;
                Vector3 targetDestination = RandomSphere(transform.position, UnityEngine.Random.Range(genes.VisionRange*0.5f, genes.VisionRange));
                if (CheckPath(targetDestination))
                {
                    navMeshAgent.destination = targetDestination;
                    inAction = true;
                    hasDestination = true;
                    needsTarget = false;
                }
            }
             else if (currentAction == Enums.CurrentAction.GoingToEat)
            {
                inAction = true;

                navMeshAgent.destination = FindClosestWithTag(foodTag);
                navMeshAgent.speed = genes.WalkSpeed;
                
            }

             else if (currentAction == Enums.CurrentAction.GoingToDrink)
             {
                 inAction = true;
                 navMeshAgent.destination = FindClosestWithTag("Water");
                 navMeshAgent.speed = genes.WalkSpeed;
                 
             }
             
             if (currentAction == Enums.CurrentAction.Eating)
                 PerformAction(Enums.CurrentAction.Eating);
             if (currentAction == Enums.CurrentAction.Drinking)
                 PerformAction(Enums.CurrentAction.Drinking);







        }


        

        
    }



    protected bool CheckPath(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(destination,path);
        if (path.status != NavMeshPathStatus.PathComplete) return false;

        return true;
    }
    protected Enums.CurrentAction SelectNewAction()
    {


        
        
            Random random = new Random();
            if (genes.HungerTimer < genes.MaxHunger * 0.5f)
                return Enums.CurrentAction.GoingToEat;
             else if (genes.ThirstTimer < genes.MaxThirst * 0.5f)
                return Enums.CurrentAction.GoingToDrink;
             else if (genes.Energy < genes.MaxEnergy * 0.3f)
                return Enums.CurrentAction.Idle;
             else return Enums.CurrentAction.Exploring;
             
             

        //return (Enums.CurrentAction) actions.GetValue(random.Next(actions.Length));
    }
    protected Vector3 RandomSphere(Vector3 origin, float range)
    {
        Vector3 direction = UnityEngine.Random.insideUnitSphere * range;
        

        direction += origin;
        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, range, NavMesh.AllAreas);
        //Debug.LogError(hit.position + " DISTANCE: " + Vector3.Distance(hit.position,origin));
        return hit.position;

    }

    /*protected Vector3 FindClosestWithTag(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, genes.VisionRange );
        ArrayList validObjects = new ArrayList();
        Transform closestObject;
                
        foreach(var collider in colliders)
            if (collider.gameObject.CompareTag(tag))
                validObjects.Add(collider.transform);

        Debug.LogWarning(validObjects.Count+" FOUND");
        if (validObjects.Count == 0) //FindClosestWithTag(tag);
        {
            needsTarget = true;

            return transform.position;
        }
        else needsTarget = false;
                
        closestObject = (Transform)validObjects[0];

        hasDestination = true;
        foreach(Transform gameObject in validObjects)
            if (Vector3.Distance(transform.position, gameObject.position) >
                Vector3.Distance(transform.position, closestObject.position))
            {
                closestObject = gameObject;
                //hasDestination = true;
            }

        Debug.LogWarning(closestObject.position);
        target = closestObject.gameObject;
        return closestObject.position;
    }
    */
    protected Vector3 FindClosestWithTag(string tag)
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float distance = Mathf.Infinity;

        foreach (GameObject listedObject in objects)
        {
            Vector3 line = listedObject.transform.position - transform.position;

            float lineDistance = line.sqrMagnitude;

            if (lineDistance < distance)
            {
                closestObject = listedObject;
                distance = lineDistance;
            }
        }

        target = closestObject;
        return closestObject.transform.position;
    }


    protected void PerformAction(Enums.CurrentAction action)
    {
        if (currentAction == Enums.CurrentAction.Exploring || currentAction == Enums.CurrentAction.GoingToEat ||
            currentAction == Enums.CurrentAction.GoingToDrink) return;
        inAction = true;
        navMeshAgent.speed = 0;
        
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0)
        {
                if (currentAction == Enums.CurrentAction.Eating)
                    genes.HungerTimer = genes.MaxHunger;
                else if (currentAction == Enums.CurrentAction.Drinking)
                {
                    genes.ThirstTimer = genes.MaxThirst;
                    Debug.LogError(genes.ThirstTimer);

                }

                inAction = false;
            actionTimer = 2.5f;
            currentAction = Enums.CurrentAction.Idle;

            target = null;
            hasDestination = false;
            
        }
        
    }

    protected bool DestinationReached()
    {
        if (!navMeshAgent.pathPending)
            if(navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance)
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    lastPosition = transform.position;
                    return true;
                }
        
        
        return false;
    }

    public Genes Genes
    {
        get => genes;
        set => genes = value;
    }

    public Array Actions
    {
        get => actions;
        set => actions = value;
    }

    public Enums.Species Predator
    {
        get => predator;
        set => predator = value;
    }

    public Enums.Species Meal
    {
        get => meal;
        set => meal = value;
    }

    public Enums.CurrentAction CurrentAction
    {
        get => currentAction;
        set => currentAction = value;
    }

    public Transform Model
    {
        get => model;
        set => model = value;
    }

    public NavMeshAgent NavMeshAgent
    {
        get => navMeshAgent;
        set => navMeshAgent = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public int CriticalHungerCount
    {
        get => criticalHungerCount;
        set => criticalHungerCount = value;
    }

    public int CriticalThirstCount
    {
        get => criticalThirstCount;
        set => criticalThirstCount = value;
    }

    public int CriticalDangerCount
    {
        get => criticalDangerCount;
        set => criticalDangerCount = value;
    }

    public int CriticalPregnancyDangerCount
    {
        get => criticalPregnancyDangerCount;
        set => criticalPregnancyDangerCount = value;
    }

    public float ActionTimer
    {
        get => actionTimer;
        set => actionTimer = value;
    }

    public string FoodTag
    {
        get => foodTag;
        set => foodTag = value;
    }

    public bool InAction
    {
        get => inAction;
        set => inAction = value;
    }

    public bool HasParents
    {
        get => hasParents;
        set => hasParents = value;
    }

    public bool IsPregnant
    {
        get => isPregnant;
        set => isPregnant = value;
    }

    public bool HasMated
    {
        get => hasMated;
        set => hasMated = value;
    }

    public bool HasDestination
    {
        get => hasDestination;
        set => hasDestination = value;
    }

    public bool NeedsTarget
    {
        get => needsTarget;
        set => needsTarget = value;
    }

    public bool IsAdult
    {
        get => isAdult;
        set => isAdult = value;
    }

    public Vector3 LastPosition
    {
        get => lastPosition;
        set => lastPosition = value;
    }

    public GameObject Instance
    {
        get => instance;
        set => instance = value;
    }

    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    public Genes Genes1
    {
        get => genes;
        set => genes = value;
    }

    public Enums.Species Species
    {
        get => species;
        set => species = value;
    }

    public bool IsAlive1
    {
        get => isAlive;
        set => isAlive = value;
    }

    public Coordinates Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }
}
