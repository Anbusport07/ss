using UnityEngine;
using UnityEngine.SceneManagement;

public class commondHandler : MonoBehaviour
{
    

    public void loadScenes(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void loadScenes(int index)
    {
        SceneManager.LoadScene(index);
    }
}
