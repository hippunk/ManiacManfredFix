using Articy.Unity;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple script for a Language Toggle button If you click it it will change its icon depending on the used language
/// and change the language in the underlying localization manager
/// </summary>
public class LanguageButton : MonoBehaviour
{
	// the sprite for the "de" language
	public Sprite deFlagSprite;
	// the sprite for the "en" language
	public Sprite enFlagSprite;

	// the image component that will be the target of our flags
	private Image buttonImage;

	// initialize the button for the first start
	void Start()
	{
		buttonImage = GetComponent<Image>();
		UpdateFlagSprite();
	}
	// make sure that the sprite on the button is the one of the current language
	private void UpdateFlagSprite()
	{
		buttonImage.sprite = GetFlagForLanguage(ArticyDatabase.Localization.Language);
	}

	// Returns the correct sprite for the supplied language
	private Sprite GetFlagForLanguage(string aLanguage)
	{
		if (aLanguage == "en") return enFlagSprite;
		if (aLanguage == "de") return deFlagSprite;
		return null;
	}

	// event handler message for the click event of the button
	public void OnToggleLanguage()
	{
		ArticyDatabase.Localization.Language = ArticyDatabase.Localization.Language == "en" ? "de" : "en";
		UpdateFlagSprite();
	}
}
