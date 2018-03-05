using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;

/// <summary>
/// This is a settings behaviour for our Location Creator. It stores some settings used the creator 
/// </summary>
public class ArticyLocation : MonoBehaviour
{
	[ArticyTypeConstraint(typeof(ILocation))]
	public ArticyRef location;
	public bool fitToCanvas = true;
	public int pixelsToUnits = 100;
}


