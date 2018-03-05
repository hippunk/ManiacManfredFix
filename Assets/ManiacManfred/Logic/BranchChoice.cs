using Articy.ManiacManfred;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

// This is the UI control for a single option in a multiple choice dialogue
// It will contain a preview text of what to expect from this dialog option; updates the icon if the dialog option involves an item
// and most importantly will trigger the ArticyFlowPlayer to continue once selected.
public class BranchChoice : MonoBehaviour
{
	[Header("Setup UI")]
	// a reference to the text control that will be the target of the preview text for our dialog option
	public Text dialogText;
	// a reference to the loca caretaker that will take care of the localization
	public ArticyLocaCaretaker dialogTextCaretaker;
	// a reference to the image control that will contain either a normal dialog line icon, or a thumbnail of the used item
	public Image typeImage;

	// a reference to the ArticyFlowPlayer Branch this option represents, used once the user selects this option.
	private Branch mBranch;
	// a reference to the ArticyFlowPlayer used in the current dialogue.
	private ArticyFlowPlayer mProcessor;

	// this is called when building a list of branches for the ui and giving it all necessary informations.
	public void AssignBranch(ArticyFlowPlayer aProcessor, Branch aBranch, string aOverrideText = null)
	{
		mBranch = aBranch;
		mProcessor = aProcessor;
		// this is a little debug help, if the branch would not be valid, we make this button red
		// usually we don't allow invalid branches and therefore never use this
		dialogText.color = aBranch.IsValid ? Color.black : Color.red;
		
		var target = aBranch.Target;
		dialogText.text = "";

		// the caller could set a text that he wants to use, otherwise we build it using the information we find inside the branch
		if (aOverrideText != null)
		{
			dialogTextCaretaker.LocaKey = aOverrideText;
		}
		else
		{
			// most importantly we want to use the MenuText of a DialogueFragment as the preview text
			var localizedObj = target as IObjectWithLocalizableMenuText;
			if (localizedObj != null)
			{
				dialogTextCaretaker.LocaKey = localizedObj.LocaKey_MenuText;
			}
			else
			{
				var obj = target as IObjectWithMenuText;

				if (obj != null)
				{
					dialogText.text = obj.MenuText;
				}
			}

			// if for some reason we don't find one, we show at least 3 dots, useful when its not really a choice but more a "continue talking" branch
			if (dialogText.text == "")
				dialogTextCaretaker.LocaKey = "...";
		}

		// the dialogchoice contains additional information about a dialog option
		var dialogChoice = target as DialogChoice;
		if(dialogChoice != null)
		{
			// if we have a dialog choice, that uses an item, we show that items sprite and the items name
			if(dialogChoice.Template.DialogChoice.RequiredItem != null)
			{
				var item = dialogChoice.Template.DialogChoice.RequiredItem as Item;

				typeImage.sprite = item.PreviewImage.Asset.LoadAssetAsSprite();
				dialogTextCaretaker.LocaKey = item.LocaKey_DisplayName;
            }
		}
	}

	// the dialog option, basically a button, calls this method when it is clicked by the user
	public void OnBranchSelected()
	{
		mProcessor.Play(mBranch);
	}
}
