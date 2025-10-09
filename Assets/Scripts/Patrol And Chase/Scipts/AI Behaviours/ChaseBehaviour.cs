using UnityEngine;

[CreateAssetMenu(fileName="Chase Behaviour", menuName="AI Behaviours/Behaviours/New Chase Behaviour")]
public class ChaseBehaviour : AIBehaviour
{
    public string sightPropertyName = "_SeeingTarget";

    private Vector3 lastKnownPosition;
    private Transform target;
    private Vector3 t_pos;

    public override void Initialize(GameObject parent)
    {
        this.parent = parent;
        target = parent.GetComponent<NPC>().target;
    }

    /*
    Returns the last known position of the target. The last known position is updated if:
     - The NPC can see the target
     - The target is within a very close distance of the target (NPC can hear)
    */
    public override Vector3 SelectTarget()
    {
        t_pos = new(
            target.position.x,
            parent.transform.position.y,
            target.position.z
        );

        // If the NPC can see the player, using the sight property from the NPC class
        if ((bool)typeof(NPC).GetProperty(sightPropertyName).GetValue(parent.GetComponent<NPC>()))
            lastKnownPosition = t_pos;

        return lastKnownPosition;
    }

    public override void Gizmos()
    {
        // No gizmos
    }
}