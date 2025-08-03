using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public bool isPaused;
    [SerializeField] Animator uiAnim;
    [SerializeField] private float actionDelay;

    [SerializeField] GameObject pausePanel, escBtnPaused, escBtn;

    private bool canPause = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            escBtnPaused.SetActive(true);
            escBtn.SetActive(false);
            StartCoroutine(DelayedTogglePause());
        }

        if (isPaused && Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(DelayedExitGame());
        }
    }

    private IEnumerator DelayedTogglePause()
    {
        canPause = false;

        if (isPaused)
        {
            if (uiAnim) uiAnim.Play("WaterBillboardTransitionReverse");

            yield return new WaitForSecondsRealtime(0f);

            escBtnPaused.SetActive(false);
            escBtn.SetActive(true);

            ResumeGame();
        }
        else
        {
            if (uiAnim) uiAnim.Play("WaterBillboardTransitionVerse");

            yield return new WaitForSecondsRealtime(actionDelay);

            PauseGame();
        }

        canPause = true;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    private IEnumerator DelayedExitGame()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(actionDelay);
        Application.Quit();
    }
}
