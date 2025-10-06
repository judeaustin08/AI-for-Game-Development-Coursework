using UnityEngine;

public class Runtime : MonoBehaviour
{
    [SerializeField] private float spawnLife = 2;

    void Update()
    {
        // If input pressed, send a raycast to get the position of the new object
        // Store the hit data in a new RaycastHit object called hit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit)) {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // Move the object back by half of its world space size to prevent collision
            obj.transform.position = hit.point - new Vector3(0, 0, .5f);

            // Destroy object after a delay
            Destroy(obj, spawnLife);
        }
    }
}
