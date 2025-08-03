using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    [SerializeField] Player player;
    
    [SerializeField] private float actionDelay;
    [SerializeField] private int scene;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoadScene(scene);
            player.uiAnim.Play("WaterBillboardTransitionVerse");
        }
        
    }
    public void LoadScene(int scene)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(DelayedLoadScene(scene));
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
