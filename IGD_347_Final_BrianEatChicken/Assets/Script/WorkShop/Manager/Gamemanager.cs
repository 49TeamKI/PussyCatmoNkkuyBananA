using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public int currentScore = 0;
    public bool isGamePaused = false;

    // ตัวแปรสำหรับ Checkpoint
    private Vector3 currentCheckpointPosition;
    private bool isCheckpointSet = false;

    [Header("UI Game")]
    public GameObject pauseMenuUI;
    
    // +++ [เพิ่ม] ช่องใส่หน้าต่างชนะเกม +++
    public GameObject winPanel; 
    // +++++++++++++++++++++++++++++++++

    public TMP_Text scoreText;
    public Slider HPBar;

    [Header("System References")]
    public CameraControl mainCameraFollowScript;

    [Header("Enemies Management")]
    public List<Enemy> allEnemies = new List<Enemy>();

    private void Awake()
    {
        // +++ [เพิ่ม] สั่งรีเซ็ตเวลาให้เดินปกติทุกครั้งที่เริ่มฉากใหม่ +++
        Time.timeScale = 1f;
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++

        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // ปิดไว้ถูกต้องแล้วครับ
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // +++ [เพิ่ม] สั่งปิดหน้าต่าง Win Panel ทันทีที่เริ่มเกม +++
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        // +++++++++++++++++++++++++++++++++++++++++++++++++++

        // ค้นหา Enemy ทุกตัวในฉากเก็บใส่ List
        Enemy[] enemiesFound = FindObjectsOfType<Enemy>();
        allEnemies.AddRange(enemiesFound);
    }

    // ------------------- ฟังก์ชันของเกม -------------------

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        HPBar.value = currentHealth;
        HPBar.maxValue = maxHealth;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    // --- ฟังก์ชัน Checkpoint ---
    public void SaveCheckpoint(Vector3 newPosition)
    {
        currentCheckpointPosition = newPosition + Vector3.up * 1.0f;
        isCheckpointSet = true;
        Debug.Log("GameManager: Checkpoint saved at " + currentCheckpointPosition);
    }

    // --- 2. ฟังก์ชัน Respawn (ฉบับแก้เลือดลดเอง) ---
    public void RespawnPlayer(Player player)
    {
        Debug.Log("Respawning player...");

        // 1. สั่งให้ GameObject 'ตื่น'
        player.gameObject.SetActive(true);

        // 2. รีเซ็ต Animator
        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Respawn");
        }

        // 3. รีเซ็ตฟิสิกส์
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // 4. ย้ายตำแหน่ง
        Vector3 spawnPosition;
        if (isCheckpointSet)
        {
            spawnPosition = currentCheckpointPosition;
        }
        else
        {
            spawnPosition = new Vector3(0, 1, 0);
        }

        if (playerRb != null)
        {
            playerRb.position = spawnPosition;
        }
        else
        {
            player.transform.position = spawnPosition;
        }

        // 5. เติมเลือด
        player.Heal(player.maxHealth);

        // +++ [สำคัญมาก] สั่งล้างสถานะพิษ/เลือดไหล ตรงนี้ครับ +++
        player.ClearAllStatus();
        // +++++++++++++++++++++++++++++++++++++++++++++++

        UpdateHealthBar(player.health, player.maxHealth);

        if (mainCameraFollowScript != null)
        {
            mainCameraFollowScript.SnapToTarget();
        }
        else
        {
            Debug.LogError("ตาย");
        }

        // สั่งรีเซ็ตศัตรู
        ResetAllEnemies();
    }
    // --- สิ้นสุด Checkpoint ---

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
        pauseMenuUI.SetActive(isGamePaused);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ResetAllEnemies()
    {
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.ResetEnemy();
            }
        }
    }
}