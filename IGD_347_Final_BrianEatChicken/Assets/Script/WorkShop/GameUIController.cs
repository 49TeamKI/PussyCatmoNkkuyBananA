using UnityEngine;
using UnityEngine.SceneManagement; // สำคัญมาก ห้ามลืม

public class GameUIController : MonoBehaviour
{
    
    public void BackToMenu()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติ เผื่อมีการหยุดเวลาไว้
        SceneManager.LoadScene(0);
    }
}