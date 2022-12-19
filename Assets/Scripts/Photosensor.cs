using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photosensor : MonoBehaviour
{
    public static Photosensor inst;

    [SerializeField] float sensorDistance;

    bool isActivated;

    void Awake()
    {
        inst = this;
    }

    public void Activate()
    {
        isActivated = true;
    }

    void Update()
    {
        if (isActivated)
            if (Physics.Raycast(transform.position, transform.forward, sensorDistance))
            {
                isActivated = false;
                MainController.inst.Stop();
            }
    }
}
