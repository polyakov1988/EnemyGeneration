using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GhostSpawner : MonoBehaviour
{
    private readonly int _poolCapacity = 10;
    private readonly int _maxSize = 10;
    private readonly WaitForSeconds _waitForSpawnGhost = new(2);
    
    [SerializeField] private Ghost _ghostPrefab;
    
    private List<SpawnPoint> _spawnPoints;
    private List<Target> _targets;
    private ObjectPool<Ghost> _pool;
    
    private void Awake()
    {
        _spawnPoints = new List<SpawnPoint>();
        _targets = new List<Target>();
        
        _pool = new ObjectPool<Ghost>(createFunc: CreateGhost, 
            actionOnGet: SetGhostState, 
            actionOnRelease:(obj) => obj.gameObject.SetActive(false), 
            actionOnDestroy: Destroy, 
            collectionCheck: true, 
            defaultCapacity: _poolCapacity, 
            maxSize: _maxSize);
        
        GetAllSpawnPoints();
        GetAllTargets();
    }

    private void Start()
    {
        StartCoroutine(SpawnGhost());
    }
    
    private IEnumerator SpawnGhost() {
        while(true)
        {
            Ghost ghost;
            
            if (_pool.CountInactive > 0)
            {
                ghost = _pool.Get();
                
                ghost.gameObject.SetActive(true);
            }
            else
            {
                ghost = CreateGhost();
                
                SetGhostState(ghost);
            }
            
            yield return _waitForSpawnGhost;
        }
    }
    
    public void AddGhostToPool(Ghost ghost)
    {
        _pool.Release(ghost);
    }

    private Ghost CreateGhost()
    {
        return Instantiate(_ghostPrefab);
    }

    private void SetGhostState(Ghost ghost)
    {
        SetGhostPosition(ghost);
        SetGhostTarget(ghost);
    }
    
    private void SetGhostPosition(Ghost ghost)
    {
        SpawnPoint spawn = GetRandomSpawnPoint();
        
        ghost.transform.position = spawn.transform.position;
    }
    
    private void SetGhostTarget(Ghost ghost)
    {
        ghost.SetTarget(GetRandomTarget());
    }
    
    private void GetAllSpawnPoints() 
    {
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            SpawnPoint spawnPoint = tr.GetComponent<SpawnPoint>();
            
            if (spawnPoint) 
                _spawnPoints.Add(spawnPoint); 
        }
    }
    
    private void GetAllTargets() 
    {
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            Target target = tr.GetComponent<Target>();
            
            if (target) 
                _targets.Add(target); 
        }
    }

    private SpawnPoint GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);

        return _spawnPoints[randomIndex];
    }
    
    private Target GetRandomTarget()
    {
        int randomIndex = Random.Range(0, _targets.Count);

        return _targets[randomIndex];
    }
}
