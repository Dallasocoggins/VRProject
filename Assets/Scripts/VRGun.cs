using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRGun : MonoBehaviour
{
    public Transform muzzle; // Where bullets are fired from
    public float range = 50f; // Max raycast distance
    public float damage = 50f;
    public LineRenderer laserLine; // Laser sight
    public LayerMask hitLayers; // Layers that can be hit
    public Color defaultLaserColor = Color.red;
    public Color fireLaserColor = Color.yellow;
    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor currentInteractor;
    private bool isFiring = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(FireGun);
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        if (laserLine)
        {
            laserLine.positionCount = 2;
            laserLine.enabled = true;
            laserLine.startColor = defaultLaserColor;
            laserLine.endColor = defaultLaserColor;
        }
    }

    void Update()
    {
        if (laserLine)
        {
            Vector3 endPoint = muzzle.position + muzzle.forward * range;
            RaycastHit hit;

            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, hitLayers))
            {
                endPoint = hit.point;
            }

            laserLine.SetPosition(0, muzzle.position);
            laserLine.SetPosition(1, endPoint);
        }
    }

    void FireGun(ActivateEventArgs args)
    {
        if (currentInteractor != null)
        {
            isFiring = true;
            ChangeLaserColor(fireLaserColor);

            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, hitLayers))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }

            Invoke("ResetLaserColor", 0.1f); // Briefly change color
        }
    }

    void ChangeLaserColor(Color color)
    {
        if (laserLine)
        {
            laserLine.startColor = color;
            laserLine.endColor = color;

            if (laserLine.material != null)
            {
                laserLine.material.SetColor("_Color", color); // Ensure material color changes
                laserLine.material.SetColor("_EmissionColor", color); // Helps if using emissive shaders
            }
        }
    }


    void ResetLaserColor()
    {
        isFiring = false;
        ChangeLaserColor(defaultLaserColor);
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

public interface IDamageable
{
    void TakeDamage(float amount);
}
