using System;
using System.Collections.Generic;
using Articy.ManiacManfred;
using UnityEngine;

// this class will take care of caching our cursor textures and making it easier to set it depending on our MouseCursor Enum
public class MouseCursorResources : MonoBehaviour
{
	// a cached dictionary mapping the enum to the textures
	private Dictionary<MouseCursor, Texture2D> cursors;

	// get the associated texture for the enum value
	// Auto should never be passed in here
	public Texture2D GetCursor(MouseCursor aCursor)
	{
		if (aCursor == MouseCursor.Auto)
			return null;
		return cursors[aCursor];
	}
	// set the cursor to the enum value
	public void SetCursor(MouseCursor aCursor)
	{
		var cursor = GetCursor(aCursor);
		Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
	}

	// Clear cursor changes it back to the system default one
	public void ClearCursor()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	// On start we load all the cursor textures once and cache them
	void Start()
	{
		cursors = new Dictionary<MouseCursor, Texture2D>();

		// this allows us to iterate over the values of an enum
		foreach (var val in Enum.GetValues(typeof(MouseCursor)))
		{
			// then we turn the enum value into a string and use that to build the name of the texture, representing the enum value
			var name = val.ToString();
			// here we load the texture, its important to understand that the image must be named precisely after the enum value
			// otherwise this would not work
			var icon = Resources.Load<Texture2D>(string.Format("Assets/Images/UI/mouse_{0}", name));
			if (icon != null)
			{
				// and finally we cache the texture with the enum value
				cursors.Add((MouseCursor)val, icon);
			}
		}
		// The Use enum value, does not have an image assigned, instead we reuse the point texture
		cursors.Add(MouseCursor.Use, Resources.Load<Texture2D>("Assets/Images/UI/mouse_point"));
	}
}
