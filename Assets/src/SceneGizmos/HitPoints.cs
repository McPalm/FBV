using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// A network script that syncronizes hitpoints across all clients.
/// All changes has to be done serverside or they will not go through.
/// Events are fired clientside. They should be used to update UI elements.
/// </summary>

public class HitPoints : NetworkBehaviour
{
	[SerializeField, SyncVar(hook = "OnChangeMax")]
	int maxHealth = 10;
	[SerializeField, SyncVar(hook = "OnChangeDamage")]
	int damageTaken;
	
	public HealthEvent EventChangeHealth;
	public IntEvent EventHurt;
	public IntEvent EventHeal;
	public UnityEvent EventDeath;

	void Start()
	{
		EventChangeHealth.Invoke(CurrentHealth, MaxHealth);
	}

	public int CurrentHealth
	{
		get
		{
			return maxHealth - damageTaken;
		}
		set
		{
			if (maxHealth - value < 0)
				damageTaken = 0;
			else
				damageTaken = maxHealth - value;
		}
	}
	public float HealthPercent
	{
		get
		{
			return CurrentHealth / (float)maxHealth;
		}
	}
	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}

		set
		{
			maxHealth = value;
		}
	}

	public void Hurt(int d)
	{
		damageTaken += d;
		EventHurt.Invoke(d);
	}
	
	public void Heal(int d)
	{
		CurrentHealth += d;
		EventHeal.Invoke(d);
	}

	void OnChangeDamage(int damageTaken)
	{
		this.damageTaken = damageTaken;
		EventChangeHealth.Invoke(maxHealth - damageTaken, maxHealth);
		if (damageTaken >= maxHealth) EventDeath.Invoke();
	}

	void OnChangeMax(int max)
	{
		EventChangeHealth.Invoke(max - damageTaken, max);
		if (damageTaken >= maxHealth) EventDeath.Invoke();
	}

	[System.Serializable]
	public class HealthEvent : UnityEvent<int, int> { }
	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }
}
