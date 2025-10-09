using UnityEngine;

[CreateAssetMenu(fileName = "Flee Behaviour", menuName = "AI Behaviours/Behaviours/New Flee Behaviour")]
public class FleeBehaviour : AIBehaviour
{
    private Transform target;
    [SerializeField] private float pathDistance;

    public override void Initialize(GameObject parent)
    {
        this.parent = parent;
        target = parent.GetComponent<NPC>().target;
    }

    public override Vector3 SelectTarget()
    {
        Vector3 dir2T = (target.position - parent.transform.position).normalized;
        return parent.transform.position - dir2T * pathDistance;
    }

    public override void Gizmos()
    {
        // No gizmos
    }
}