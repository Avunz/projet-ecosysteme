using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chick, Chicken1, Chicken2, Rooster, Fox, FemaleFox;

    private Chicken chicken;
    private Fox fox;

    private Genes chickenBaseGenes, foxBaseGenes;
    void Start()
    {
        chickenBaseGenes = new Genes(7f, 1.5f, 2.5f, 60f, 50f, 70f, 55f);
        foxBaseGenes = new Genes(10f, 11f, 3f, 60f, 50f, 70f, 55f);

        DontDestroyOnLoad(gameObject);
        chicken = Chicken1.GetComponent<Chicken>();
        for (int index = 0; index < -1; index++)
        {
            Instantiate(Chicken1, new Vector3(-15, 2, 20), Quaternion.identity);
            chicken.Genes = chickenBaseGenes;




        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
