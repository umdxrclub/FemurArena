using System.Collections;
using UnityEngine;

/// <summary>
/// A manager for FemurArena that spawns in enemies. 
/// </summary>
public class FemurArena : MonoBehaviour
{
    /// <summary>
    /// The prefab of the head crab.
    /// </summary>
    public HeadCrab headCrabPrefab;
    
    /// <summary>
    /// The transform around which to spawn head crabs.
    /// </summary>
    public Transform spawnTransform;
    
    /// <summary>
    /// The transform for the crabs to target (which is the player).
    /// </summary>
    public Transform targetTransform;
    
    /// <summary>
    /// The radius to spawn head crabs around.
    /// </summary>
    public float spawnRadius = 20f;
    
    /// <summary>
    /// The interval (in seconds) to spawn.
    /// </summary>
    public float spawnInterval = 2f;
    
    /// <summary>
    /// The limit of head crabs to spawn.
    /// </summary>
    public int headcrabLimit = 8;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    /// <summary>
    /// Spawns a headcrab according to the specified interval.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (HeadCrab.totalHeadCrabs < headcrabLimit)
            {
                Vector3 spawnPosition = spawnTransform.position + Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(0,0, spawnRadius);
                Quaternion spawnRotation =
                    Quaternion.LookRotation((spawnTransform.position - spawnPosition).normalized, Vector3.up);
                HeadCrab crab = Instantiate(headCrabPrefab, spawnPosition, spawnRotation);
                crab.target = targetTransform;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Draws a circle within the Scene View that represents the spawn area of the head crabs.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;

        int segments = 16;
        for (int i = 0; i < segments; i++)
        {
            int nextSegment = (i + 1) % segments;
            float startAngle = (float) i / segments * (2 * Mathf.PI);
            float endAngle = (float) nextSegment / segments * (2 * Mathf.PI);
            Vector3 start = spawnRadius * new Vector3(Mathf.Cos(startAngle), 0, Mathf.Sin(startAngle));
            Vector3 end = spawnRadius * new Vector3(Mathf.Cos(endAngle), 0, Mathf.Sin(endAngle));
            Gizmos.DrawLine(start, end);
        }
        
    }
}
