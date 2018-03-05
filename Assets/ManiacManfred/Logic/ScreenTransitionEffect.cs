using UnityEngine;
using System.Collections;

// this is just a nice little transition effect, when we change scene
[ExecuteInEditMode]
public class ScreenTransitionEffect : MonoBehaviour
{
	[Header("UI Elements")]
	public RectTransform top;
	public RectTransform left;
	public RectTransform right;
	public RectTransform bottom;

	[Range(0.0f, 1.0f)]
	public float percentage;

	private float time;
	private RectTransform barParent;

	private bool transitionIn;
	private bool transitionOut;

	// Use this for initialization
	void Start()
	{
		if (top == null)
			return;
		var parent = top.parent;
		barParent = parent.GetComponent<RectTransform>();
	}

	public void TransitionIn()
	{
		var parent = top.parent;
		barParent = parent.GetComponent<RectTransform>();

		transitionIn = true;
		transitionOut = false;

		AssignTransitionPercentage(1.0f);
	}

	public void TransitionOut()
	{
		var parent = top.parent;
		barParent = parent.GetComponent<RectTransform>();

		transitionIn = false;
		transitionOut = true;

		AssignTransitionPercentage(0.0f);
	}

	// Update is called once per frame
	void Update()
	{
		if (transitionOut)
		{
			time += Time.deltaTime;
			percentage = Mathf.Lerp(0, 1, time);
			if (percentage >= 1.0f)
			{
				transitionOut = false;
				time = 0.0f;
			}
		}

		if (transitionIn)
		{
			time += Time.deltaTime;
			percentage = Mathf.Lerp(1, 0, time);
			if (percentage <= 0.0f)
			{
				transitionIn = false;
				time = 0.0f;
			}
		}

		AssignTransitionPercentage(percentage);
	}

	private void AssignTransitionPercentage(float aPercentage)
	{
		if (barParent == null)
			return;

		var halfWidth = (barParent.rect.width / 2.0f) * aPercentage;
		var halfHeight = (barParent.rect.height / 2.0f) * aPercentage;

		top.sizeDelta = new Vector2(0, halfHeight);
		left.sizeDelta = new Vector2(halfWidth, 0);
		right.sizeDelta = new Vector2(halfWidth, 0);
		bottom.sizeDelta = new Vector2(0, halfHeight);
	}
}
