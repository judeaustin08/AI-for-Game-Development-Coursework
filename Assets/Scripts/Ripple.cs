using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Ripple : MonoBehaviour
{
    public int meshSize = 10;
    public Vector3[] vertices;

    void Awake() {
        vertices = new Vector3[meshSize + 1];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
