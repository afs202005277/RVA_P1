using System.Collections.Generic;

[System.Serializable]
public class DifficultySettings
{
    private DefensiveStructureSettings defensiveStructureSettings;
    private Dictionary<string, MonsterSettings> monsterSettings; // Dictionary for monster settings
    private float passiveMoneyEarned;
    private float wallCost;
    private float towerCost;

    public DifficultySettings(DefensiveStructureSettings defensiveStructureSettings,
                              Dictionary<string, MonsterSettings> monsterSettings,
                              float passiveMoneyEarned, float wallCost, float towerCost)
    {
        this.defensiveStructureSettings = defensiveStructureSettings;
        this.monsterSettings = monsterSettings;
        this.passiveMoneyEarned = passiveMoneyEarned;
        this.wallCost = wallCost;
        this.towerCost = towerCost;
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

    public float WallCost
    {
        get { return wallCost; }
    }

    public float TowerCost
    {
        get { return towerCost; }
    }
}
