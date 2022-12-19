using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MassController : MonoBehaviour
{
    public static MassController inst;

    public float leftBodyMass, rightBodyMass;

    [SerializeField] Transform leftBody, rightBody;

    [SerializeField] TextMesh leftMassText, rightMassText;

    [SerializeField] AnimationCurve scaleMassDependency;

    Coroutine leftBodyScaling, rightBodyScaling;

    void Awake()
    {
        inst = this;
    }

    public void SetLeftBodyMass(Slider slider)
    {
        {
            leftBodyMass = slider.value;
            leftMassText.text = leftBodyMass.ToString("0.00");

            if (leftBodyScaling != null)
                StopCoroutine(leftBodyScaling);

            leftBodyScaling = StartCoroutine(ScaleBody(leftBody, leftBodyMass));
        }

    }

    public void SetLeftBodyMass(InputField inputField)
    {
        if (!float.TryParse(inputField.text, out leftBodyMass))
            return;

        inputField.text = "";

        leftBodyMass = Mathf.Clamp(leftBodyMass, 0.5f, 99f);

        leftMassText.text = leftBodyMass.ToString("0.00");

        if (leftBodyScaling != null)
            StopCoroutine(leftBodyScaling);

        leftBodyScaling = StartCoroutine(ScaleBody(leftBody, leftBodyMass));
    }

    public void SetRightBodyMass(Slider slider)
    {
        rightBodyMass = slider.value;
        rightMassText.text = rightBodyMass.ToString("0.00");

        if (rightBodyScaling != null)
            StopCoroutine(rightBodyScaling);

        rightBodyScaling = StartCoroutine(ScaleBody(rightBody, rightBodyMass));
    }

    public void SetRightBodyMass(InputField inputField)
    {
        if (!float.TryParse(inputField.text, out rightBodyMass))
            return;

        inputField.text = "";

        rightBodyMass = Mathf.Clamp(rightBodyMass, 0.5f, 99f);

        rightMassText.text = rightBodyMass.ToString("0.0");

        if (rightBodyScaling != null)
            StopCoroutine(rightBodyScaling);

        rightBodyScaling = StartCoroutine(ScaleBody(rightBody, rightBodyMass));
    }


    public void Reset()
    {
        leftBodyMass = rightBodyMass = 1f;
        leftMassText.text = rightMassText.text = "1";


        if (leftBodyScaling != null)
            StopCoroutine(leftBodyScaling);

        leftBodyScaling = StartCoroutine(ScaleBody(leftBody, leftBodyMass));

        if (rightBodyScaling != null)
            StopCoroutine(rightBodyScaling);

        rightBodyScaling = StartCoroutine(ScaleBody(rightBody, rightBodyMass));
    }

    IEnumerator ScaleBody(Transform body, float mass)
    {
        var scaleVar = scaleMassDependency.Evaluate(mass);

        var targetScale = new Vector3(scaleVar, scaleVar, body.localScale.z);

        while (body.localScale != targetScale)
        {
            body.localScale = Vector3.Lerp(body.localScale, targetScale, 12f * Time.deltaTime);
            yield return null;
        }
    }

}
