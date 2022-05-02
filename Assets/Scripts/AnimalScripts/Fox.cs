using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    // Start is called before the first frame update
    
    void Start()
    {
        //Debug.LogWarning(instance.name);
        foodTag = "Chicken";
        genes = new Genes(10f, 1.5f, 2.9f, 60f, 50f, 70f,55f);
        base.Start();
        
        navMeshAgent.angularSpeed = 100;
        
    }

    void Update()
    {
        if (currentAction == Enums.CurrentAction.GoingToEat) navMeshAgent.speed = genes.RunSpeed;
        else navMeshAgent.speed = genes.WalkSpeed;
        if (currentAction == Enums.CurrentAction.Eating && target != null)
        {
            Chicken targetChicken = target.GetComponent<Chicken>();
            targetChicken.IsAlive = false;
            target = null;
            genes.HungerTimer = genes.MaxHunger;

        }
        base.Update();
    }
    
}
