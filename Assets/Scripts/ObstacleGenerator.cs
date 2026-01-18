using UnityEngine;

public class TileObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Trage aici Prefab-ul Cubului/Obstacolului
    public float[] lanes = { -3f, 0f, 3f };
    public float obstacleY = 0.5f;

    [HideInInspector]
    public bool shouldSpawn = false; // Controlat de RoadGenerator

    void Start()
    {
        if (shouldSpawn)
        {
            SpawnRandomObstacle();
        }
    }

    void SpawnRandomObstacle()
    {
        // Alegem o banda la intamplare
        int randomLane = Random.Range(0, lanes.Length);
        
        // Pozitia este relativa la pozitia segmentului de drum (transform.position)
        // Am pus Z = 0 pentru ca obstacolul va fi "copil" si se va alinia cu centrul segmentului
        Vector3 localSpawnPos = new Vector3(lanes[randomLane], obstacleY, 0f);

        // Cream obstacolul
        GameObject obs = Instantiate(obstaclePrefab);
        
        // Il facem copilul segmentului curent
        obs.transform.SetParent(this.transform);
        
        // Ii setam pozitia locala fata de parinte
        obs.transform.localPosition = localSpawnPos;
    }
}