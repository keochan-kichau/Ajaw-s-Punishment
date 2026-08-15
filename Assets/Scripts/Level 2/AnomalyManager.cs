using UnityEngine;
using System.Collections.Generic;

public class AnomalyManager : MonoBehaviour
{
    [Header("Danh sách dị vật trên Map")]
    [Tooltip("Kéo thả các object dị vật vào mảng này")]
    public GameObject[] anomalies;

    [Header("Tỉ lệ xuất hiện dị vật (0.0 đến 1.0)")]
    public float anomalyChance = 0.5f;

    void Start()
    {
        foreach (GameObject obj in anomalies)
        {
            if (obj != null) obj.SetActive(false);
        }

        CemeteryHouseInteract house = FindAnyObjectByType<CemeteryHouseInteract>();
        if (house == null) return;

        if (CemeteryHouseInteract.currentLevel == 0)
        {
            house.hasAnomaly = false;
            Debug.Log("Đang ở Level 0. Map an toàn tuyệt đối.");
            return;
        }

        float roll = Random.Range(0f, 1f);

        if (roll <= anomalyChance && anomalies.Length > 0)
        {
            house.hasAnomaly = true; 

            int maxSpawns = Mathf.Min(4, anomalies.Length);
            int numToSpawn = Random.Range(1, maxSpawns + 1); 

            Debug.Log("Cảnh báo: Sẽ có " + numToSpawn + " dị vật xuất hiện lần này!");

            List<int> availableIndices = new List<int>();
            for (int i = 0; i < anomalies.Length; i++)
            {
                availableIndices.Add(i);
            }

            for (int i = 0; i < numToSpawn; i++)
            {
                int randomPick = Random.Range(0, availableIndices.Count);
                int selectedIndex = availableIndices[randomPick];

                if (anomalies[selectedIndex] != null)
                {
                    anomalies[selectedIndex].SetActive(true);
                    Debug.Log("-> Dị vật: " + anomalies[selectedIndex].name);
                }

                availableIndices.RemoveAt(randomPick);
            }
        }
        else
        {
            house.hasAnomaly = false;
            Debug.Log("Map bình thường, không có gì lạ.");
        }
    }
}