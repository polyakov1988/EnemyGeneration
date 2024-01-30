using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GhostSpawner : MonoBehaviour
{
    private readonly int _poolCapacity = 10;
    private readonly int _maxSize = 10;
    
    private List<GameObject> _spawns;
    private ObjectPool<GameObject> _pool;
    private int _currentSpawn;
    
    [SerializeField] private Ghost _ghostPrefab;

    public void AddGhostToPool(GameObject ghost)
    {
        _pool.Release(ghost);
    }
    void Awake()
    {
        _spawns = new List<GameObject>();
        _pool = new ObjectPool<GameObject>(createFunc: CreateGhost, 
            actionOnGet: SetGhostPosition, 
            actionOnRelease:(obj) => obj.SetActive(false), 
            actionOnDestroy: Destroy, 
            collectionCheck: true, 
            defaultCapacity: _poolCapacity, 
            maxSize: _maxSize);
        
        GetAllSpawns();
    }

    void Start()
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
        ghost.transform.rotation = spawn.transform.rotation;
    }
    
    private void GetAllSpawns() 
    {
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.GetComponent<Spawn>()) 
                _spawns.Add(tr.gameObject); 
        }
    }

    private GameObject GetRandomSpawn()
    {
        int randomIndex = Random.Range(0, _spawns.Count);

        return _spawns[randomIndex];
    }
}
