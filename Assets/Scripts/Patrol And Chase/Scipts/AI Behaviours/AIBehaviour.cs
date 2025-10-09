using Pathfinding;
using UnityEngine;

// Set up to work with the Unity AstarPathfindingProject by Aron Granberg
public abstract class AIBehaviour : ScriptableObject
{
    protected GameObject parent;
    protected NNConstraint constraint = new();
    public float minimumInterval;
    public float randomInterval;
    public bool continuous = false;
    public float speedModifier = 0;
    public float multiplicativeSpeedMultiplier = 1;
    public abstract Vector3 SelectTarget();
    public abstract void Gizmos();
    public abstract void Initialize(GameObject parent);

    // Set global variables
    public AIBehaviour()
    {
        // Return only walkable nodes
        constraint.constrainWalkability = true;
    }
}
