using System;

public class Unit
{
    public event Action<Unit> OnDie;
    public UnitTeam UnitTeam { get; private set; }
    public float CurrentHealth { get; private set; }
    public IStats Stats { get; private set; }

    private bool isDead;

    public Unit(UnitTeam unitTeam, IStats stats)
    {
        UnitTeam = unitTeam;
        Stats = stats;
        CurrentHealth = Stats.HP;
    }

    public void ApplyDamage(float value)
    {
        if(isDead) return;

        CurrentHealth -= value;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnDie?.Invoke(this);
    }
}

