using Articy.ManiacManfred;
using Articy.Unity;
using UnityEngine;

// a scene image takes care of an visual element in the scene that could be visible or invisible depending on the result of an condition script
[AttachBehaviourByTemplate("DisplayConditionTemplate")]
public class SceneImageElement : MonoBehaviour
{
	DisplayCondition imageObject;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
	{
		imageObject = GetComponent<ArticyReference>().GetObject<DisplayCondition>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
	{
		// the referenced sprite renderer is just enabled or disabled depending on the condition
		spriteRenderer.enabled = imageObject.Template.DisplayCondition.ShowMeIf.CallScript();
    }
}
