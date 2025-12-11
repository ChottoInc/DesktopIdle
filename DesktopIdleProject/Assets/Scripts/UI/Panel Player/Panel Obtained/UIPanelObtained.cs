using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIPanelObtained : MonoBehaviour
{
    [SerializeField] Sprite spriteOre;
    [SerializeField] Sprite spriteCard;

    [Space(10)]
    [SerializeField] Player player;

    [Space(10)]
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Transform startContentPos;
    [SerializeField] Transform endContentPos;

    [Space(10)]
    [SerializeField] GameObject objectToMove;
    [SerializeField] Image imageObtained;



    //private Vector2 startPos;
    //private Vector2 endPos;

    private Queue<UtilsItem.ItemType> queueItems;

    private Tween tweenMovement;
    private bool isAnimating;

    private void Awake()
    {
        player.OnItemAdd += AddItemToQueue;
    }

    private void OnDestroy()
    {
        player.OnItemAdd -= AddItemToQueue;

        tweenMovement?.Kill();
    }

    private void Start()
    {
        queueItems = new Queue<UtilsItem.ItemType>();
    }


    private void Update()
    {
        if(queueItems.Count > 0 && !isAnimating)
        {
            // if queue not empty and no animation is playing, play new animation
            isAnimating = true;
            Debug.Log("start ani");
            // deque item and set sprite
            UtilsItem.ItemType itemType = queueItems.Dequeue();
            imageObtained.sprite = GetSpriteByType(itemType);

            // reset obejct pos and show
            objectToMove.transform.position = startContentPos.position;
            objectToMove.SetActive(true);

            // move and hide at the end
            tweenMovement = objectToMove.transform.DOMove(endContentPos.position, moveTime).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                objectToMove.SetActive(false);
                isAnimating = false;
                Debug.Log("end ani");
            });
        }
    }




    private void AddItemToQueue(UtilsItem.ItemType itemType)
    {
        // enqueue new item animation to do
        queueItems.Enqueue(itemType);
    }

    private Sprite GetSpriteByType(UtilsItem.ItemType itemType)
    {
        switch(itemType)
        {
            default:
            case UtilsItem.ItemType.Ore: return spriteOre;
            case UtilsItem.ItemType.Card: return spriteCard;
        }
    }
}
