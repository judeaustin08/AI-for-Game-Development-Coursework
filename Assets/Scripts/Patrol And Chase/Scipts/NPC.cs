using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[AddComponentMenu("AI Behaviours/NPC")]
public class NPC : MonoBehaviour
{
    [Header("Behaviours")]

    [SerializeField] private ConstrainedTrigger[] triggers;

    [Tooltip("Order these in descending order of priority. The first behaviour that fits the constraints will be chosen for pathfinding.")]
    [SerializeField] private MovementBehaviour[] movementBehaviours;

    private int activeBehaviour = -1;
    private int checkActiveBehaviour = -1;
    private float timer;
    private readonly System.Random rand = new();

    [Header("NPC")]

    public Transform target;
    private Vector3 eyePos;
    private Vector3 targetAtEyeHeight;

    [SerializeField] private float health = 100;
    public float _Health => health;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float height = 0.5f;

    private bool chasing = false;
    public bool _Chasing
    {
        get { return chasing; }
        set { chasing = value; }
    }

    [SerializeField] private float seeingDistance = 10;
    [SerializeField] private LayerMask obstacleLayer;
    private bool seeingTarget = false;
    public bool _SeeingTarget
    {
        get { return seeingTarget; }
        set { seeingTarget = value; }
    }

    private Seeker seeker;
    private Path path;

    private int currentWaypoint = 0;
    [SerializeField] private float nextWaypointDistance = 3;

    [SerializeField] private bool drawGizmos = false;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    private void OnEnable()
    {
        seeker.pathCallback += OnPathComplete;
    }

    private void OnDisable()
    {
        seeker.pathCallback -= OnPathComplete;
    }

    /*
    Initialize all of the constraints with the reference to this gameObject so the constraint can
    access the object properties
    */
    private void Start()
    {
        foreach (MovementBehaviour b in movementBehaviours)
        {
            b.behaviour.Initialize(gameObject);

            foreach (Constraint c in b.constraints)
                c.Initialize(gameObject);
        }

        foreach (ConstrainedTrigger t in triggers)
        {
            t.trigger.Initialize(this);

            foreach (Constraint c in t.constraints)
                c.Initialize(gameObject);
        }
    }

    /*
    Go down the list of behaviours and make sure the correct one is selected before pathfinding 
    with the correct targeting behaviour
    */
    private void Update()
    {
        /*
        Check if NPC can see target
        Returns true if the raycast hits no obstacles (returns false), false if the raycast 
        hits obstacles (returns true)
         > ie seeingTarget = !Raycast
        */
        eyePos = transform.position + Vector3.up * height;
        targetAtEyeHeight = transform.position + Vector3.up * height;
        seeingTarget = !Physics.Raycast(
            eyePos,
            target.position - eyePos,
            seeingDistance,
            obstacleLayer
        );

        // Invoke all applicable triggers
        foreach (ConstrainedTrigger t in triggers)
            if (Constraint.Evaluate(t.constraints))
                t.trigger.Invoke();
        
        // Check whether behaviour has changed
        activeBehaviour = GetActiveBehaviour();

        // Return early if no behaviour is selected
        if (activeBehaviour == -1)
        {
            Debug.Log("No AI behaviour - selecting a behaviour");
            return;
        }

        // Generate new path if AI Behaviour changed
        if (activeBehaviour != checkActiveBehaviour)
        {
            Debug.Log("New AI behaviour - generating new path");
            checkActiveBehaviour = activeBehaviour;
            StartCoroutine(StartPath());
        }

        AIBehaviour active = movementBehaviours[activeBehaviour].behaviour;

        timer += Time.deltaTime;
        // Request new path after an interval if defined by behaviour, do not erase previous path during generation
        if (active.continuous && timer > active.minimumInterval)
        {
            timer = 0;
            StartCoroutine(StartPath(false));
        }

        // Follow path, using whether path exists as a status variable
        if (path != null)
        {
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            Vector3 movement = (speed + active.speedModifier) * active.multiplicativeSpeedMultiplier * Time.deltaTime * dir;
            Debug.Log(string.Format(
                "Waypoint {0} = {1}\nMovement vector = {2}",
                currentWaypoint,
                path.vectorPath[currentWaypoint],
                movement
            ));
            transform.Translate(movement);

            // Complete all waypoints which are close enough
            while (true)
            {
                float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                        currentWaypoint++;
                    else
                    {
                        Debug.Log("Finished path - generating new path");
                        StartCoroutine(StartPath());
                        break;
                    }
                else break;
            }
        }
    }

    private int GetActiveBehaviour()
    {
        for (int i = 0; i < movementBehaviours.Length; i++)
            if (Constraint.Evaluate(movementBehaviours[i].constraints))
                return i;  // Stop loop when the first viable AI behaviour is found

        return -1;
    }

    private IEnumerator StartPath(bool reset = true)
    {
        if (reset)
            path = null;    // Reset path like a status variable

        if (activeBehaviour != -1)
        {
            AIBehaviour active = movementBehaviours[activeBehaviour].behaviour;
            // Get target and path to target
            Vector3 targetPosition = active.SelectTarget();
            yield return new WaitForSeconds(active.minimumInterval + (float)(rand.NextDouble() * active.randomInterval));
            seeker.StartPath(transform.position, targetPosition);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (p.error == true) return;

        path = p;
        currentWaypoint = 0;    // Reset to avoid IndexOutOfBounds errors
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !drawGizmos) return;

        if (0 <= activeBehaviour && activeBehaviour < movementBehaviours.Length)
            movementBehaviours[activeBehaviour].behaviour.Gizmos();
    }
}

[System.Serializable]
public struct MovementBehaviour
{
    public AIBehaviour behaviour;
    public Constraint[] constraints;
}

[System.Serializable]
public struct ConstrainedTrigger
{
    public Trigger trigger;
    public Constraint[] constraints;
}