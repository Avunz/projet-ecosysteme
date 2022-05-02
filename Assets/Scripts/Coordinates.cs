using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Structs are passed by value, unlike classes (which are passed by reference)
public struct Coordinates
{
    public int x, z;

    public Coordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public Coordinates Front()
    {
        return new Coordinates(0, 1);
    }
    
    
    public Coordinates Back()
    {
        return new Coordinates(0, 1);
    }
    
    
    public Coordinates Left()
    {
        return new Coordinates(-1, 0);
    }
    
    
    public Coordinates Right()
    {
        return new Coordinates(1, 0);
    }

    public float DistanceBetweenCoords(Coordinates start, Coordinates finish)
    {
        float distanceSquared = (finish.x - start.x) * (finish.x - start.x) + (finish.z - start.z) * (finish.z - start.z);
        return Mathf.Sqrt(distanceSquared);
    }
    
    public override string ToString()
    {
        return "Position: " + x + ", " + z;
    }
}
