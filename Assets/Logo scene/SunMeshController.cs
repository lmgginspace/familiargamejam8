using System;
using System.Collections.Generic;
using UnityEngine;

public class SunMeshController : MonoBehaviour
{
    public List<GameObject> streakList;

    private System.Random r;
    private List<float> streakSpeed;

	private void Awake()
    {
        this.r = new System.Random(47);
        this.r = new System.Random(r.Next());

        this.streakSpeed = new List<float>();

        foreach (var streak in this.streakList)
        {
            streak.transform.Rotate(0.0f, (float)(r.NextDouble() * 360.0), 0.0f, Space.Self);
            streak.transform.localScale = new Vector3((float)((r.NextDouble() * 0.3333) + 0.3333), 1.0f, (float)(r.NextDouble() + 0.125)) * 100.0f;
            streakSpeed.Add((float)((r.NextDouble() * 12.0) - 8.0));
        }
	}
	
	// Update is called once per frame
	private void Update()
    {
        for (int i = 0; i < this.streakList.Count; i++)
        {
            GameObject streak = this.streakList[i];
            streak.transform.Rotate(0.0f, streakSpeed[i] * Time.deltaTime, 0.0f, Space.Self);
        }
	}

}