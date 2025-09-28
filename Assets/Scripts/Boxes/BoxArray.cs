using UnityEngine;

public class BoxArray : MonoBehaviour
{
    #region Grid Generation Variables
    [Header("Grid Generation Variables")]
    [SerializeField] private int gridSize = 5;
    [SerializeField] private float spacing = 1.41f;
    private GameObject[,] boxes;
    #endregion

    #region Wave Propagation Variables
    [Header("Wave Propagation Variables")]
    [Range(0, 360)]
    [Tooltip("Angle counterclockwise about the Y-axis in degrees for the propagation of the wave")]
    [SerializeField] private float angle = 0f;

    [Tooltip("Speed of propagation for the wave")]
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private float frequency = 1f;
    #endregion

    void Awake()
    {
        boxes = new GameObject[gridSize, gridSize];
    }

    void Start()
    {
        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
            {
                GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                box.transform.position = new Vector3(
                    i * spacing,
                    0,
                    j * spacing
                );
                boxes[i, j] = box;
            }
    }

    // Wave
    void Update()
    {
        // For each box
        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
            {
                GameObject box = boxes[i, j];

                // dir = <cos(angle), 0, sin(angle)>
                // aka direction of propagation
                Vector3 dir = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * angle),
                    0,
                    Mathf.Sin(Mathf.Deg2Rad * angle)
                );

                // r_pos is position of a node relative to local origin of this object
                Vector3 r_pos = box.transform.position - transform.position;

                // Calculate component of r_pos in the direction of dir
                float distance = Vector3.Dot(r_pos, dir) / dir.magnitude;

                // Use this distance as an input for a sinusoidal function
                // Use Time.time as an offset so that the wave propagates
                float height = Mathf.Sin(Time.time * speed + distance * frequency);

                // Use the sinusoidal function output to change the altitude of this box
                box.transform.position = new Vector3(
                    box.transform.position.x,
                    height,
                    box.transform.position.z
                );
            }
    }
}
