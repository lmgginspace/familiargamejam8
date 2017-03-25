using Extensions.System;
using System;
using UnityEngine;

public class CanvasBar : MonoBehaviour
{
	// ---- ---- ---- ---- ---- ---- ---- ----
	// Fields
	// ---- ---- ---- ---- ---- ---- ---- ----
	public RectTransform barHealth;
    public RectTransform manaHealth;

    public GameObject target;

	// ---- ---- ---- ---- ---- ---- ---- ----
	// Properties
	// ---- ---- ---- ---- ---- ---- ---- ----
	public float FillHealth
	{
		get { return this.barHealth.anchorMax.x; }
		set { this.barHealth.anchorMax = new Vector2(value.ClampToUnit(), 1); }
	}

    public float FillMana {
        get { return this.manaHealth.anchorMax.x; }
        set { this.manaHealth.anchorMax = new Vector2(value.ClampToUnit(), 1); }
    }

    private void Update() {
        if (target.tag == "Hero") {
            FillHealth = target.GetComponent<Hero>().health / target.GetComponent<Hero>().healthMax;
            FillMana = target.GetComponent<Hero>().mana / target.GetComponent<Hero>().manaMax;
        }
        if (target.tag == "Villain") {
            FillHealth = target.GetComponent<Villain>().health / target.GetComponent<Villain>().healthMax;
        }
    }

}