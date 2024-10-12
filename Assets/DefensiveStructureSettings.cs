[System.Serializable]
public class DefensiveStructureSettings
{
    private float damageModifier;
    private float rangeModifier;
    private float cooldownModifier;
    private float hpModifier;

    public DefensiveStructureSettings(float damage, float range, float cooldown, float hp)
    {
        this.damageModifier = damage;
        this.rangeModifier = range;
        this.cooldownModifier = cooldown;
        this.hpModifier = hp;
    }

    public float DamageModifier
    {
        get { return damageModifier; }
    }

    public float RangeModifier
    {
        get { return rangeModifier; }
    }

    public float CooldownModifier
    {
        get { return cooldownModifier; }
    }

    public float HPModifier
    {
        get { return hpModifier; }
    }
}
