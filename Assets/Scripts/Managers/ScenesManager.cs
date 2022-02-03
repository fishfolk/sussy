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

    public void GoToOnlineScene()
    {
        SceneManager.LoadScene(ScenesDataStore.GetOnlineSceneName());
    }

    public void GoToLocalScene()
    {
        SceneManager.LoadScene(ScenesDataStore.GetLocalSceneName());
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

    private static string LocalSceneName = "LocalScene";
    public static string GetLocalSceneName() { return LocalSceneName; }

    private static string MainMenuSceneName = "MainMenuScene";
    public static string GetMainMenuSceneName() { return MainMenuSceneName; }

    private static string FreeRoamSceneName = "FreeRoamScene";
    public static string GetFreeRoamSceneName() { return FreeRoamSceneName; }

}