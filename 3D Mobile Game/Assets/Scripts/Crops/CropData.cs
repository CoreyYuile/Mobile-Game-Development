using UnityEngine;

[CreateAssetMenu]
public class CropData : ScriptableObject
{
    public string cropName;

    [Header("Timing")]
    public float growthDuration = 5f;

    [Header("Rewards")]
    public int harvestReward = 20;

    [Header("Prefabs")]
    public GameObject seedlingPrefab;
    public GameObject grownPrefab;
}
