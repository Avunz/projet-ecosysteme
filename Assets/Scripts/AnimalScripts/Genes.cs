using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes
{
    private float visionRange, walkSpeed, runSpeed, hungerTimer, thirstTimer,
        energy, maxEnergy, maxHunger, maxThirst, gestationDuration;
    private const float mutationChance = 0.15f;

    private bool isPregnant;

    public Genes(float visionRange, float walkSpeed, float runSpeed, float maxHunger, float maxThirst, float maxEnergy, float gestationDuration)
    {
        this.visionRange = visionRange;
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.maxHunger = maxHunger;
        this.maxThirst = maxThirst;
        this.maxEnergy = maxEnergy;
        this.gestationDuration = gestationDuration;

        hungerTimer = maxHunger;
        thirstTimer = maxThirst;
        energy = maxEnergy;

        isPregnant = false;

    }



    public float VisionRange
    {
        get => visionRange;
        set => visionRange = value;
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public float HungerTimer
    {
        get => hungerTimer;
        set => hungerTimer = value;
    }

    public float ThirstTimer
    {
        get => thirstTimer;
        set => thirstTimer = value;
    }

    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }

    public float Energy
    {
        get => energy;
        set => energy = value;
    }

    public float MaxHunger
    {
        get => maxHunger;
        set => maxHunger = value;
    }

    public float MaxThirst
    {
        get => maxThirst;
        set => maxThirst = value;
    }
    
}
