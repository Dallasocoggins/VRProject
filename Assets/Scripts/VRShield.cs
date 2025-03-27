using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRShield : MonoBehaviour
{
    public float repelForce = 100f;
    public LayerMask repelledLayers;
    public float repelRadius = 1.5f; // Radius for overlap check
    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor currentInteractor;
    private bool isRepelling = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(TriggerPressed); // Listen for trigger press
        grabInteractable.deactivated.AddListener(TriggerReleased); // Listen for trigger release
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        if (isRepelling)
        {
            // Check for overlapping colliders within the repel radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, repelRadius, repelledLayers);
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 repelDirection = (col.transform.position - transform.position).normalized;
                    rb.AddForce(repelDirection * repelForce, ForceMode.Impulse);
                }
            }
        }
    }

    void TriggerPressed(ActivateEventArgs args)
    {
        isRepelling = true;
    }

    void TriggerReleased(DeactivateEventArgs args)
    {
        isRepelling = false;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null;
        isRepelling = false;
    }
}
