using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Transform _target;
    
    private void Update()
    {
        if (!_target)
        {
            return;
        }
        
        transform.LookAt(_target);
        transform.position = Vector3.Lerp(transform.position, 
            _target.position, 
            _speed * Time.deltaTime);
    }

    public void InitTarget(Transform target)
    {
        _target = target;
    }
}
