using System;
using System.Collections.Generic;
using Articy.ManiacManfred;
using Articy.Unity;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The inventory class is the manager of everything item related. 
/// It will build a list of all items available; will manage the global variables that represent the items; will update the list of the items on the lower part of the screen
/// and will take care of the mouse holding the item
/// </summary>
public class Inventory : MonoBehaviour
{
	[Header("Setup")]
	// the prefab of a single item in our inventory bar at the bottom of the screen
	public GameObject itemContainerPrefab;
	// the parent transform that will receive any new item
	public RectTransform itemListPanel;
	// the image object that will be moved around the screen once the user holds an item with the mouse
	public Image mouseDragImage;
	// the main canvas
	public Canvas uiCanvas;
	
	[Header("Settings")] 
	// the image dragged by the mouse needs a small offset otherwise the mouse would always hover over the dragged image
	public Vector2 cursorToDragImageOffset = new Vector2(10, -10);

	// here we keep a simple map of item and their variable name for easier use
	private Dictionary<string, Item> variableItemMap = new Dictionary<string, Item>();
	// a reference to the item the user is currently holding, if this is null the user is not holding any item
	private Item currentlyUsedItem;
	// a reference to the scene handler
	private SceneHandler sceneHandler;

	/// <summary>
	/// true if the user is currently holding an item
	/// </summary>
	public bool IsUsingItem
	{
		get { return currentlyUsedItem != null; }
	}

	/// <summary>
	/// gets the currently held item, or null if none is held
	/// </summary>
	public Item CurrentlyUsedItem
	{
		get { return currentlyUsedItem; }
	}

	void Awake()
	{
		sceneHandler = FindObjectOfType<SceneHandler>();
    }

	// here we initialize the inventory system
	void Start()
	{
		// This makes use of the fact that every Item entity knows its variable name and every item has a global variable inside the "Inventory" variable set.
		// so we register a listener to the global variables to inform us about any changes of variables inside the Inventory variable set.
		ArticyDatabase.DefaultGlobalVariables.Notifications.AddListener("Inventory.*", OnVarChanged);

		// as mentioned before, every Item knows its variable name, to make easier use in our listener method
		// we create a dictionary linking variable name (eg. "Inventory.crowbar") to the actual Item reference (eg. "Itm_Crowbar")
		// so first we get all items
		var allItems = ArticyDatabase.GetAllOfType<Item>();
		variableItemMap.Clear();

		// then we loop over each item
		foreach (var item in allItems)
		{
			try
			{
				// access its variable binding feature, and take the variable name script in pure text form.
				// and save both in our dictionary
				variableItemMap.Add(item.Template.VariableBinding.VariableName.RawScript, item);
			}
			catch(Exception aEx)
			{
				Debug.LogException(aEx);
			}
			
		}

		// just making sure that the mouse drag image is disabled
		mouseDragImage.gameObject.SetActive(false);

		// because in editor the user could modify the global variables before starting it, and it would be nice debug feature to "cheat" us items
		// we check if any of those items are already checked.
		// now we check if any items are already aquired
		foreach (var pair in ArticyDatabase.DefaultGlobalVariables.Variables)
		{
			// if the variable is inside the Inventory variable set
			if(pair.Key.Contains("Inventory."))
			{
				// and the variable is actually true
				if ((bool)pair.Value)
				{
					// we call the same method that is called when the variable would be set at runtime
					OnVarChanged(pair.Key, true);
				}
			}
		}
	}

	// this method is called everytime an item variable changes its value
	// so if for example "Inventory.crowbar" is set to true, this method gets called
	private void OnVarChanged(string aVariable, object aValue)
	{
		// first we figure out, which variable was changed
		Item changedItem;
		var itemAdded = (bool)aValue;

		// now our previously created dictionary becomes useful, we just have to pass the aVariable name into our dictionary
		// and if it finds something, we get the associated item reference with it.
		if (variableItemMap.TryGetValue(aVariable, out changedItem))
		{
			// now depending on the value of the variable we either added it or removed it from the inventory
			if (itemAdded)
			{
				AddItem(changedItem);
			}
			else
			{
				RemoveItem(changedItem);
			}
		}
	}

	// called when the item is added to the inventory
	private void AddItem(Item aNewItem)
	{
		// we create a new UI control, using our prefab
		var itemBtn = Instantiate(itemContainerPrefab);
		// giving it a reference to the item it represents
		itemBtn.GetComponent<ItemUI>().AssignItem(aNewItem, this);
		// and adding it to our item list in the ui
		itemBtn.GetComponent<RectTransform>().SetParent(itemListPanel);
	}

	// called when an item is removed from the inventory
	public void RemoveItem(Item aOldItem)
	{
		// we get all controls inside our inventory panel
		var itemUis = itemListPanel.GetComponentsInChildren<ItemUI>();
		foreach (var itemUi in itemUis)
		{
			// and if any of the referenced items matches the removed item
			if(itemUi.RepresentedItem == aOldItem)
			{
				// we destroy the control
				Destroy(itemUi.gameObject);
			}
		}
	}

	// this is called when the user clicks one of the items in the inventory panel and therefore start using it aka attaching it to the mouse cursor
	public void StartUsingItem(ItemUI aItem)
	{
		// we keep a reference, necessary when the user wants to combine it and clicks something else
		currentlyUsedItem = aItem.RepresentedItem;
		// our mouse drag image object is now set to active
		mouseDragImage.gameObject.SetActive(true);
		// and also its sprite is changed to the one of our now used item
		mouseDragImage.sprite = aItem.icon.sprite;
		// finally, because it looks nicer the cursor is changed to the take cursor
		sceneHandler.CursorResources.SetCursor(MouseCursor.Take);
	}

	// called when the user right clicks or used the item (no matter if correctly or not)
	public void StopUsingItem()
	{
		currentlyUsedItem = null;
		// we hide the drag image again
		mouseDragImage.gameObject.SetActive(false);
		mouseDragImage.sprite = null;
		// and reset back to the normal mouse cursor
		sceneHandler.CursorResources.ClearCursor();
	}

	// if we are using an item, we need to update the position of the dragged item, obviously we do that inside update
	void Update()
	{
		if (IsUsingItem)
		{
			Vector2 pos;
			// this will take care of any transformations from screen to world space
			RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, Input.mousePosition, uiCanvas.worldCamera, out pos);
			// we only need to adjust the position slightly by the offset
			mouseDragImage.GetComponent<RectTransform>().position = uiCanvas.transform.TransformPoint(pos + cursorToDragImageOffset);
			// and finally, if the user presses the right mouse button, we cancel the item using
			if (Input.GetMouseButtonUp(1))
			{
				StopUsingItem();
			}
		}
	}
}
