using Articy.ManiacManfred;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;

/// <summary>
/// A location usually has only a single background image, but if it has more than one, this
/// class will make sure to select the correct one, when entering the location.
/// </summary>
public class BackgroundImageHandler : MonoBehaviour
{
	// the image object in the articy location that represents the background image
	[ArticyTypeConstraint(typeof(ILocationImage))]
	public ArticyRef imageElement;
	// reference to the location
	private LocationSettings currentLocation;

	// Use this for initialization
	void Start()
	{
		// we get the current location object from the behaviour that was used for the location creator, as it already holds a reference to the current location
		currentLocation = GetComponent<ArticyLocation>().location.GetObject< LocationSettings>();
		// here we make sure that the correct background image is selected
		EnsureCurrentBackgroundImage();
	}
	/// <summary>
	/// Make sure to select the correct background image
	/// </summary>
	private void EnsureCurrentBackgroundImage()
	{
		// Instead of referencing the image component directly, and therefore breaking the code every time the location generator
		// recreates the game objects. We stored the articy game object that represents the background image, and now search for the game object
		// that was created representing the background image object.
		var childRefs = GetComponentsInChildren<ArticyReference>(true);
		SpriteRenderer spriteRenderer = null;
		foreach (var child in childRefs)
		{
			// same id, means the image object we stored, and the one stored on this game object is the same
			// means, this game object is our background image representation
			if (child.reference.id == imageElement.id)
			{
				// so we take its sprite renderer.
				spriteRenderer = child.gameObject.GetComponent<SpriteRenderer>();
				break;
			}
		}

		// if we don't find one, we might not have yet generated the game objects
		if (spriteRenderer == null)
		{
			Debug.LogError("Image game object not found!");
			return;
		}

		// the template contains a reference strip with a list of alternative backgrounds
		// we iterate over every one
		foreach (var background in currentLocation.Template.LocationSettings.Backgrounds)
		{
			var varBinding = background as BackgroundImage;
			if (varBinding != null)
			{
				// and call its script, checking if this texture should be used
				var shouldBeVisible = varBinding.Template.VariableBinding.VariableName.CallScript(null);
				if (shouldBeVisible)
				{
					// now we just create the texture from the image, using the cliprect
					var tex = varBinding.LoadAsset<Texture2D>();
					var clipRect = imageElement.GetObject<LocationImage>().ClipRect;
					var sourceRect = ArticyUtility.ConvertToSpriteSourceRect(tex, clipRect);
					// and finally we create a sprite from the texture and rect
					spriteRenderer.sprite = Sprite.Create(tex, sourceRect, new Vector2());
					break;
				}
			}
		}
	}

}
