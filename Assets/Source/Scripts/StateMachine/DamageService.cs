
public class DamageService
{
    public void ApplyDamage(UnitView target, float damage)
    {
        if (target == null || target.Unit?.CurrentHealth == 0) return;

        target.Unit?.ApplyDamage(damage);
    }
}