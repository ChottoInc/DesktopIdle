using UnityEngine;

public class SpellMage : MonoBehaviour, IMageSpell
{
    [Header("Settings")]
    [SerializeField] protected bool _doesMove;
    [SerializeField] protected float _speed;

    [Header("Animations")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected bool _hasDeathAnimation;
    

    public bool DoesMove => _doesMove;

    protected Vector2 _target;

    protected Vector2 _currentDirection;

    protected bool _canPerform;

    public void SetPositions(Vector2 startPos, Vector2 target)
    {
        transform.position = startPos;

        _target = target;
    }

    public void Perform()
    {
        _canPerform = true;
    }


    protected virtual void FixedUpdate()
    {
        if (!_canPerform) return;

        float distance = Mathf.Abs(transform.position.x - _target.x);

        if (distance > 0.1f)
        {
            // get target dir
            _currentDirection = new Vector2(_target.x - transform.position.x, 0).normalized;

            // move
            transform.position += _speed * Time.fixedDeltaTime * (Vector3)_currentDirection;
        }
        else
        {
            CheckDeathVFX();

            _canPerform = false;
        }
    }

    protected virtual void CheckDeathVFX()
    {
        if (_hasDeathAnimation)
        {
            _animator.SetTrigger("Death");
        }
    }

    public virtual void EndAnimation()
    {
        Destroy(gameObject);
    }
}
