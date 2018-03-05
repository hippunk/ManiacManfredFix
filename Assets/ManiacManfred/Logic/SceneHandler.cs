using System.Collections.Generic;
using Articy.ManiacManfred;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The main object for the game logic and especially the implementation of our flow player callbacks
/// This also makes sure to switch scenes if requested and show or hide the dialog UI.
/// </summary>
public class SceneHandler : MonoBehaviour, IArticyFlowPlayerCallbacks, IScriptMethodProvider
{
	#region fields and properties
	[Header("Setup")]
	// a reference to the scene transition effect, used when we move from the current scene to another one
	public ScreenTransitionEffect transition;
	// a prefab to spawn a close button in a branching dialog that effectively closes the dialog and returns to the game
	public GameObject closePrefab;
	// a prefab for a single option inside a branching dialog
	public GameObject branchPrefab;

	[Header("UI")] 
	// a reference to the whole dialog ui, so we can hide or show it depending on if we have currently a dialog to show
	public GameObject dialogWidget;
	// a reference to the image control, this will hold the current speaking entity image
	public Image speakerImage;
	// a reference to the Localization Caretaker that is responsible for updating the spoken text
	public ArticyLocaCaretaker dialogText;
	// a reference to the panel that we use to insert all branch options into, the panel will automatically align them vertically
	public RectTransform branchLayoutPanel;
	// a reference to the text control, which will contain the current moral value of the player character
	public Text moralTextLabel;

	[Header("Debug")]
	// debug flag that we can use to show or hide branches that are evaluated to false
	public bool showFalseBranches;

	// reference to the articy flow player
	private ArticyFlowPlayer flowPlayer;
	// a flag to check if we are currently showing the dialog ui interface
	private bool dialogShown = false;
	// a reference to the inventory system
	private Inventory inventory;

	// the scene handler is built as a singleton, and also to be kept across scene switches
	private static SceneHandler instance;
	private MouseCursorResources cursorResources;

	/// <summary>
	/// Reference to the cursor resources
	/// </summary>
	public MouseCursorResources CursorResources
	{
		get { return cursorResources; }
	}

	#endregion

	#region initialization

	void Awake()
	{
		// we are forming a Singleton that we can keep across multiple scenes
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			DontDestroyOnLoad(gameObject);
			Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}

		cursorResources = GetComponent<MouseCursorResources>();
		inventory = FindObjectOfType<Inventory>();
		flowPlayer = GetComponent<ArticyFlowPlayer>();

		// set the default method provider for script methods, so that we needn't pass it as a parameter when calling script methods manually.
		// look into region "script methods" at the end of this class for more information.
		ArticyDatabase.DefaultMethodProvider = this;

		// disable the dialog widget, just to be safe
		dialogWidget.SetActive(false);

		// normally the Localization Caretaker works with UnityEngine.UI.Text controls
		// but in this case we want it to update our custom animated text element. 
		// in that case we have to give the caretaker a method that does the text assignment for him, while the caretaker still listenes wor language changes.
		dialogText.localizedTextAssignmentMethod.AddListener(AssignDialogueText);
		
