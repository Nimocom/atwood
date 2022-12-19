using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public static RopeController inst;

    [SerializeField] LineRenderer leftLine, rightLine;
    [SerializeField] Transform leftBody, rightBody;

    [SerializeField] Transform leftUpperPoint, rightUpperPoint;

    void Awake()
    {
        inst = this;
    }

    public void UpdateRopes()
    {
        leftLine.SetPosition(0, leftUpperPoint.position);
        leftLine.SetPosition(1, leftBody.position);

        rightLine.SetPosition(0, rightUpperPoint.position);
        rightLine.SetPosition(1, rightBody.position);
    }
}
