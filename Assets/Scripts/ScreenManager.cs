using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private float actionDelay;

    public void LoadScene(int scene)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(DelayedLoadScene(scene));
    }

    public void ExitGame()
    {
        StartCoroutine(DelayedExitGame());
    }

    private IEnumerator DelayedLoadScene(int scene)
    {
        yield return new WaitForSeconds(actionDelay);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator DelayedExitGame()
    {
        yield return new WaitForSeconds(actionDelay); 
        Application.Quit();
    }
}
