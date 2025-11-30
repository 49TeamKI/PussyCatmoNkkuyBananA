using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private float duration;

    // ฟังก์ชันสำหรับตั้งค่าเมื่อไอคอนถูกสร้างขึ้นมา
    public void Setup(Sprite sprite, float _duration)
    {
        iconImage.sprite = sprite;
        duration = _duration;

        // สั่งทำลายตัวเองเมื่อครบเวลา
        Destroy(gameObject, duration);
    }
}