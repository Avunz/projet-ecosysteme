using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Chick, Chicken1, Chicken2, Rooster, Fox, FemaleFox;

    private Chicken chicken;
    private Fox fox;

    private Genes chickenBaseGenes, foxBaseGenes;
    void Start()
    {
        chickenBaseGenes = new Genes(7f, 1.5f, 2.5f, 60f, 50f, 70f, 55f);
        foxBaseGenes = new Genes(10f, 11f, 3f, 60f, 50f, 70f, 55f);

        DontDestroyOnLoad(gameObject);
        chicken = Chick.GetComponent<Chicken>();
        for (int index = 0; index < 10; index++)
        {
            Instantiate(Chick, new Vector3(-1, 2.1f, 9), Quaternion.identity);
            chicken.Genes = chickenBaseGenes;




        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
