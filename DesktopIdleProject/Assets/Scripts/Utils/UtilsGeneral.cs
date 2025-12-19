using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class UtilsGeneral
{
    public struct MyColors
    {
        public static Color CommonRarity
        {
            get
            {
                return new Color(255f / 255f, 195f / 255f, 95f / 255f, 1f);
            }
        }

        public static Color UncommonRarity
        {
            get
            {
                return new Color(96f / 255f, 180f / 255f, 255f / 255f, 1f);
            }
        }

        public static Color RareRarity
        {
            get
            {
                return new Color(255f / 255f, 125f / 255f, 95f / 255f, 1f);
            }
        }
    }


    public static Color GetColorByRarity(UtilsItem.CardRarity rarity)
    {
        switch(rarity)
        {
            default:
            case UtilsItem.CardRarity.Common: return MyColors.CommonRarity;
            case UtilsItem.CardRarity.Uncommon: return MyColors.UncommonRarity;
            case UtilsItem.CardRarity.Rare: return MyColors.RareRarity;
        }
    }


    [System.Serializable]
    public struct GeneralChances<T>
    {
        public T value;
        public int chanches;
    }

    public static T GetRandomValueFromGeneralChanches<T>(GeneralChances<T>[] array)
    {
        float randValue = Random.value;
        float tempSumChance = 0;

        T result = default;

        for (int i = 0; i < array.Length; i++)
        {
            tempSumChance += (float)array[i].chanches / 100f;
            if (randValue <= tempSumChance)
            {
                result = array[i].value;
                break;
            }
        }

        return result;
    }



    #region TUTORIAL

    public const int ID_INTRO_TUTORIAL = 0;


    private const string TUTORIAL_INTRO_1 = "This is you.";
    private const string TUTORIAL_INTRO_2 = "Defeat monsters to advance the stages.";
    private const string TUTORIAL_INTRO_3 = "Here you can increase your job levels.";
    private const string TUTORIAL_INTRO_4 = "Here you can change your current job.";
    private const string TUTORIAL_INTRO_5 = "Here you can see your inventory.";

    /// <summary>
    /// Struct containing the dialogue and if the text panel need to move to next position
    /// </summary>
    public struct TutorialDialogueNeedPos
    {
        private readonly string dialogue;
        private readonly bool need;

        public TutorialDialogueNeedPos(string dialgoue, bool need)
        {
            this.dialogue = dialgoue;
            this.need = need;
        }

        public string Dialogue => dialogue;
        public bool Need => need;
    }

    // Tutorial intro
    public static readonly IList<TutorialDialogueNeedPos> TutorialIntroDialogues = new ReadOnlyCollection<TutorialDialogueNeedPos>(
        new[]
        {
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_1, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_2, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_3, true),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_4, true),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_5, true),
        });
    

    // Use to get all the dialogue for a specific tutorial
    public static readonly Dictionary<int, IList<TutorialDialogueNeedPos>> DictTutorials = new Dictionary<int, IList<TutorialDialogueNeedPos>>()
    {
        { ID_INTRO_TUTORIAL, TutorialIntroDialogues }
    };

    #endregion
}
