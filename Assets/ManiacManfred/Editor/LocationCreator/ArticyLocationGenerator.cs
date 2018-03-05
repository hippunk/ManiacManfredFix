using System;
using System.Collections.Generic;
using System.Reflection;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class will take care of populating a unity scene with elements from an articy location.
/// The goal of this class is to very quickly and easily fill your scene from an articy location and it should give you an idea how you can use the plugin
/// for editor scripts.
/// 
/// Note: This class will make use of some reflection voodoo, mostly to make sure it compiles even if you have not imported an articy project yet or excluded some properties
/// usually this is not necessary.
/// Also this class is not a fully LocationGenerator and is used specifically for the Maniac Manfred Adventure Demo project, while it might work for your projects, you might have to 
/// modify a lot of it to make it work for you.
/// </summary>
public static class ArticyLocationGenerator
{
	/// <summary>
	/// Convenience method to get the value of a property from an articy object. 
	/// The only reason we are using reflection here is to make sure this class always compiles and does not rely on generated code files.
	/// </summary>
	private static _T GetProperty<_T>(ArticyObject aObject, string aProperty)
	{
		return (_T)aObject.GetType().GetProperty(aProperty, BindingFlags.Instance | BindingFlags.Public).GetValue(aObject, null);
	}

	/// <summary>
	/// Starts the creation process.
	/// This will effectively create a new gameobject for every object in our articy location, keeping the 
	/// </summary>
	public static void GenerateLocation(GameObject aTarget, ILocation aLocation, int aPixelsToUnits)
	{
		var mdl = aLocation as ArticyObject;
		var attachBehaviours = new Dictionary<string, Type>();

		// The first thing we do is, find all the user scripts inside the project
		MonoScript[] types = (MonoScript[])Resources.FindObjectsOfTypeAll(typeof(MonoScript));

		foreach (var script in types)
		{
			if (script == null) continue;

			var type = script.GetClass();

			if (type == null) continue;
			// now we check if this script contains our AttachBehaviorByTemplate attribute
			var attributes = type.GetCustomAttributes(typeof(AttachBehaviourByTemplateAttribute), true);
			if (attributes.Length > 0)
			{
				// if it does, we extract the template name that links both script and template
				var attach = attributes[0] as AttachBehaviourByTemplateAttribute;

				// now we only need to cache the type of the behavior to the template name for later use
				attachBehaviours[attach.TemplateName] = type;
			}
		}

		// here we calculate the bounds of the 2D elements we are going to create
		Rect overallBounds = new Rect();
		foreach (var child in mdl.Children)
		{
			// the only elements that could actually change the bounds are objects with vertices (Zones, images etc)
			var vertices = child as IObjectWithVertices;

			if (vertices != null)
			{
				var childBounds = ArticyUtility.GetBounds(vertices.Vertices);
				var correctedPoints = new Vector2[vertices.Vertices.Count];
				int i = 0;
				// articy and unity have a different y axis, so we have to take care of that and also incorporating the pixels to units conversion
				foreach (var vec in vertices.Vertices)
				{
					var v = new Vector2(vec.x / aPixelsToUnits, (childBounds.yMax - vec.y) / aPixelsToUnits);
					correctedPoints[i++] = v;
				}
				childBounds = ArticyUtility.GetBounds(correctedPoints);
				overallBounds = ArticyUtility.Union(overallBounds, childBounds);
			}
		}
		// and now we can create all the elements inside the location
		int zindex = 0;
		GenerateChildren(aTarget.transform, mdl, aPixelsToUnits, overallBounds, attachBehaviours, ref zindex);
	}

	private static string GetNameForGameObject(ArticyObject aObject)
	{
		string finalName = aObject.TechnicalName;
		var displayName = aObject as IObjectWithDisplayName;
		if (displayName != null)
			finalName = displayName.DisplayName;

		var target = aObject as IObjectWithTarget;
		if(target != null)
		{
			var targetObject = target.Target;
			if (targetObject != null)
				return GetNameForGameObject(targetObject);
		} 

		return finalName;
    }