		// and if we are loaded we trigger a transition effect.
		transition.TransitionIn();
	}

	#endregion

	/// <summary>
	/// This method is used to trigger different behaviours depending on the result or outcome of an interaction by the user.
	/// </summary>
	public void ContinueFlow(IArticyObject aObject)
	{
		// if the object is a location, we load a new level
		if (aObject is ILocation)
		{
			var loc = aObject as ArticyObject;
			// we want to leave, so we trigger our transition effect
			transition.TransitionOut();
			// the new scene name is the same as the technical name of the supplied location
			// we start a coroutine because we want to delay the loading of the level a bit for the transition to finish
			StartCoroutine(LoadNextLevel(loc.TechnicalName));
		}
		else if(aObject is IEntity)
		{
			// if the object is an item, that usually means that the item is given to the player.
			var item = aObject as Item;
			if(aObject is Item)
			{
				ArticyDatabase.DefaultGlobalVariables.SetVariableByString(item.Template.VariableBinding.VariableName.RawScript, true);
			}
		}
		else if (aObject != null)
		{
			// if we are about to open a new dialog, we better stop the user from using any items
			inventory.StopUsingItem();
			// we enable the dialog ui
			dialogShown = true;
			dialogWidget.SetActive(dialogShown);
			// and we assign the object as the start node
			flowPlayer.StartOn = aObject;
		}
	}

	/// <summary>
	/// Coroutine to delay the level loading to let the transition effect finish before we actualy load the scene
	/// </summary>
	private IEnumerator LoadNextLevel(string aLevelName)
	{
		yield return new WaitForSeconds(1.0f);
		SceneManager.LoadScene(aLevelName);
		UpdateMoraleUI();
	}
	
	/// <summary>
	/// This is one of the important callbacks from the ArticyFlowPlayer, and will notify us about pausing on any flow object.
	/// It will make sure that the paused object is displayed in our dialog ui, by extracting its text, potential speaker etc.
	/// </summary>
	public void OnFlowPlayerPaused(IFlowObject aObject)
	{
		// if the flow player paused on a dialog, we immediately continue, usually getting to the first dialogue fragment inside the dialogue
		// makes it more convenient to set the startOn to a dialogue
		if (aObject is IDialogue)
		{
			flowPlayer.Play();
			return;
		}

		// here we extract any Text from the paused object.
		// In most cases this is the spoken text of a dialogue fragment
		var modelWithText = aObject as IObjectWithLocalizableText;
		if (modelWithText != null)
		{
			// but we are not taking Text directly and assigning it to the ui control
			// we take the LocaKey and assign it to our caretaker. The caretaker will localize it
			// using the current language and will set it to our text control. The caretaker will also make sure that the text
			// is updated and localized again if we change the language while it is currently displayed to the screen.
			dialogText.LocaKey = modelWithText.LocaKey_Text;
		}

		// finally we update the speaker image in our ui, by checking if the paused object has a speaker
		var dlgSpeaker = aObject as IObjectWithSpeaker;
		if (dlgSpeaker != null)
		{
			// getting the speaker object
			var speaker = dlgSpeaker.Speaker;
			if (speaker != null)
			{
				// checking if the speaker itself has a preview image
				var speakerAsset = ((speaker as IObjectWithPreviewImage).PreviewImage.Asset as Asset);
				if (speakerAsset != null)
				{
					// and finally loading the preview image as a sprite and assign it to our image control
					speakerImage.sprite = speakerAsset.LoadAssetAsSprite();
				}
			}
		}

		// the dialog choice contains a script that is called when this dialog option was taken
		// modifying the moral outcome of the play through
		var dialogChoice = aObject as DialogChoice;
		if (dialogChoice != null)
		{
			dialogChoice.Template.DialogChoice.MoraleChange.CallScript();
		}

	}

	/// <summary>
	/// This is the other important callback from the ArticyFlowPlayer, and is called everytime the flow player has new branches
	/// for us. We use that to update the list of buttons in our dialog interface.
	/// </summary>
	public void OnBranchesUpdated(IList<Branch> aBranches)
	{
		// This will update our little morale text label on the top of the screen with the current
		// moral of the player character
		UpdateMoraleUI();

		// first we clear all the old branch options from the panel
		foreach (Transform child in branchLayoutPanel)
			Destroy(child.gameObject);

		// if none of the branches point to a dialoge fragment, we consider this dialog to be finished.
		bool dialogIsFinished = true;
		foreach (var branch in aBranches)
		{
			if (branch.Target is IDialogueFragment)
				dialogIsFinished = false;
		}

		// so if the dialog continues, we create a new list of buttons
		// otherwise we create the close button
		if(!dialogIsFinished)
		{
			// for all of the branches
			foreach (var branch in aBranches)
			{
				// we filter those out that are not valid(unless we want it)
				if (!branch.IsValid && !showFalseBranches) continue;

				// and we create a new branch button
				var btn = Instantiate(branchPrefab);
				// insert it into our ui panel
				var rect = btn.GetComponent<RectTransform>();
				rect.SetParent(branchLayoutPanel, false);
				// and set it up, so it knows about the flow player and the branch it represents
				btn.GetComponent<BranchChoice>().AssignBranch(flowPlayer, branch);
			}
		}
		else
		{
			// create the close button
			var btn = Instantiate(closePrefab);
			// insert it into our ui panel
			var rect = btn.GetComponent<RectTransform>();
			rect.SetParent(branchLayoutPanel, false);
			// and register the click event
			var btnComp = btn.GetComponent<Button>();
			btnComp.onClick.AddListener(CloseDialog);
		}
	}

	/// <summary>
	/// This is the click event method for our close button, it will cleanup the dialog ui and will handle any post dialog logic 
	/// </summary>
	private void CloseDialog()
	{
		// The last object is shown, and we are about to close the dialog. But the last paused object
		// might have an outputpin that contains important scripts, and we have to call that.
		flowPlayer.FinishCurrentPausedObject();

		// we hide our dialog ui, as we are finished with it for now
		dialogShown = false;
		dialogWidget.SetActive(dialogShown);

		// it is possible that the last dialog fragment shown, was a dialog choice and contains a location change request in its template
		var choice = flowPlayer.PausedOn as DialogChoice;
		if (choice != null)
		{
			// so we extract it, and continue with it
			ContinueFlow(choice.Template.DialogChoice.LocationChange);
		}
	}

	/// <summary>
	/// update the moral ui
	/// </summary>
	private void UpdateMoraleUI()
	{
		// the moral is found in the template of the Manfred entity, the of the Player_Character class
		var moralValue = ArticyDatabase.GetObject<Player_Character>("Chr_Manfred").Template.Morale.MoraleValue;
		// assign the moral value
		moralTextLabel.text = moralValue.ToString();

		// and to make it a little nicer, we change the color according to the moral value
		if (moralValue == 0) moralTextLabel.color = Color.yellow;
		if (moralValue < 0) moralTextLabel.color = Color.red;
		if (moralValue > 0) moralTextLabel.color = Color.green;
	}


	/// <summary>
	/// As mentioned in the Awake method. This method is given to the Caretaker that allows him to update something different with the localized text, in this case
	/// our animated text.
	/// </summary>
	private void AssignDialogueText(Component aTargetComponent, string aLocalizedText)
	{
		var text = aTargetComponent as TextAnimator;
		text.ChangeText(aLocalizedText);
	}

	#region script methods

	/// <summary>
	/// This property is important for the proper support of script methods, check the help to learn more about its use.
	/// </summary>
	public bool IsCalledInForecast { get; set; }

	/// <summary>
	/// this is a script method the designer created in the articy project, and the plugin generated a c# method for us.
	/// The intent of this method is to just completely restart the game, and is used on the final screen of the game.
	/// </summary>
	public void restart()
	{
		// its important to only call the logic outside of any forecastings
		if (!IsCalledInForecast)
		{
			// because we want to restart the game, we reset the variables
			ArticyDatabase.DefaultGlobalVariables.ResetVariables();

			// we have to reset the moral value
			ArticyDatabase.GetObject<Player_Character>("Chr_Manfred").Template.Morale.MoraleValue = 0;

			// then we check every location we have in the database to find the one that is flagged as the start location
			var locations = ArticyDatabase.GetAllOfType<LocationSettings>();
			foreach(var loc in locations)
			{
				if(loc.Template.LocationSettings.IsStartLocation)
				{
					// once found, we just continue with that.
					ContinueFlow(loc);
					break;
				}
			}
		}
	}
	#endregion
}
