using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassThroughControl : MonoBehaviour
{
    private GameObject[] Walls;
    private GameObject[] OuterWalls;
    private GameObject[] Grounds;
    private MazeChasingNPC Enemy;
    public float timeForPassthrough = 5f;
    public float cooldown = 3f;
    public Image canUse;

    private bool canPassthrough = true;

    public OVRPassthroughLayer ovrlayer;
    public float flashDuration = 2f;
    public float flashInterval = 0.2f;

    // Flash effect values
    public float flashBrightness = 0.5f;
    public float flashContrast = 0.5f;
    public float flashSaturation = 0.5f;

    // Default values to restore
    private float defaultBrightness;
    private float defaultContrast;
    private float defaultSaturation;

    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");
        OuterWalls = GameObject.FindGameObjectsWithTag("OuterWall");
        Grounds = GameObject.FindGameObjectsWithTag("Ground");
        Enemy = FindFirstObjectByType<MazeChasingNPC>();

        // Save default passthrough values
        defaultBrightness = ovrlayer.colorMapEditorBrightness;
        defaultContrast = ovrlayer.colorMapEditorContrast;
        defaultSaturation = ovrlayer.colorMapEditorSaturation;
    }

    public void DisableWalls()
    {
        if (canPassthrough)
        {
            StartCoroutine(DisableWallsTimer());
        }
    }

    private IEnumerator DisableWallsTimer()
    {
        canPassthrough = false;
        canUse.color = new Color(255, 0, 0);

        // Disable walls
        foreach (GameObject obj in Walls)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in OuterWalls)
        {
            obj.SetActive(false);
        }

        // Set grounds to translucent (alpha 0.3)
        SetGroundTransparency(0.3f);

        Enemy.SetFrozen(true);

        // Wait before starting flash
        float waitTimeBeforeFlashing = timeForPassthrough - flashDuration;
        if (waitTimeBeforeFlashing > 0)
        {
            yield return new WaitForSeconds(waitTimeBeforeFlashing);
            StartCoroutine(FlashPassthrough());
        }

        yield return new WaitForSeconds(flashDuration);

        // Re-enable walls
        foreach (GameObject obj in Walls)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in OuterWalls)
        {
            obj.SetActive(true);
        }

        // Restore ground opacity (alpha 1.0)
        SetGroundTransparency(1f);

        Enemy.SetFrozen(false);

        ResetPassthroughColor();
        canPassthrough = false;

        yield return new WaitForSeconds(cooldown);
        canPassthrough = true;
        canUse.color = new Color(0, 255, 0);
    }

    private IEnumerator FlashPassthrough()
    {
        float startTime = Time.time;
        float endTime = startTime + flashDuration;
        bool flashOn = false;

        while (Time.time < endTime)
        {
            float remaining = endTime - Time.time;
            float dynamicInterval = Mathf.Lerp(0.05f, flashInterval, remaining / flashDuration);

            if (flashOn)
            {
                ovrlayer.SetBrightnessContrastSaturation(flashBrightness, flashContrast, flashSaturation);
            }
            else
            {
                ResetPassthroughColor();
            }

            flashOn = !flashOn;
            yield return new WaitForSeconds(dynamicInterval);
        }

        ResetPassthroughColor();
    }

    private void ResetPassthroughColor()
    {
        ovrlayer.SetBrightnessContrastSaturation(defaultBrightness, defaultContrast, defaultSaturation);
    }

    private void SetGroundTransparency(float alpha)
    {
        foreach (GameObject obj in Grounds)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) continue;

            foreach (Material mat in renderer.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;

                mat.SetFloat("_Mode", 2); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}
