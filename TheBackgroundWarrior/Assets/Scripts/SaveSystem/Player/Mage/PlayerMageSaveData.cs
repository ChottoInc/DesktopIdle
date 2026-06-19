using System.Collections.Generic;
using System.Linq;

public class PlayerMageSaveData
{
    public int levelStatInsight;
    public int levelStatCastSpeed;
    public int levelStatScholar;
    public int levelStatProficiency;

    public int availableStatPoints;

    public int currentLevel;
    public long currentExp;

    public List<SpellSaveData> spells;

    public int currentLearningSpell;

    public int equippedSlot1Spell;
    public int equippedSlot2Spell;
    public int equippedSlot3Spell;
    public int equippedSlot4Spell;

    public PlayerMageSaveData() { }

    public PlayerMageSaveData(PlayerMageData data)
    {
        levelStatInsight = data.LevelStatInsight;
        levelStatCastSpeed = data.LevelStatCastSpeed;
        levelStatScholar = data.LevelStatScholar;
        levelStatProficiency = data.LevelStatProficiency;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        spells = data.Spells.Select(spell => new SpellSaveData(spell)).ToList();

        currentLearningSpell = (int)data.CurrentLearningSpell;

        equippedSlot1Spell = (int)data.EquippedSlot1Spell;
        equippedSlot2Spell = (int)data.EquippedSlot2Spell;
        equippedSlot3Spell = (int)data.EquippedSlot3Spell;
        equippedSlot4Spell = (int)data.EquippedSlot4Spell;
    }
}
