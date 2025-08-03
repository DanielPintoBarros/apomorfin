using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] Animator uiAnim;
    [SerializeField] GameObject credits;
    [SerializeField] bool creditsShown = false;

    [SerializeField] private float actionDelay;
    [SerializeField] private int scene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
            uiAnim.Play("WaterBillboardTransition");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadScene(scene);
            uiAnim.Play("WaterBillboardTransition");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (creditsShown == false)
            {
                credits.SetActive(true);
                creditsShown = true;
                uiAnim.Play("WaterBillboardTransition");
            }
            else
            {
                credits.SetActive(false);
                creditsShown = false;
                uiAnim.Play("WaterBillboardTransitionHalfReverse");
            }
        }
    }

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
