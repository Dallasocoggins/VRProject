using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneManage: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene(int sceneToLoadIndex, DoorTeleport door)
    {
        SceneManager.LoadScene(sceneToLoadIndex);
    }
}
