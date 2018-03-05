using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This behaviour will feed a unity Text control with a localized text, but will wait a bit after each character
/// this creates the elusion of animated text, similar to a typewriter. 
/// </summary>
public class TextAnimator: MonoBehaviour
{
	// should we pause after a sentence?
	public bool pauseAfterSentence = true;

	// reference to our text label
	private Text uiTextLabel;
	// the current used text(already localized)
	private string currentAnimatedText;

	// how long will takes to go from one character to the next?
	private float timeForSingleCharacter = 0.05f;
	// how long will it take from the end of one sentence to start the next
	private float timeForSentencePause = 0.50f;

	void OnEnable()
	{
		uiTextLabel = GetComponent<Text>();
	}

	// this method resets the current animated text and starts a new one
	public void ChangeText(string aNewText)
	{
		StopCoroutine("TextAnimationTick");
		currentAnimatedText = aNewText;
        StartCoroutine("TextAnimationTick");
	}

	/// <summary>
	/// This will check if the supplied character is one that identifies a finished sentence
	/// </summary>
	private bool IsSentencePause(char aCh)
	{
		return aCh == '.' || aCh == '?' || aCh == '!' || aCh == ';';
    }

	// the coroutine that will feed the text control with one character at a time
	private IEnumerator TextAnimationTick()
	{
		int i = 0;
		uiTextLabel.text = "";
        while (i < currentAnimatedText.Length)
		{
			var ch = currentAnimatedText[i++];
            uiTextLabel.text += ch;

			if(pauseAfterSentence && IsSentencePause(ch))
				yield return new WaitForSeconds(timeForSentencePause);
			else
				yield return new WaitForSeconds(timeForSingleCharacter);
		}
	}
}