using Articy.ManiacManfred;
using Articy.Unity;
using UnityEngine;

/// <summary>
/// Every Location in the articy project is also a scene in unity and every scene could, once loaded, contain a dialog that we want to trigger directly
/// This behaviour will take care of that.
/// </summary>
public class GameLocation : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// our LocationCreator settings behaviour (ArticyLocation) also holds a reference to the location has our location
		var location = (LocationSettings)GetComponent<ArticyLocation>().location;

		var sceneHandler = FindObjectOfType<SceneHandler>();
		// now we just need to check the locations InitialDialog and give it to the scene handler
		if (location.Template.LocationSettings.InitialDialog != null)
		{
			// going into a location and triggering the initial dialog is bound to a condition
			if(location.Template.LocationSettings.InitialDialogCondition.CallScript())
				sceneHandler.ContinueFlow(location.Template.LocationSettings.InitialDialog);
		}
	}
}
