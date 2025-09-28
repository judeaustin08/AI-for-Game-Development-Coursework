using UnityEngine;

public class MoveBoxes : MonoBehaviour
{
    [SerializeField] private int numBoxes = 10;
    [SerializeField] private float spacing = 1.41f;
    [SerializeField] private GameObject[] boxes;

    void Awake() {
        boxes = new GameObject[numBoxes];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        for (int i = 0; i < numBoxes; i++) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boxes[i] = box;
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < numBoxes; i++) {
            float wave = Mathf.Tan(Time.time + i);
            boxes[i].transform.position = new Vector3(
                spacing * i,
                wave,
                0
            );
        }
    }
}
