using UnityEngine;

public class Dog : NPC
{
    public Meat meat;

    private bool distracted;

    protected override void Update()
    {
        if (distracted)
        {
            MoveToMeat();
            return;
        }

        base.Update(); // still handles movement if needed

        if (!isInspecting)
            StartInspection("sniffing...");
    }

    public void Distract()
    {
        distracted = true;
        isInspecting = false;
    }

    void MoveToMeat()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            meat.transform.position,
            walkSpeed * Time.deltaTime
        );
    }
}