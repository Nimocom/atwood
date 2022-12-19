using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Diagnostics;

public class MainController : MonoBehaviour
{
    public static MainController inst;

    [SerializeField] Transform leftBody, rightBody;

    [SerializeField] Transform pulley;

    [SerializeField] TextMesh timerText;
    [SerializeField] TextMesh forceText;

    [SerializeField] float pulleyTorqueMlt;

    [SerializeField] float forceMlt;
    [SerializeField] float frictionMlt;

    [SerializeField] float yMin, yMax;

    [SerializeField] Button startButton;
    [SerializeField] Button resetButton;

    [SerializeField] Slider leftSlider;
    [SerializeField] Slider rightSlider;

    [SerializeField] InputField leftInput;
    [SerializeField] InputField rightInput;

    float dif;
    float force;
    float deltaS;
    float deltaV;

    Stopwatch stopwatch;

    public bool isReady;
    public bool isLaunched;

    Vector3 leftBodyStartPosition;
    Vector3 rightBodyStartPosition;

    void Awake()
    {
        inst = this;

        stopwatch = new Stopwatch();
    }

    void Start()
    {
        leftBodyStartPosition = leftBody.position;
        rightBodyStartPosition = rightBody.position;

        isReady = true;
    }

    public void Launch()
    {
        if (MassController.inst.rightBodyMass <= MassController.inst.leftBodyMass)
            return;

        dif = (MassController.inst.leftBodyMass - MassController.inst.rightBodyMass) / (MassController.inst.leftBodyMass + MassController.inst.rightBodyMass);
        force = (dif * Physics.gravity.y) * forceMlt;

        forceText.text = "a=" + force.ToString("0.000");

        isLaunched = true;

        Photosensor.inst.Activate();

        stopwatch.Start();

        leftSlider.interactable = rightSlider.interactable = false;
        leftInput.interactable = rightInput.interactable = false;

        startButton.interactable = false;
        resetButton.interactable = false; 
    }

    public void Stop()
    {
        stopwatch.Stop();
        stopwatch.Reset();
        isLaunched = false;
        isReady = false;

        dif = force = deltaS = deltaV = 0f;

        resetButton.interactable = true;
    }

    public void Reset()
    {
        StartCoroutine(ResetBodies());
        MassController.inst.Reset();
    }

    IEnumerator ResetBodies()
    {
        while (leftBody.position != leftBodyStartPosition && rightBody.position != rightBodyStartPosition)
        {
            leftBody.transform.position = Vector3.Lerp(leftBody.transform.position, leftBodyStartPosition, 12f * Time.deltaTime);
            rightBody.transform.position = Vector3.Lerp(rightBody.transform.position, rightBodyStartPosition, 12f * Time.deltaTime);

            leftSlider.value = Mathf.Lerp(leftSlider.value, 1f, 20f * Time.deltaTime);
            rightSlider.value = Mathf.Lerp(rightSlider.value, 1f, 20f * Time.deltaTime);

            yield return null;
        }

        isReady = true;

        startButton.interactable = true;
        resetButton.interactable = false;

        leftSlider.interactable = rightSlider.interactable = true;
        leftInput.interactable = rightInput.interactable = true;

        timerText.text = "0.000";
        forceText.text = "a=0.000";
    }

    void Update()
    {
        if (!isLaunched)
            return;

        deltaS += dif * force * frictionMlt * Time.deltaTime;
        deltaV += deltaS * Time.deltaTime;

        Vector3 bodyPosition;

        bodyPosition = leftBody.position;
        bodyPosition.y = Mathf.Clamp(bodyPosition.y - deltaV, yMin, yMax);
        leftBody.position = bodyPosition;

        bodyPosition = rightBody.position;
        bodyPosition.y = Mathf.Clamp(bodyPosition.y + deltaV, yMin, yMax);
        rightBody.position = bodyPosition;

        RopeController.inst.UpdateRopes();

        pulley.Rotate(Vector3.forward * deltaV * pulleyTorqueMlt * Time.deltaTime, Space.World);

        timerText.text = stopwatch.Elapsed.TotalSeconds.ToString("0.00");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
