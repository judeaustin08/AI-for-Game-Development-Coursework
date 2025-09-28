using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Generator : MonoBehaviour
{
    [SerializeField] private int size = 11;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float heightMultiplier = 1f;   // Initial height multiplier
    private float _heightMultiplier;    // Variable height multiplier

    private Vector3[] vertices;
    private int[] tris;

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Awake() {
        // Initialize the 2D array with a variable size
        vertices = new Vector3[size * size];
        tris = new int[(size - 1) * (size - 1) * 6];

        meshFilter = GetComponent<MeshFilter>();
    }

    void Start() {
        // Generate vertex data
        for (int i = 0; i < vertices.Length; i++)
        {
            int x = i % size;
            int z = i / size;

            // Create a new point and set it to the correct position
            Vector3 vertex = new Vector3(
                x * spacing,
                0,
                z * spacing
            );

            // Save the point to its corresponding position in the array so it can be modified later
            vertices[i] = vertex;
        }
        
        // Generate tris
        for (int i = 0; i < size - 1; i++) {
            for (int j = 0; j < size - 1; j++) {
                int t = (i * (size - 1) + j) * 6;   // Iterator for tris
                int v = i * size + j;               // Iterator for vertices

                tris[t] = v;
                tris[t + 1] = v + size;
                tris[t + 2] = v + size + 1;

                tris[t + 3] = v;
                tris[t + 4] = v + size + 1;
                tris[t + 5] = v + 1;
            }
        }

        // Create new mesh
        mesh = new Mesh {
            vertices = vertices,
            triangles = tris
        };
        // Recalculate normals after updating the vertices for lighting
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    void Update() {
        // Update height multiplier based on a sinusoidal function
        _heightMultiplier = heightMultiplier * Mathf.Sin(Time.time);

        // Update the height of each point in the mesh
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            
            float y = (Mathf.Pow(worldPos.x, 2) + Mathf.Pow(worldPos.z, 2)) * _heightMultiplier;
            vertices[i].y = y;
        }
        
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }
}
