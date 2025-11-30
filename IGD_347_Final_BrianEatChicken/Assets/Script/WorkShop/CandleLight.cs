using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private Light myLight;

    [Header("ตั้งค่าแสงเทียน")]
    public float minIntensity = 0.8f; 
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 10f;  

    void Start()
    {
        myLight = GetComponent<Light>();
        // ถ้าลืมใส่ Light ให้มันหาเองในลูก
        if (myLight == null) myLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (myLight == null) return;

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0);

        myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}