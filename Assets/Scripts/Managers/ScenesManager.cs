using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void GoToScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void GoToSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoToOnlineScene()
    {
        SceneManager.LoadScene(ScenesDataStore.GetOnlineSceneName());
    }

    public void GoToLocalScene(int playerCount)
    {
        switch (playerCount)
        {
            case 2:
                SceneManager.LoadScene(ScenesDataStore.GetLocal2SceneName());
                break;
            case 3:
                SceneManager.LoadScene(ScenesDataStore.GetLocal3SceneName());
                break;
            case 4:
                SceneManager.LoadScene(ScenesDataStore.GetLocal4SceneName());
                break;
        }
    }
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene(ScenesDataStore.GetMainMenuSceneName());
    }

    public void GoToFreeRoamScene()
    {
        SceneManager.LoadScene(ScenesDataStore.GetFreeRoamSceneName());
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}

public class ScenesDataStore
{
    private static string OnlineSceneName = "OnlineScene";
    public static string GetOnlineSceneName() { return OnlineSceneName; }

    private static string Local4SceneName = "Local4Scene";
    public static string GetLocal4SceneName() { return Local4SceneName; }
    private static string Local3SceneName = "Local3Scene";
    public static string GetLocal3SceneName() { return Local3SceneName; }
    private static string Local2SceneName = "Local2Scene";
    public static string GetLocal2SceneName() { return Local2SceneName; }

    private static string MainMenuSceneName = "MainMenuScene";
    public static string GetMainMenuSceneName() { return MainMenuSceneName; }

    private static string FreeRoamSceneName = "FreeRoamScene";
    public static string GetFreeRoamSceneName() { return FreeRoamSceneName; }

}