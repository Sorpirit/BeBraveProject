using UnityEngine;

public class MainSceneLoader : MonoBehaviour
{
    public void LoadGamePlayScene()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
