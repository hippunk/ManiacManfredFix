using Articy.ManiacManfred;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// This is the script on every clickable zone in every scene. A zone can have different logic attached to it, 
/// depending on its template like starting dialogs, using an item with the zone, changing locations(scenes) etc.
/// </summary>
[AttachBehaviourByTemplate("Conditional_ZoneTemplate")]
public class ClickableZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	/// <summary>
	/// a reference to the actual articy object of the zone. Otherwise we would have to call GetComponent and ask the ArticyReference everytime.
	/// </summary>
	private Conditional_Zone articyZone;
	/// <summary>
	/// a reference to the scene handler. We need it if this zone could start a new dialog or change the scene
	/// </summary>
	private SceneHandler sceneHandler;
	/// <summary>
	/// a reference to the inventory manager, necessary when this zone could interact with items in any way.
	/// </summary>
	private Inventory inventory;

	// Here we just grab references to our frequently used objects
	void Start()
	{
		articyZone = (Conditional_Zone)GetComponent<ArticyReference>().reference;
		sceneHandler = FindObjectOfType<SceneHandler>();
		inventory = FindObjectOfType<Inventory>();
	}

	// Update contains a safety measure to work around zones that overlap each other
	void Update()
	{
		Debug.Assert(articyZone != null, "Articy zone is empty");
		Debug.Assert(articyZone.Template != null, "Articy zone template is empty");
		Debug.Assert(articyZone.Template.ZoneCondition != null, "Articy zone template condition is empty");

		// this will make sure that only zones that can be interacted with are valid event system targets
		// otherwise overlapping zones would not properly work.
		// we basically check its condition, and if its false, we disable the collider
		var collider = GetComponent<PolygonCollider2D>();
		bool isValid = articyZone.Template.ZoneCondition.ClickCondition.CallScript();
        if (!isValid)
		{
			// To correctly disable the collider, we have to check if this zone doesn't have a "condition does not match" target
 			// if it does, we still need to be able to interact with it, and can't safely disable the collider on the result of the expression alone. 
			collider.enabled = articyZone.Template.ZoneCondition.IfConditionFalse != null;
		}
		else
			collider.enabled = true;
	}

	// This method looks very long but its just a bunch of checks selecting the final cursor texture when the user moves the cursor into this zone.
	public void OnPointerEnter(PointerEventData eventData)
	{
		// using the condition we can use the result to check which cursor we have to show
		var result = articyZone.Template.ZoneCondition.ClickCondition.CallScript();

		MouseCursor cursor;
		// if the user is currently holding an item, we change the cursor to "Point" which will give the user feedback that he could try to interact with this zone
		if (inventory.IsUsingItem)
		{
			cursor = MouseCursor.Point;
		}
		else
		{
			// if the condition is true, it means we are allowed to interact with the zone
			if (result)
			{
				// the designer could have set a very specific cursor or just set it to auto 
				cursor = articyZone.Template.ZoneCondition.CursorIfConditionTrue;

				// auto cursor is special because it doesn't have a matching image, its basically the designer telling us to use a cursor that makes sense depending on the "condition matches" object
				if (cursor == MouseCursor.Auto)
				{
					// if clicking this would trigger a new dialogue, we show a speak icon
					if (articyZone.Template.ZoneCondition.IfConditionTrue is IDialogue)
					{
						cursor = MouseCursor.Speak;
					}

					// if clicking this would trigger a new dialogue from a dialogue fragment, an indication this something like a monologue, we show the inspect icon
					if (articyZone.Template.ZoneCondition.IfConditionTrue is IDialogueFragment)
					{
						cursor = MouseCursor.Inspect;
					}

					// if clicking this would trigger a scene transition, we show the walk icon
					if (articyZone.Template.ZoneCondition.IfConditionTrue is ILocation)
					{
						cursor = MouseCursor.Walk;
					}
				}
			}
			else // in this case, the condition was false, and we where not allowed to interact with this zone unless it has a ConditionDoesNotMatch object
			{
				// the designer could have set a very specific cursor or just set it to auto 
				cursor = articyZone.Template.ZoneCondition.CursorIfConditionFalse;

				// auto cursor is special because it doesn't have a matching image, its basically the designer telling us to use a cursor that makes sense depending on the "condition matches" object
				if (cursor == MouseCursor.Auto)
				{
					// if clicking this would trigger a scene transition, we show the inspect icon
					if (articyZone.Template.ZoneCondition.IfConditionFalse is ILocation)
					{
						cursor = MouseCursor.Inspect;
					}

					// if clicking this would trigger a new dialogue, we show a inspect icon
					if (articyZone.Template.ZoneCondition.IfConditionFalse is IDialogue)
					{
						cursor = MouseCursor.Inspect;
					}

					// if clicking this would trigger a new dialogue from a dialogue fragment we show the inspect icon
					if (articyZone.Template.ZoneCondition.IfConditionFalse is IDialogueFragment)
					{
						cursor = MouseCursor.Inspect;
					}
				}

			}
		}

		// once the cursor is chosen, and auto has been resolved we actually set it
		sceneHandler.CursorResources.SetCursor(cursor);
	}

	// when the user moves the cursor out of this zone, we have to cleanup any cursor changes we did 
	public void OnPointerExit(PointerEventData eventData)
	{
		if (inventory.IsUsingItem)
		{
			// if we are still using an item, we go back to the take cursor
			sceneHandler.CursorResources.SetCursor(MouseCursor.Take);
		}
		else
		{
			// otherwise we just clear the cursor
			sceneHandler.CursorResources.ClearCursor();
		}
	}

	// when the user actually clicked the zone, the real fun begins.
	// depending on currently held item or not; the underlying condition and its outcome we could end up with a new dialogue or change of scene.
	public void OnPointerClick(PointerEventData eventData)
	{
		// What follows are a bunch of checks, but most of the times we end up with a specific object that triggers something in our logic.
		// this could be a new dialogue telling us that unlocking the door worked; a new dialogue fragment telling us that we can't pickup what the zone represents yet; or a scene change
		ArticyObject outcomeObject = null;

		// using an item means we want to combine the currently held item with the zone, when using a key(item) on a door(zone) for example
		// otherwise we just interact with the zone directly
		if (inventory.IsUsingItem)
		{
			// we check if the used item can interact with our zone
			if(articyZone.Template.ZoneCondition.ItemToInteractWith == inventory.CurrentlyUsedItem)
			{
				// while item and zone seem to match together, we have a condition to check aswell
				var itemInteractResult = articyZone.Template.ZoneCondition.InteractionCondition.CallScript();
				if(itemInteractResult)
				{
					// make sure we don't keep the item in our hand
					inventory.StopUsingItem();
					// so we used the correct item, and we were allowed to interact with the zone
					// now we trigger the valid interaction script ...
					articyZone.Template.ZoneCondition.InstructionIfItemValid.CallScript();
					// and tell the scene manager about a potential target outcome object (new dialogue, new scene etc.)
					outcomeObject = articyZone.Template.ZoneCondition.LinkIfItemValid;
				}
			}
			else
			{
				// make sure we don't keep the item in our hand
				inventory.StopUsingItem();
				// the item is not the correct one, so we trigger the invalid dialog option
				outcomeObject = articyZone.Template.ZoneCondition.LinkIfItemInvalid;
			}
		}
		else
		{
			// lets check if we can click, and then we follow depending on the outcome
			var result = articyZone.Template.ZoneCondition.ClickCondition.CallScript();
			if (result)
			{
				// we were allowed to click it, so we call its click instruction
				articyZone.Template.ZoneCondition.OnClickInstruction.CallScript(sceneHandler);
				// and get its target outcome object
				outcomeObject = articyZone.Template.ZoneCondition.IfConditionTrue;
			}
			else
			{
				// so the condition was invalid, maybe we have an object
				outcomeObject = articyZone.Template.ZoneCondition.IfConditionFalse;
			}
		}

		// if we have an outcomeObject we pass it to the scene handler
		if(outcomeObject != null)
			sceneHandler.ContinueFlow(outcomeObject);
	}
}
