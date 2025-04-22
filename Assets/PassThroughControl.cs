using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughControl : MonoBehaviour
{
    private GameObject[] Walls;
    private GameObject[] OuterWalls;
    private MazeChasingNPC Enemy;
    public float timeForPassthrough = 5f;
    public float cooldown = 3f;

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
        Enemy = FindFirstObjectByType<MazeChasingNPC>();

        // Save default values
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

        foreach (GameObject obj in Walls)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in OuterWalls)
        {
            obj.SetActive(false);
        }
        Enemy.SetFrozen(true);

        // Wait before starting flash
        float waitTimeBeforeFlashing = timeForPassthrough - flashDuration;
        if (waitTimeBeforeFlashing > 0)
        {
            yield return new WaitForSeconds(waitTimeBeforeFlashing);
            StartCoroutine(FlashPassthrough());
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (GameObject obj in Walls)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in OuterWalls)
        {
            obj.SetActive(true);
        }
        Enemy.SetFrozen(false);

        ResetPassthroughColor();
        canPassthrough = false;

        yield return new WaitForSeconds(cooldown);
        canPassthrough = true;
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
}
