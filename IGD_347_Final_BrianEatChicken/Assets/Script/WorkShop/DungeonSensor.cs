using UnityEngine;

public class DungeonSensor : MonoBehaviour
{
    public Light targetLight; // ลากตัว Light มาใส่ช่องนี้

    [Header("การตั้งค่า")]
    public float maxIntensity = 2.0f; // ความสว่างเต็มที่
    public float fadeSpeed = 2.0f;    // ความเร็วในการสว่าง (ยิ่งเลขเยอะยิ่งสว่างเร็ว)
    public bool turnOffWhenExit = false; // ถ้าเดินออกแล้วจะให้ดับไหม? (True = ดับ, False = ติดตลอด)

    private float targetValue = 0f; // ค่าแสงที่เราต้องการให้เป็นตอนนี้

    void Start()
    {
        if (targetLight != null)
        {
            targetLight.intensity = 0f; // เริ่มเกมมาสั่งปิดไฟก่อนเลย
        }
    }

    void Update()
    {
        if (targetLight == null) return;

        // คำสั่งให้ค่อยๆ เปลี่ยนค่าความสว่าง (Lerp) ไปหาค่าเป้าหมาย
        targetLight.intensity = Mathf.MoveTowards(targetLight.intensity, targetValue, Time.deltaTime * fadeSpeed);
    }

    // เมื่อมีอะไรเดินเข้ามาในเขต
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // เช็คว่าเป็นผู้เล่นไหม
        {
            targetValue = maxIntensity; // ตั้งเป้าหมายให้ไฟสว่าง
        }
    }

    // เมื่อเดินออกจากเขต
    private void OnTriggerExit(Collider other)
    {
        if (turnOffWhenExit && other.CompareTag("Player"))
        {
            targetValue = 0f; // ตั้งเป้าหมายให้ไฟดับ
        }
    }
}