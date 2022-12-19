using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float xSpeed = 120.0f;
    [SerializeField] float ySpeed = 120.0f;

    [SerializeField] float yMinLimit = -20f;
    [SerializeField] float yMaxLimit = 80f;

    [SerializeField] float distanceMin = .5f;
    [SerializeField] float distanceMax = 15f;

    Rigidbody rbody;

    float x = 0.0f;
    float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;

        rbody = GetComponent<Rigidbody>();

        if (rbody != null)
            rbody.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            target.position += Vector3.up * Time.deltaTime * 1f;
            ClampHeight();
        }

        if (Input.GetKey(KeyCode.E))
        {
            target.position += Vector3.down * Time.deltaTime * 1f;
            ClampHeight();
        }

        
    }

    void ClampHeight()
    {
        var pos = target.position;
        pos.y = Mathf.Clamp(pos.y, 0.12f, 1.2f);
        target.position = pos;
    }

    void LateUpdate()
    {
        if (target)
        {
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 3f, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 12f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, position, 12f * Time.deltaTime);
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}