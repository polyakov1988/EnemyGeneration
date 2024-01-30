using UnityEngine;

public class ActiveArea : MonoBehaviour
{
    [SerializeField] private GhostSpawner _ghostSpawner;

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Ghost>())
        {
            _ghostSpawner.AddGhostToPool(other.gameObject);
        }
    }
}
