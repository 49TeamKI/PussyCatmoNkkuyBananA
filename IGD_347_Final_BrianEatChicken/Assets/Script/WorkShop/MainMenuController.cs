using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
  
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    public void BackToMainMenu()
    {
        
        SceneManager.LoadScene(0); 
        
        
    }
    

    
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}