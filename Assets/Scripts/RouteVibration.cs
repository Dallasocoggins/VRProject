using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class RouteVibration : MonoBehaviour
{
    public HapticImpulsePlayer haptic;
    public Transform goal;

    public float fastest = 0.05f;
    public float slowest = 0.5f;
    public float minAmp = 0.3f;
    public float maxAmp = 0.8f;

    private bool isInRoute = false;

    private void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        if (!haptic)
        {
            haptic = GetComponent<HapticImpulsePlayer>();
        }
    }


    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("route"))
        {
            isInRoute = true;
            StartCoroutine(PlayFeedback());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("route"))
        {
            isInRoute = false;
            StopCoroutine(PlayFeedback());
        }
    }

    private IEnumerator PlayFeedback()
    {
        while (isInRoute)
        {
            float distance = Mathf.Abs(Vector3.Distance(this.transform.position, goal.position));
            float freq = Mathf.Lerp(slowest, fastest, 1 - Mathf.Clamp01(distance / 10f));
            float amplitude = Mathf.Lerp(minAmp, maxAmp, 1 - Mathf.Clamp01(distance / 10f));

            haptic.SendHapticImpulse(amplitude, freq);
      
            yield return new WaitForSeconds(freq * 2);
        }
    }
}
