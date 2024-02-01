using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ghost : MonoBehaviour
{
    private static readonly int HasTargetTrigger = Animator.StringToHash("hasTarget");
    
    private Animator _animator;
    private GameObject _target;

    [SerializeField] private float _speed;
    
    public void SetTarget(GameObject target)
    {
        _target = target;

        _animator.SetBool(HasTargetTrigger, true);
    }

    public String GetTargetName()
    {
        return _target ? _target.name : null;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!_target)
        {
            return;
        }
        
        transform.LookAt(_target.transform);
        transform.position = Vector3.Lerp(transform.position, _target.gameObject.transform.position, _speed * Time.deltaTime);
    }
}
