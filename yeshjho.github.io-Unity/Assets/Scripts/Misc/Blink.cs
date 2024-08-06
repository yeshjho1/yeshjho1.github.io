using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private float blinkPeriod = 1f;
    [SerializeField] private bool startVisible = true;

    [SerializeField] private MonoBehaviour componentToBlink;
    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        componentToBlink.enabled = startVisible;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkPeriod)
        {
            componentToBlink.enabled = !componentToBlink.enabled;
            timer = 0;
        }
    }
}
