using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject h_hand;
    [SerializeField] private GameObject m_hand;
    [SerializeField] private GameObject s_hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DateTime now = DateTime.Now;
        Debug.Log(string.Format("{0}:{1}:{2}", now.Hour, now.Minute, now.Second));
        updateHand(h_hand.transform, now.Hour, 12);
        updateHand(m_hand.transform, now.Minute);
        updateHand(s_hand.transform, now.Second);
    }

    void updateHand(Transform hand, int pos, int cycle = 60)
    {
        float rot = 360f * pos / cycle;
        hand.rotation = Quaternion.Euler(0, 0, -rot);
    }
}
