using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IPoolObject
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Death")]
    [SerializeField] ParticleSystem smashVFX;

    private float smashVFXDuration;
    private float timerSmashVFX;

    private RockData rockData;

    private int rockIndex;


    // ------- DEATH

    private bool isSmasVFXPlaying;




    public RockData RockData => rockData;

    public bool IsSmashed => rockData.CurrentDurability <= 0;


    private void Update()
    {
        if (isSmasVFXPlaying)
        {
            CheckSmashVFX();
        }
    }

    private void CheckSmashVFX()
    {
        if (timerSmashVFX <= 0)
        {
            isSmasVFXPlaying = false;
            HideAfterSmash();
        }
        else
        {
            timerSmashVFX -= Time.deltaTime;
        }
    }

    public void Setup(RockData rockData, int index)
    {
        this.rockData = rockData;

        rockIndex = index;

        spriteRenderer.sortingOrder = rockIndex;
    }

    private void HideSprite(bool hide)
    {
        // save initial color
        Color spriteColor = spriteRenderer.color;

        if (hide)
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        else
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
    }

    public void PlayDeath(bool setSmashed)
    {
        if (setSmashed)
            rockData.SetSmashed();

        HideSprite(true);

        // play vfx
        smashVFX.Play();
        timerSmashVFX = smashVFXDuration;
        isSmasVFXPlaying = true;
    }

    private void HideAfterSmash()
    {
        smashVFX.Stop();
        Die();
    }

    public void OnSpawn()
    {
        HideSprite(false);
    }

    public void OnDespawn()
    {

    }

    public void Die()
    {
        //StageManager.Instance.RemoveFromCurrentEnemiesList(this);
        PoolManager.Instance.Return(gameObject, "Rock");
    }




    public override bool Equals(object other)
    {
        Rock otherEnemy = other as Rock;
        return rockIndex == otherEnemy.rockIndex;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
