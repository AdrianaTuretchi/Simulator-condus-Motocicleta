using UnityEngine;

public class SegmentController2 : MonoBehaviour
{
    [Header("Obstacle Setup")]
    public GameObject obstaclePrefab; // Conectați Prefab-ul de Obstacol aici
    [Range(0, 1)]
    public float obstacleProbability = 0.6f; // Probabilitatea de a genera un obstacol la un loc
    public int numberOfSpots = 5;            // Câte puncte de spawn de-a lungul segmentului
    private float[] lanesX = { -3.5f, 0f, 3.5f }; // Pozițiile pe X ale celor 3 benzi (se potrivesc cu laneWidth din MotorcycleController)

    // Metodă apelată de RoadGenerator imediat după instanțiere
    public void SpawnObstacles()
    {
        // Determinăm lungimea segmentului
        float segmentLength = transform.localScale.z * 10f; // Presupunând că scala inițială e 1
        
        // Iterăm prin punctele de spawn de-a lungul segmentului (pe axa Z)
        for (int i = 1; i <= numberOfSpots; i++)
        {
            if (Random.value < obstacleProbability)
            {
                // Calculăm poziția Z relativă în interiorul segmentului
                float spawnZ = (i * (segmentLength / (numberOfSpots + 1))) - (segmentLength / 2);
                
                // Alegem o bandă aleatorie (Stânga, Centru, Dreapta)
                float spawnX = lanesX[Random.Range(0, lanesX.Length)]; 

                Vector3 spawnPos = new Vector3(spawnX, 0.5f, transform.position.z + spawnZ);
                
                // Instanțiază obstacolul ca fiu al segmentului (pentru organizare)
                Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}
