[System.Serializable]
public class MonsterSettings
{
    private float damageModifier;
    private float speedModifier;
    private float hpModifier;
    private float absoluteMoneyEarned;

    public MonsterSettings(float damage, float speed, float hp, float moneyModifier)
    {
        this.damageModifier = damage;
        this.speedModifier = speed;
        this.hpModifier = hp;
        this.absoluteMoneyEarned = moneyModifier;
    }

    public float DamageModifier
    {
        get { return damageModifier; }
    }

    public float SpeedModifier
    {
        get { return speedModifier; }
    }

    public float HPModifier
    {
        get { return hpModifier; }
    }

    public float AbsoluteMoneyEarned
    {
        get { return absoluteMoneyEarned; }
    }
}
