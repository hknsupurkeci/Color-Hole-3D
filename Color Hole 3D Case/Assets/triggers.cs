using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggers : MonoBehaviour
{
    public GameObject startMenu, gameOverMenu, finishMenu;

    public void StartLevel()
    {
        OnChangePosition.gameOver = true;
        startMenu.SetActive(false);
    }

    public void RetryLevel()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //sahne yüklendikten sonra false yapıyorum.
         GameController.flag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            OnChangePosition.minZ = -37.78f;
            OnChangePosition.maxZ = -13.95f;
            gameOverMenu.SetActive(true);
            OnChangePosition.gameOver = false;
            Destroy(other.gameObject);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
