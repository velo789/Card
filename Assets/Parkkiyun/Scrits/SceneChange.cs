using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }
    public void GameScene()
    {
        SceneManager.LoadScene(1);
    }
}
