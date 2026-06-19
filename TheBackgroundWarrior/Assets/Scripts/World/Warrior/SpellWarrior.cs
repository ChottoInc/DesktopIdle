using UnityEngine;

public class SpellWarrior : SpellMage
{
    [Space(10)]
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] protected bool faceRight;

    protected Vector3 _startScale;


    protected Enemy _targetEnemy;

    protected bool _canDestroy;
    protected bool _requestedDestroy;


    protected SpellData _spellData;

    public void SetData(SpellData spellData)
    {
        _spellData = spellData;
    }

    public void SetTargetEnemy(Enemy targetEnemy)
    {
        _targetEnemy = targetEnemy;
    }

    protected void Start()
    {
        _startScale = _spriteRenderer.transform.localScale;
    }

    protected virtual void Update()
    {
        if(_canDestroy && _requestedDestroy)
        {
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {
        if(_targetEnemy != null)
        {
            _target = _targetEnemy.transform.position;
        }

        base.FixedUpdate();

        CheckFlip();
    }

    protected void CheckFlip()
    {
        // check sprite flip
        float vx = _currentDirection.x;
        if (vx > 0.01f && faceRight)
        {
            _spriteRenderer.transform.localScale = _startScale;
        }
        else if (vx > 0.01f && !faceRight)
        {
            _spriteRenderer.transform.localScale = new Vector3(-_startScale.x, _startScale.y, _startScale.z);
        }
        else if (vx < -0.01f && faceRight)
        {
            _spriteRenderer.transform.localScale = new Vector3(-_startScale.x, _startScale.y, _startScale.z);
        }
        else if (vx < -0.01f && !faceRight)
        {
            _spriteRenderer.transform.localScale = _startScale;
        }
    }

    public void ExternalMakeEffect()
    {
        MakeEffect();
    }

    protected virtual void MakeEffect()
    {
        Debug.Log("override to make effects");
    }

    public override void EndAnimation()
    {
        // request to be destroyed, once it can after the effects it will be destroyed
        _requestedDestroy = true;
    }
}
