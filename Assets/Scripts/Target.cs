using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GhostSpawner _ghostSpawner;
    
    private void OnTriggerEnter(Collider other)
    {
        Ghost ghost = other.gameObject.GetComponent<Ghost>();
        
        if (ghost && ghost.GetTargetName() == name)
        {
            _ghostSpawner.AddGhostToPool(other.gameObject);
        }
    }
}
