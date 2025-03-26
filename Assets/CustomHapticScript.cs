using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CustomHapticScript : MonoBehaviour
{
    public HapticImpulsePlayer leftHandHaptic;
    public HapticImpulsePlayer rightHandHaptic;
    public XRBaseInteractor leftHandInteractor;
    public XRBaseInteractor rightHandInteractor;
    public Transform goal;

    [Header("Haptic Intensities")]
    public float minAmp = 0.1f;
    public float maxAmp = 0.8f;
    public float activationAmp = 0.5f;

    private Coroutine leftHandHapticCoroutine;
    private Coroutine rightHandHapticCoroutine;

    private void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        leftHandInteractor.selectEntered.AddListener(StartLeftHandHaptic);
        leftHandInteractor.selectExited.AddListener(StopLeftHandHaptic);
        rightHandInteractor.selectEntered.AddListener(StartRightHandHaptic);
        rightHandInteractor.selectExited.AddListener(StopRightHandHaptic);
    }

    private IEnumerator PlayContinuousHaptic(HapticImpulsePlayer hapticImpulse, XRBaseInteractor interactor)
    {
        while (true)
        {
            if (interactor == null || hapticImpulse == null) yield break;

            float distance = Vector3.Distance(interactor.transform.position, goal.position);
            float amplitude = Mathf.Lerp(minAmp, maxAmp, 1 - Mathf.Clamp01(distance / 10f));
            hapticImpulse.SendHapticImpulse(amplitude, 1.0f);

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void StartLeftHandHaptic(SelectEnterEventArgs args)
    {
        HapticTypeComponent hapticComponent = args.interactableObject.transform.GetComponent<HapticTypeComponent>();
        if (hapticComponent == null || hapticComponent.hapticType == HapticType.None)
            return;

        if (hapticComponent.hapticType == HapticType.Low || hapticComponent.hapticType == HapticType.Medium || hapticComponent.hapticType == HapticType.High)
        {
            float amp = GetHapticIntensity(hapticComponent.hapticType);
            PlayHapticFeedback(leftHandHaptic, amp);
        }
        else if (hapticComponent.hapticType == HapticType.Continuous && leftHandHapticCoroutine == null)
        {
            leftHandHapticCoroutine = StartCoroutine(PlayContinuousHaptic(leftHandHaptic, leftHandInteractor));
        }
    }

    private void StartRightHandHaptic(SelectEnterEventArgs args)
    {
        HapticTypeComponent hapticComponent = args.interactableObject.transform.GetComponent<HapticTypeComponent>();
        if (hapticComponent == null || hapticComponent.hapticType == HapticType.None)
            return;

        if (hapticComponent.hapticType == HapticType.Low || hapticComponent.hapticType == HapticType.Medium || hapticComponent.hapticType == HapticType.High)
        {
            float amp = GetHapticIntensity(hapticComponent.hapticType);
            PlayHapticFeedback(rightHandHaptic, amp);
        }
        else if (hapticComponent.hapticType == HapticType.Continuous && rightHandHapticCoroutine == null)
        {
            rightHandHapticCoroutine = StartCoroutine(PlayContinuousHaptic(rightHandHaptic, rightHandInteractor));
        }
    }

    private void StopLeftHandHaptic(SelectExitEventArgs args)
    {
        if (leftHandHapticCoroutine != null)
        {
            StopCoroutine(leftHandHapticCoroutine);
            leftHandHapticCoroutine = null;
        }
    }

    private void StopRightHandHaptic(SelectExitEventArgs args)
    {
        if (rightHandHapticCoroutine != null)
        {
            StopCoroutine(rightHandHapticCoroutine);
            rightHandHapticCoroutine = null;
        }
    }

    private void PlayHapticFeedback(HapticImpulsePlayer hapticImpulse, float amplitude)
    {
        if (hapticImpulse != null)
        {
            hapticImpulse.SendHapticImpulse(amplitude, 0.1f);
        }
    }

    public void PlayHapticFeedback(float amplitude)
    {
        rightHandHaptic.SendHapticImpulse(amplitude, 0.1f);
        leftHandHaptic.SendHapticImpulse(amplitude, 0.1f);
    }

    private float GetHapticIntensity(HapticType hapticType)
    {
        switch (hapticType)
        {
            case HapticType.Low: return minAmp;
            case HapticType.Medium: return (minAmp + maxAmp) / 2;
            case HapticType.High: return maxAmp;
            default: return 0f;
        }
    }
}
