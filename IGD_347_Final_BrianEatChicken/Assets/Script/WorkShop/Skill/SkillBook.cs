using System.Collections.Generic;

using UnityEngine;



public class SkillBook : MonoBehaviour

{

    public List<Skill> skillsSet = new List<Skill>();

    public GameObject[] skillEffects;

    List<Skill> DulationSkills = new List<Skill>();



    Player player;



    public void Start()

    {

        player = GetComponent<Player>();



        // (สมมติว่า Fireball และ BuffSpeed "ล็อก" อยู่ตอนเริ่ม)

        skillsSet.Add(new FireballSkill());    // (Index 0) - ล็อก

        skillsSet.Add(new HealSkill());        // (Index 1) - ปลดล็อกแล้ว (สมมติ)

        skillsSet.Add(new BuffSkillMoveSpeed()); // (Index 2) - ล็อก

    }



    void Update()

    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) { UseSkill(0); } // Fireball

        else if (Input.GetKeyDown(KeyCode.Alpha2)) { UseSkill(1); } // Heal

        else if (Input.GetKeyDown(KeyCode.Alpha3)) { UseSkill(2); } // BuffSpeed



        // (Update DulationSkills - เหมือนเดิม)

        for (int i = DulationSkills.Count - 1; i >= 0; i--)

        {

            DulationSkills[i].UpdateSkill(player);

            if (DulationSkills[i].timer <= 0)

            {

                DulationSkills.RemoveAt(i);

            }

        }

    }



    public void UseSkill(int index)

    {

        if (index >= 0 && index < skillsSet.Count)

        {

            Skill skill = skillsSet[index];



            // --- 1. (เพิ่มส่วนตรวจสอบนี้!) ---

            // เช็คว่าสกิลนี้ "ปลดล็อก" แล้วหรือยัง

            if (!skill.isUnlocked)

            {

                Debug.Log($"Skill '{skill.skillName}' is still LOCKED!");

                return; // (จบการทำงาน ถ้ายังล็อก)

            }

            // --- สิ้นสุดส่วนที่เพิ่ม ---



            // (เช็คคูลดาวน์ - เหมือนเดิม)

            if (!skill.IsReady(Time.time))

            {

                Debug.Log($"Skill '{skill.skillName}' is on cooldown.");

                return;

            }



            // (ใช้งานสกิล - เหมือนเดิม)

            GameObject g = Instantiate(skillEffects[index], transform.position, Quaternion.identity, transform);

            Destroy(g, 1);

            skill.Activate(player);

            skill.TimeStampSkill(Time.time);



            if (skill.timer > 0)

            {

                DulationSkills.Add(skill);

            }

        }

    }



    // --- 2. (เพิ่มฟังก์ชันนี้!) ---

    // นี่คือฟังก์ชันที่ "Trigger" จะเรียกใช้

    public void UnlockSkill(string skillName)

    {

        // วนหาใน "สมุดสกิล"

        foreach (Skill skill in skillsSet)

        {

            if (skill.skillName == skillName)

            {

                if (skill.isUnlocked)

                {

                    Debug.Log($"Skill '{skillName}' is already unlocked.");

                }

                else

                {

                    // (เจอแล้ว! สั่งปลดล็อก)

                    skill.isUnlocked = true;

                    Debug.Log($"NEW BUFF/SKILL UNLOCKED: {skill.skillName}!");

                }

                return; // (เจอฟังก์ชันแล้ว ออก)

            }

        }

        Debug.LogWarning($"Could not find skill with name '{skillName}' to unlock.");

    }

    // --- สิ้นสุดส่วนที่เพิ่ม ---



    private void OnDrawGizmos()

    {

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 5);

    }

}