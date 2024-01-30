using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GhostSpawner : MonoBehaviour
{
    private readonly int _poolCapacity = 10;
    private readonly int _maxSize = 10;
    
    [SerializeField] private Ghost _ghostPrefab;
    
    private List<GameObject> _spawnPoints;
    private ObjectPool<GameObject> _pool;
    
    public void AddGhostToPool(GameObject ghost)
    {
        _pool.Release(ghost);
    }
    private void Awake()
    {
        _spawnPoints = new List<GameObject>();
        _pool = new ObjectPool<GameObject>(createFunc: CreateGhost, 
            actionOnGet: SetGhostPosition, 
            actionOnRelease:(obj) => obj.SetActive(false), 
            actionOnDestroy: Destroy, 
            collectionCheck: true, 
            defaultCapacity: _poolCapacity, 
            maxSize: _maxSize);
        
        GetAllSpawnPoints();
    }

    private void Start()
    {
        StartCoroutine(SpawnGhost());
    }
    
    private IEnumerator SpawnGhost() {
        while(true)
        {
            GameObject ghost;
            
            if (_pool.CountInactive > 0)
            {
                ghost = _pool.Get();
                
                ghost.SetActive(true);
            }
            else
            {
                ghost = CreateGhost();
                
                SetGhostPosition(ghost);
            }
            
            yield return new WaitForSeconds(2);
        }
    }

    private GameObject CreateGhost()
    {
        return Instantiate(_ghostPrefab).GameObject();
    }

    private void SetGhostPosition(GameObject ghost)
    {
        GameObject spawn = GetRandomSpawn();
        
        ghost.transform.position = spawn.transform.position;
        ghost.transform.rotation = GetRandomDirection();
    }
    
    private void GetAllSpawnPoints() 
    {
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.GetComponent<SpawnPoint>()) 
                _spawnPoints.Add(tr.gameObject); 
        }
    }

    private GameObject GetRandomSpawn()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);

        return _spawnPoints[randomIndex];
    }

    private Quaternion GetRandomDirection()
    {
        return Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
    }
}
