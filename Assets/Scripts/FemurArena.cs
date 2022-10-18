using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FemurArena : MonoBehaviour
{
    public HeadCrab headCrabPrefab;
    public Transform spawnTransform;
    public Transform targetTransform;
    public float spawnRadius = 5f;
    public float spawnInterval = 2f;
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
