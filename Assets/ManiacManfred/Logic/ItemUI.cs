using Articy.ManiacManfred;
using Articy.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is attached to every item control shown in the UI in the inventory bar at the bottom
/// it will take care of mouse input and especially of combining it with a currently held item
/// </summary>
public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[Header("Setup")]
	// a reference to the image control for the item icon
	public Image icon;
	// a reference to the image control for the selection border and background
	public Image hoverBackground;
	// an articy reference storing our actual Item
	private ArticyReference itemRef;
	// a reference to the inventory
	private Inventory inventorySystem;
	// a reference to the scene handler, necessary when a combination would trigger a new dialogue for example
	private SceneHandler sceneHandler;

	/// <summary>
	/// Quick access to the item that this ui control represents
	/// </summary>
	public Item RepresentedItem
	{
		get { return itemRef.GetObject<Item>(); }
	}

	/// <summary>
	/// Called when the control is created and it basically is setup to represent the item in the inventory bar
	/// </summary>
	public void AssignItem(Item aNewItem, Inventory aInventorySystem)
	{
		itemRef = GetComponent<ArticyReference>();
		sceneHandler = FindObjectOfType<SceneHandler>();
		inventorySystem = aInventorySystem;
		itemRef.SetObject(aNewItem);

		if (itemRef.IsValid)
		{
			// we use the previewimage to get the icon of the item
			icon.sprite = aNewItem.PreviewImage.Asset.LoadAssetAsSprite();
			// and we update the object name, makes it easier when debugging the inventory in unity
			gameObject.name = string.Format("ItemUI_{0}", aNewItem.TechnicalName);
		}
		// making sure the hover background is hidden
		hoverBackground.gameObject.SetActive(false);
	}

	// when the cursor moves into our item, we hover it 
	public void OnPointerEnter(PointerEventData eventData)
	{
		hoverBackground.gameObject.SetActive(true);
		sceneHandler.CursorResources.SetCursor(MouseCursor.Point);
	}

	// leaving our item, cleans up any changes
	public void OnPointerExit(PointerEventData eventData)
	{
		hoverBackground.gameObject.SetActive(false);
		if (inventorySystem.IsUsingItem)
		{
			sceneHandler.CursorResources.SetCursor(MouseCursor.Take);
		}
		else
		{
			sceneHandler.CursorResources.ClearCursor();
		}
	}

	// clicking the item in the inventory bar, needs to handle the case where the user wants to combine a used item or where he wants to start to use an item
	public void OnPointerClick(PointerEventData eventData)
	{
		if (!inventorySystem.IsUsingItem)
		{
			// if we weren't using an item already, we now want to use the clicked item
			inventorySystem.StartUsingItem(this);
		}
		else
		{
			// lets see if we can combine items

			// first we get the item the user is currently holding
			var heldItem = inventorySystem.CurrentlyUsedItem;

			// and then we ask its template if the clicked item is a valid combination
			if (heldItem.Template.ItemCombination.ValidCombination == RepresentedItem)
			{
				// combination is allowed and usually ends in a new item
				var result = heldItem.Template.ItemCombination.CombinationResult;
				if (result != null)
				{
					var item = result as Item;
					// now we rely on the inventory system to work properly: We set the global variable, representing the new item that resulted from combining
					// the two items, to true. Effectively this will get the listener called inside the Inventory and that will take care of adding the new item
					ArticyDatabase.DefaultGlobalVariables.SetVariableByString(item.Template.VariableBinding.VariableName.RawScript, true);

					// both old items must now be removed, because we consumed them to create the new item
					inventorySystem.RemoveItem(heldItem);
					inventorySystem.RemoveItem(RepresentedItem);
					// note: We could call SetVariableByString here aswell, but i wanted to leave the variables set to true.
					// this seems counter intuitive, but some trigger checking if you have those consumed items would suddenly be true again
					// ending up giving us the item again. There is no harm in leaving the set to true, and just removing them from the inventory manually

					// finally, if we have a potential new dialog, we tell the scene handler about it
					sceneHandler.ContinueFlow(heldItem.Template.ItemCombination.LinkIfSuccess);
				}
			}
			else
			{
				// sometimes trying to combine items trigger a "That won't work" dialogue.
				sceneHandler.ContinueFlow(heldItem.Template.ItemCombination.LinkIfFailure);
			}
			// no matter, we stop using the item
			inventorySystem.StopUsingItem();
		}
	}
}
