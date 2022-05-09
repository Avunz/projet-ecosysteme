using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    
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
        base.Update();
        if (currentAction == Enums.CurrentAction.GoingToEat)
        {
            navMeshAgent.speed = genes.RunSpeed;
            if(target != null)
            animator.SetFloat("Speed",1);

        }
        else
        {
            navMeshAgent.speed = genes.WalkSpeed;
            animator.SetFloat("Speed",0.5f);

        }
        if (currentAction == Enums.CurrentAction.Eating && target != null)
        {
            Chicken targetChicken = target.GetComponent<Chicken>();
            targetChicken.IsAlive = false;
            target = null;
            genes.HungerTimer = genes.MaxHunger;

        }

    }
    
}
