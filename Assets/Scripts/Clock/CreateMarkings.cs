using UnityEngine;

public class CreateMarkings : MonoBehaviour
{
    [SerializeField] private GameObject markingPrefab;
    [SerializeField] private float distance = 4f;

    void Start()
    {
        for (int angle = 0; angle < 360; angle += 30)
        {
            float rads = Mathf.Deg2Rad * angle;
            Instantiate(
                markingPrefab,
                new Vector3(
                    Mathf.Cos(rads),
                    Mathf.Sin(rads),
                    0
                ) * distance,
                Quaternion.Euler(0, 0, angle),
                transform
            );
        }
    }
}
