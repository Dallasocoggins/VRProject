using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField]
    int sceneToLoadIndex;

    [SerializeField]
    Transform tpPoint;

    SceneManage sceneManage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManage = FindFirstObjectByType<SceneManage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sceneManage.GoToScene(sceneToLoadIndex, this);
        }
    }
}
