using UnityEngine;
using System.Collections; 

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private GameObject activationEffect;

    [Header("Settings")]
    [SerializeField] 
    private float effectDuration = 2.0f; 

    
    private Coroutine currentCoroutine;

    private void Start()
    {
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"เสา {gameObject.name} ต้องติ๊ก 'Is Trigger' ใน Collider ด้วยนะครับ!");
        }

        
        if (activationEffect != null)
        {
            activationEffect.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                
                GameManager.instance.SaveCheckpoint(transform.position);

                
                if (activationEffect != null)
                {
                    
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }

                    
                    currentCoroutine = StartCoroutine(ShowEffectTemp());
                }

                Debug.Log($"[Checkpoint] บันทึกจุดเกิดที่: {gameObject.name} | เวลา: {Time.time}");
            }
            else
            {
                Debug.LogError("ไม่เจอ GameManager!");
            }
        }
    }

    
    private IEnumerator ShowEffectTemp()
    {
        
        activationEffect.SetActive(true);

        
        yield return new WaitForSeconds(effectDuration);

        
        activationEffect.SetActive(false);
        
        
        currentCoroutine = null;
    }
}