using Extensions.System;
using System;
using UnityEngine;

public class CanvasBar : MonoBehaviour
{
	// ---- ---- ---- ---- ---- ---- ---- ----
	// Fields
	// ---- ---- ---- ---- ---- ---- ---- ----
	[SerializeField]
	private RectTransform barFill;

	// ---- ---- ---- ---- ---- ---- ---- ----
	// Properties
	// ---- ---- ---- ---- ---- ---- ---- ----
	public float FillPercentage
	{
		get { return this.barFill.anchorMax.x; }
		set { this.barFill.anchorMax = new Vector2(0.0f, value.ClampToUnit()); }
	}

}