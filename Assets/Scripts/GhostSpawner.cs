using System.Collections;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    private readonly WaitForSeconds _waitForSpawnGhost = new(2);
    
    [SerializeField] private Ghost _ghostPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _target;

    private void Start()
    {
        StartCoroutine(SpawnGhost());
    }
    
    private IEnumerator SpawnGhost() {
        while(true)
        {
            Ghost ghost = CreateGhost();
                
            SetGhostPosition(ghost);
            
            yield return _waitForSpawnGhost;
        }
    }

    private Ghost CreateGhost()
    {
        Ghost ghost = Instantiate(_ghostPrefab);
        ghost.InitTarget(_target);
        
        return ghost;
    }
    
    private void SetGhostPosition(Ghost ghost)
    {
        ghost.transform.position = _spawnPoint.transform.position;
    }
}
