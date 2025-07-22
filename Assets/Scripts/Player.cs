using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Move(Vector3 moveStep, string animationName)
    {
        transform.position += moveStep;
        if(!string.IsNullOrEmpty(animationName))
            _animator.Play(animationName);
    }
}