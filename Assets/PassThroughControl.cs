using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughControl : MonoBehaviour
{

    private GameObject[] Walls;
    public float timeForPassthrough;
    public float cooldown;

    private bool canPassthrough;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach(GameObject obj in Walls)
        {
            obj.SetActive(false);
        }

        yield return new WaitForSeconds(timeForPassthrough);

        foreach (GameObject obj in Walls)
        {
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(cooldown);

        canPassthrough = true;
    }
}