	/// <summary>
	/// this method is called for every articy object in the location, starting with the location itself
	/// </summary>
	private static void GenerateChildren(Transform aParent, ArticyObject aChild, int aPixelsToUnits, Rect aOverallBounds, Dictionary<string, Type> aAttachBehaviours, ref int aZindex)
	{
		// the children collection is usually unsorted, but in this case we have to worry about proper ordering regarding z sorting
		// luckly the plugin has a way to return the children sorted by a user defined method, in this case we sort our children by their zindex
		var sortedChildren = aChild.GetSortedChildren<IObjectWithZIndex>((x, y) => x.ZIndex.CompareTo(y.ZIndex) * -1);
		foreach (var sortedChild in sortedChildren)
		{
			var child = sortedChild as ArticyObject;
			var gameObjectName = GetNameForGameObject(child);
            bool hasZoneScript = false;

			#region Create new Gameobject for our Location child
			// we create a game object for the new object and parent it to our previous game object
			var childGameObject = new GameObject(gameObjectName);
			childGameObject.transform.SetParent(aParent, false);
			EditorUtility.SetDirty(childGameObject);
			Rect bounds = new Rect();
			bool boundsSet = false;

			// then we add an articyReference to it, storing the articy object that this new game object represents with it
			var artRef = childGameObject.AddComponent<ArticyReference>();
			artRef.reference = new ArticyRef() { id = child.Id };
			#endregion

			#region Attach Behaviours By Template to the new game object
			// now we use the previously created cache of behaviours for specific templates.
			// So first we check via reflection, if the object has a template property
			var templateProp = child.GetType().GetProperty("Template");
			if (templateProp != null)
			{
				// and if it does, we get the value of its Template property
				var templateObj = templateProp.GetValue(child, null);
				if (templateObj != null)
				{
					// now we can extract its class name, which usually is the Template Technical name with a Template suffix "Condition_ZoneTemplate" for example.
					var templateName = templateObj.GetType().Name;
					Type behaviour;
					// and here we ask the cache if it has a registered behaviour under the current template name
					if (aAttachBehaviours.TryGetValue(templateName, out behaviour))
					{
						// if it does, we add a new script component using the stored type
						childGameObject.AddComponent(behaviour);
						// we also store if it was a clickable zone behaviour, because then we have to create the collider and enable it
						// location images for example could also have an attached behaviour, but we don't want them to interfere with our raycasting
						if (behaviour == typeof(ClickableZone))
							hasZoneScript = true;
					}
				}
			}
			#endregion

			#region Create a Collider if it has vertices
			// all objects with vertices will be converted to game objects with polygon collider.
			var vertices = child as IObjectWithVertices;
			if (vertices != null)
			{
				// first we add the component to our create game object
				var collider = childGameObject.AddComponent<PolygonCollider2D>();
				// then we calculate the overall bounds of this polygon
				bounds = ArticyUtility.GetBounds(vertices.Vertices);

				// and then we convert the articy vertices to unity vertices (flipped y axis and different pixel scaling)
				var colliderPoints = new Vector2[vertices.Vertices.Count];
				int i = 0;
				foreach (var vec in vertices.Vertices)
				{
					var v = new Vector2(vec.x / aPixelsToUnits, (bounds.yMax - vec.y) / aPixelsToUnits);
					colliderPoints[i++] = v;
				}
				// after we have converted the points, we can store them
				collider.SetPath(0, colliderPoints);
	
				// we need the bounds again, if this object does not only contain vertices, but also represents an image
				bounds = ArticyUtility.GetBounds(collider.points);

				// only if this is a clickable zone, we initially enable the collider.
				collider.enabled = hasZoneScript;

				boundsSet = true;
			}
			#endregion

			#region Create and setup Spriter renderer for location images

			// our location images, have a reference to an articy reference
			// we use that to fill a sprite renderer with the image
			var image = child as ILocationImage;
			if (image != null)
			{
				SpriteRenderer spriteRenderer = null;
				// use reflection to get the Image
				var asset = GetProperty<ArticyObject>(child, "ImageAsset");
				if (asset != null)
				{
					// make sure it is an actual image asset
					var imageAsset = asset as IAsset;
					if (imageAsset != null && imageAsset.Category == AssetCategory.Image)
					{
						// add a new sprite renderer to our game object
						spriteRenderer = childGameObject.AddComponent<SpriteRenderer>();
						
						// load the underlying texture
						var tex = imageAsset.LoadAsset<Texture2D>();
						// we also need the cliprect, as the image might be cropped
						var clipRect = GetProperty<Rect>(child, "ClipRect");
						// convert the clip rect to a sprite source rect
						var sourceRect = ArticyUtility.ConvertToSpriteSourceRect(tex, clipRect);
						// the sorting order is reversed in regards to our zindex, 
						spriteRenderer.sortingOrder = 1000 - aZindex;
						// and finally convert the texture to a sprite using our calculated source rect
						spriteRenderer.sprite = Sprite.Create(tex, sourceRect, new Vector2());
						if (!boundsSet)
						{
							bounds.width = tex.width / (float)aPixelsToUnits;
							bounds.height = tex.height / (float)aPixelsToUnits;
						}
					}
				}
			}
			#endregion

			#region Adjust the objects transformations

			var trans = childGameObject.transform;
			var transform = child as IObjectWithTransformation;
			if (transform != null)
			{
				// change the position 
				trans.localPosition = new Vector3(transform.Transform.Translation.x / aPixelsToUnits,
					aOverallBounds.height - bounds.height - (transform.Transform.Translation.y / aPixelsToUnits));
				// Todo: scale and rotating
			}
			#endregion

			aZindex++;

			// some objects inside a location, can have children themselves, so we just recursively call GenerateChildren again
			GenerateChildren(childGameObject.transform, child, aPixelsToUnits, aOverallBounds, aAttachBehaviours, ref aZindex);
		}
	}
}
