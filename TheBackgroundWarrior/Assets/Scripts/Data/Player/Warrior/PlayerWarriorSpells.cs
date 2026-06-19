using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWarriorSpells : MonoBehaviour
{
    [Header("Settings")]

    // dictionary to get the prefabs of the spells
    [SerializeField] DictionarySpellTypeToPrefab[] _dictTypeToPrefabs;

    [Header("Cast")]
    [SerializeField] Transform _castPosition;

    // contains all timers for equipped slots
    private List<TimerSpell> _timerSpells;

    public bool IsChangingScene { get; set; }


    private void OnDestroy()
    {
        PlayerManager.Instance.PlayerMageData.OnEquippedSpellUpdate -= RefreshEquippedSlots;
    }

    private void Awake()
    {
        PlayerManager.Instance.PlayerMageData.OnEquippedSpellUpdate += RefreshEquippedSlots;
    }

    private void Start()
    {
        _timerSpells = new List<TimerSpell>();

        SetupEquippedSlots(true);
    }

    private void Update()
    {
        if (IsChangingScene) return;

        HandleTimers();
    }

    private void HandleTimers()
    {
        foreach (var timerSpell in _timerSpells)
        {
            if(timerSpell.timer <= 0f)
            {
                if(CombatManager.Instance != null)
                {
                    // get random enemy to shoot to
                    Enemy randEnemy = StageManager.Instance.GetRandomNonCombatEnemy();
                    if (randEnemy != null)
                    {
                        // get prefab then cast
                        GameObject prefab = GetPrefabByType(timerSpell.spellType);
                        SpawnSpell(prefab, PlayerManager.Instance.PlayerMageData.GetSpellByType(timerSpell.spellType), randEnemy);
                    }
                }
                
                // refresh the cooldown if has any changes then reset the timer
                RefreshTimer(timerSpell);
                timerSpell.timer = timerSpell.cooldown;
            }
            else
            {
                timerSpell.timer -= Time.deltaTime;
            }
        }
    }

    private void SpawnSpell(GameObject prefab, SpellData spellData, Enemy enemy)
    {
        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.identity);

        if (spawned.TryGetComponent(out SpellWarrior spellWarrior))
        {
            spellWarrior.SetData(spellData);

            if (spellWarrior.DoesMove)
            {
                spellWarrior.SetPositions(_castPosition.position, new Vector2(enemy.transform.position.x, _castPosition.position.y));
            }
            else
            {
                spellWarrior.SetPositions(new Vector2(enemy.transform.position.x, _castPosition.position.y), new Vector2(enemy.transform.position.x, _castPosition.position.y));
            }

            // if it's a single target spell and the enemy is in movement, the spell needs to follow
            if(spellData.SpellSO.TargetType == UtilsMage.SpellTargetType.Single)
            {
                spellWarrior.SetTargetEnemy(enemy);
            }

            spellWarrior.Perform();
        }
    }

    private void RefreshEquippedSlots()
    {
        SetupEquippedSlots(false);
    }

    private void SetupEquippedSlots(bool fromStart)
    {
        PlayerMageData data = PlayerManager.Instance.PlayerMageData;

        if (fromStart)
        {
            // basic setup at start, only creates timers where spels are equipped
            if (data.EquippedSlot1Spell != UtilsMage.MageSpellType.None)
            {
                AddTimerToList(data, data.EquippedSlot1Spell);
            }

            if (data.EquippedSlot2Spell != UtilsMage.MageSpellType.None)
            {
                AddTimerToList(data, data.EquippedSlot2Spell);
            }

            if (data.EquippedSlot3Spell != UtilsMage.MageSpellType.None)
            {
                AddTimerToList(data, data.EquippedSlot3Spell);
            }

            if (data.EquippedSlot4Spell != UtilsMage.MageSpellType.None)
            {
                AddTimerToList(data, data.EquippedSlot4Spell);
            }
        }
        else
        {
            // check for added or changed slots, even removed
            if (data.EquippedSlot1Spell != UtilsMage.MageSpellType.None)
            {
                int indexTimer = GetTimerIndexBySpellType(data.EquippedSlot1Spell);
                if(indexTimer < 0)  // if less than 0, need to add to list, else nothing changes
                {
                    AddTimerToList(data, data.EquippedSlot1Spell);
                }
            }

            if (data.EquippedSlot2Spell != UtilsMage.MageSpellType.None)
            {
                int indexTimer = GetTimerIndexBySpellType(data.EquippedSlot2Spell);
                if (indexTimer < 0)  // if less than 0, need to add to list, else nothing changes
                {
                    AddTimerToList(data, data.EquippedSlot2Spell);
                }
            }

            if (data.EquippedSlot3Spell != UtilsMage.MageSpellType.None)
            {
                int indexTimer = GetTimerIndexBySpellType(data.EquippedSlot3Spell);
                if (indexTimer < 0)  // if less than 0, need to add to list, else nothing changes
                {
                    AddTimerToList(data, data.EquippedSlot3Spell);
                }
            }

            if (data.EquippedSlot4Spell != UtilsMage.MageSpellType.None)
            {
                int indexTimer = GetTimerIndexBySpellType(data.EquippedSlot4Spell);
                if (indexTimer < 0)  // if less than 0, need to add to list, else nothing changes
                {
                    AddTimerToList(data, data.EquippedSlot4Spell);
                }
            }

            // now loop through timers and find which ones are not equipped anymore and erase them
            for (int i = _timerSpells.Count - 1; i >= 0; i--)
            {
                SpellData spell = data.GetSpellByType(_timerSpells[i].spellType);
                if (!data.IsSpellEquipped(spell))
                {
                    _timerSpells.RemoveAt(i);
                }
            }
        }
    }

    private int GetTimerIndexBySpellType(UtilsMage.MageSpellType spellType)
    {
        return _timerSpells.FindIndex(timer => timer.spellType == spellType);
    }

    /// <summary>
    /// Create a new TimerSpell and add to the list
    /// </summary>
    private void AddTimerToList(PlayerMageData data, UtilsMage.MageSpellType spellType)
    {
        SpellData spellData = data.GetSpellByType(spellType);
        _timerSpells.Add(new TimerSpell(spellType, spellData.CooldownCast));
    }


    /// <summary>
    /// Refresh the timer of the spell
    /// </summary>
    private void RefreshTimer(TimerSpell timer)
    {
        timer.cooldown = PlayerManager.Instance.PlayerMageData.GetSpellByType(timer.spellType).CooldownCast;
    }


    /// <summary>
    /// Returns the prefab of the spell by  the type
    /// </summary>
    private GameObject GetPrefabByType(UtilsMage.MageSpellType spellType)
    {
        return _dictTypeToPrefabs.FirstOrDefault(pair => pair.spellType == spellType)?.spellPrefab;
    }
}

[System.Serializable]
public class DictionarySpellTypeToPrefab
{
    public UtilsMage.MageSpellType spellType;
    public GameObject spellPrefab;
}

[System.Serializable]
public class TimerSpell
{
    public UtilsMage.MageSpellType spellType;
    public float cooldown;
    public float timer;

    public TimerSpell(UtilsMage.MageSpellType spellType, float cooldown)
    {
        this.spellType = spellType;
        this.cooldown = cooldown;
        timer = cooldown;
    }
}