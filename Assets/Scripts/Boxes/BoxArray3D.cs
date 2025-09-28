using UnityEngine;

public class BoxArray3D : MonoBehaviour
{
    [SerializeField] private int gridSize = 5;
    [SerializeField] private float spacing = 1.41f;
    private GameObject[,,] boxes;

    void Awake()
    {
        boxes = new GameObject[gridSize, gridSize, gridSize];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
                for (int k = 0; k < gridSize; k++)
                {
                    GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    box.transform.position = new Vector3(i, j, k) * spacing;
                    boxes[i, j, k] = box;
                }
    }
}
