using Extensions.System;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCard : MonoBehaviour
{
	[SerializeField]
	private Image bottleFill;
	[SerializeField]
	private Image cardImage;

	public float BottleFill
	{
		get { return this.bottleFill.fillAmount; }
		set { this.bottleFill.fillAmount = value.ClampToUnit(); }
	}

	public bool Active
	{
		set
		{
			this.cardImage.color = value ? Color.white : Color.gray;
		}
	}

}
