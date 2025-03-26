using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public enum HapticType
{
    None,
    ActivationOnly,
    Low,
    Medium,
    High,
    Continuous
}

[RequireComponent(typeof(XRGrabInteractable))]
public class HapticTypeComponent : MonoBehaviour
{
    public HapticType hapticType;
    public float amplitude = 1.0f;
    public float duration = 0.1f;

    private XRGrabInteractable interactable;
    private IXRSelectInteractor currentInteractor;
    private HapticImpulsePlayer currentHapticImpulse;

    private CustomHapticScript hapticScript;

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.activated.AddListener(OnActivated);
            interactable.selectEntered.AddListener(OnGrab);
            interactable.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogWarning("XRGrabInteractable component is missing.");
        }

        hapticScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomHapticScript>();
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (hapticType == HapticType.ActivationOnly)
        {
            hapticScript.leftHandHaptic.SendHapticImpulse(amplitude, duration);
            hapticScript.rightHandHaptic.SendHapticImpulse(amplitude, duration);
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.activated.RemoveListener(OnActivated);
            interactable.selectEntered.RemoveListener(OnGrab);
            interactable.selectExited.RemoveListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null;
    }
}