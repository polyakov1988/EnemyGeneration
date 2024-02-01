using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GhostSpawner _ghostSpawner;
    
    private void OnTriggerEnter(Collider other)
    {
        Ghost ghost = other.GetComponent<Ghost>();
        
        if (ghost && this == ghost.GetTarget())
        {
            _ghostSpawner.AddGhostToPool(ghost);
        }
    }
}
