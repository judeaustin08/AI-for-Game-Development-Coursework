using UnityEngine;

public class BoxCircle : MonoBehaviour
{
    [SerializeField] private int n = 8;
    [SerializeField] private float radius = 5f;
    private GameObject[] boxes;

    void Awake()
    {
        boxes = new GameObject[n];
    }

    void Start()
    {
        for (int i = 0; i < n; i++)
        {
            float a = 2f * Mathf.PI * i / n;

            // Create box
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = new Vector3(
                radius * Mathf.Cos(a),
                0,
                radius * Mathf.Sin(a)
            );
            box.transform.rotation = Quaternion.Euler(
                0,
                a * Mathf.Rad2Deg,    // Angle is in radians, must be in degrees for transform.rotation
                0
            );

            // Save box to array
            boxes[i] = box;
        }
    }

    // Wave
    void Update()
    {
        // Iterate through boxes
        for (int i = 0; i < n; i++)
        {
            GameObject box = boxes[i];

            float height = Mathf.Sin(Time.time + i);
            box.transform.position = new Vector3(
                box.transform.position.x,
                height,
                box.transform.position.z
            );
        }
    }
}
