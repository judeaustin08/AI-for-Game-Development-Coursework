using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName="Patrol Behaviour", menuName="AI Behaviours/Behaviours/New Patrol Behaviour")]
public class PatrolBehaviour : AIBehaviour
{
    private Vector3 initialPosition;

    [SerializeField] private float patrolRadius = 10;
    [SerializeField] private bool drawPatrolRadius = false;

    // Call base constructor to set constraints
    public override void Initialize(GameObject parent)
    {
        this.parent = parent;
        initialPosition = parent.transform.position;
    }

    public override Vector3 SelectTarget()
    {
        Vector3 rand = initialPosition + Random.insideUnitSphere * patrolRadius;
        return new Vector3(
            rand.x,
            parent.transform.position.y,
            rand.z
        );
    }

    public override void Gizmos()
    {
        if (drawPatrolRadius)
            Handles.DrawWireDisc(initialPosition, Vector3.up, patrolRadius, 0.5f);
    }
}