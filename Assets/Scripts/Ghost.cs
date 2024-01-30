using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ghost : MonoBehaviour
{
    private readonly string _runClipName = "ghost_run";
    private Animator _animator;

    [SerializeField] private float _speed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != _runClipName)
        {
            return;
        }
        
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
