using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRGun : MonoBehaviour
{
    public Transform muzzle; // Where bullets spawn
    public GameObject projectilePrefab; // Bullet prefab
    public float projectileSpeed = 20f;
    public LineRenderer laserLine; // Laser sight

    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor currentInteractor; // Stores the current interactor

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(FireGun); // Listen for trigger press
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Setup Laser Pointer
        if (laserLine)
        {
            laserLine.positionCount = 2;
            laserLine.enabled = true; // Always on
        }
    }

    void Update()
    {
        // Always show laser pointer
        if (laserLine)
        {
            Vector3 endPoint = muzzle.position + muzzle.forward * 50f;
            RaycastHit hit;

            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, 50f))
            {
                endPoint = hit.point;
            }

            laserLine.SetPosition(0, muzzle.position);
            laserLine.SetPosition(1, endPoint);
        }
    }

    void FireGun(ActivateEventArgs args)
    {
        if (projectilePrefab)
        {
            GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.linearVelocity = muzzle.forward * projectileSpeed;
            }
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject; // Store the interactor when grabbing the gun
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null; // Clear the interactor when released
    }
}
