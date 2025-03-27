using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRInteractableVibration : MonoBehaviour
{
    private IXRSelectInteractor interactor;
    private bool isInsideRoute = false;
    private HapticImpulsePlayer hapticController;

    void Start()
    {
        // Attempt to find an XRGrabInteractable component and subscribe to its events
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void FixedUpdate()
    {
        if (isInsideRoute && hapticController)
        {
            hapticController.SendHapticImpulse(0.5f, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("route"))
        {
            isInsideRoute = true;
            StartVibration();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("route"))
        {
            isInsideRoute = false;
            StopVibration();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        StopVibration();
        interactor = null;
    }

    private void StartVibration()
    {
        if (interactor != null)
        {
            hapticController = interactor.transform.GetComponent<HapticImpulsePlayer>();
            if (hapticController != null)
            {
                hapticController.SendHapticImpulse(0.5f, 0.1f); 
            }
        }
    }

    private void StopVibration()
    {
        if (interactor != null)
        {
            if (hapticController != null)
            {
                hapticController.SendHapticImpulse(0f, 0f); // Stop vibration
            }
        }
    }
}
