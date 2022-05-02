using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Enums.Species species;
    protected bool isAlive;
    public Coordinates coordinates;

    public void InitiateCreature(Coordinates coordinates)
    {
        this.coordinates = coordinates;
        transform.position = new Vector3(coordinates.x, 1, coordinates.z);
    }

    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }
}
