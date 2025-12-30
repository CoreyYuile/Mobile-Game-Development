using UnityEngine;

[CreateAssetMenu]
public class CropData : ScriptableObject
{
    public enum CropIDs
    {
        Tomato,
        Potato,
        Wheat,
        Corn,
        Pumpkin,
        Sunflower
    }
    public CropIDs cropID = CropIDs.Tomato;
    public string cropName;
    
    [Header("Timing")]
    public float growthDuration = 5.0f;

    [Header("Rewards")]
    public int harvestReward = 20;

    [Header("Prefabs")]
    public GameObject seedlingPrefab;
    public GameObject grownPrefab;
}
