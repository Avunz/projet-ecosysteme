using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    private GameObject hunter;

    void Start()
    {
        //Debug.LogWarning(instance.name);
        foodTag = "FoodLand";
        genes = new Genes(7f, 1.6f, 2.5f, 70f, 70f, 70f, 55f);
        base.Start();

        navMeshAgent.angularSpeed = 100;

    }

    void Update()
    {
        hunter = ScanWithTag("Fox");
        if (hunter != null)
        {
            Flee();
            animator.SetFloat("Speed",1);
            return;
        }
        animator.SetFloat("Speed",0.5f);

        base.Update();
        

    }

    private void Flee()
    {
        navMeshAgent.speed = genes.RunSpeed;

        Vector3 directionToHunter = transform.position - hunter.transform.position;

        Vector3 targetPosition = transform.position + directionToHunter*1.5f;
        navMeshAgent.SetDestination(targetPosition);
    }
    private GameObject ScanWithTag(string tag)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, genes.VisionRange);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.tag.Equals("Fox"))
            {
                return collider.gameObject;
                //Fox fox = collider.gameObject.GetComponent<Fox>();
                //if (fox == this)
                //return fox.gameObject;
            }
        }

        return null;

    }

}
