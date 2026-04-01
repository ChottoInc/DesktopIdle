using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelDamage : MonoBehaviour
{
    [Space(10)]
    [SerializeField] Enemy enemy;

    [Space(10)]
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Transform startContentPos;
    [SerializeField] Transform endContentPos;

    [Space(10)]
    [SerializeField] GameObject objectToMove;
    [SerializeField] TMP_Text textDamage;

    private Queue<int> queueDamages;


    private Tween tweenMovement;
    private Tween tweenScale;

    private bool isAnimating;
    private Vector3 startScaleObject;


    private void Awake()
    {
        startScaleObject = objectToMove.transform.localScale;
    }

    /// <summary>
    /// Called from enemy itself, after the data has been set
    /// </summary>
    public void Setup()
    {
        enemy.EnemyData.OnTakeDamage += AddDamageToQueue;
    }

    private void OnDestroy()
    {
        if(enemy.EnemyData != null)
            enemy.EnemyData.OnTakeDamage -= AddDamageToQueue;

        tweenMovement?.Kill();

        tweenScale?.Kill();
    }

    private void Start()
    {
        queueDamages = new Queue<int>();
    }


    public void ShowDamage()
    {
        if (queueDamages.Count > 0 && !isAnimating)
        {
            // if queue not empty and no animation is playing, play new animation
            isAnimating = true;

            //Debug.Log("start ani");
            // dequeue item and set sprite
            int damage = queueDamages.Dequeue();
            textDamage.text = damage.ToString();

            // reset obejct pos/scale and show
            objectToMove.transform.position = startContentPos.position;
            objectToMove.transform.localScale = startScaleObject;
            objectToMove.SetActive(true);

            // move and hide at the end
            tweenMovement = objectToMove.transform.DOMove(endContentPos.position, moveTime).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                objectToMove.SetActive(false);
                isAnimating = false;

                // reset starting scale
                objectToMove.transform.localScale = startScaleObject;
                //Debug.Log("end ani");
            });

            tweenScale = objectToMove.transform.DOScale(0, moveTime).SetEase(Ease.InOutSine);
        }
        else if (isAnimating)
        {
            queueDamages.Dequeue();
        }
    }




    private void AddDamageToQueue(int damage)
    {
        if (queueDamages.Count > 0)
            queueDamages.Clear();

        // enqueue new item animation to do
        queueDamages.Enqueue(damage);
    }
}
