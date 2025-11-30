using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CraftingButton : MonoBehaviour
{
    public CraftingRecipe recipeToCraft;
    private Button button;

    // ไม่ต้องเก็บตัวแปร Player ไว้ตลอดก็ได้ เรียก instance เอาชัวร์กว่า

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCraftClick);
    }

    private void OnCraftClick()
    {
        // เช็คว่ามี Recipe และมี Player อยู่ในเกมจริงไหม
        if (recipeToCraft == null) return;

        // ใช้ Player.instance จะหาเจอเสมอแม้ตัวละครจะซ่อนอยู่
        if (Player.instance != null)
        {
            Player.instance.TryCraft(recipeToCraft);
        }
        else
        {
            Debug.LogWarning("CraftingButton: ไม่เจอ Player.instance ในฉาก!");
        }
    }
}