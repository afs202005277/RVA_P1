using System.Collections.Generic;

[System.Serializable]
public class DifficultySettings
{
    private DefensiveStructureSettings defensiveStructureSettings;
    private Dictionary<string, MonsterSettings> monsterSettings; // Dictionary for monster settings
    private float passiveMoneyEarned;

    public DifficultySettings(DefensiveStructureSettings defensiveStructureSettings,
                              Dictionary<string, MonsterSettings> monsterSettings,
                              float passiveMoneyEarned)
    {
        this.defensiveStructureSettings = defensiveStructureSettings;
        this.monsterSettings = monsterSettings;
        this.passiveMoneyEarned = passiveMoneyEarned;
    }

    public DefensiveStructureSettings DefensiveStructureSettings
    {
        get { return defensiveStructureSettings; }
    }

    public Dictionary<string, MonsterSettings> MonsterSettings
    {
        get { return monsterSettings; }
    }

    public float PassiveMoneyEarned
    {
        get { return passiveMoneyEarned; }
    }
}
