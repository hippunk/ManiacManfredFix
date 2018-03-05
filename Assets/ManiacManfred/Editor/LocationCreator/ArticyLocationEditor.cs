using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// This is the custom editor for the ArticyLocation settings behaviour
/// That will contain the button to actually start the scene population process.
/// </summary>
[CustomEditor(typeof(ArticyLocation))]
class ArticyLocationEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		// we only add a button and maybe some help labels, the rest can be default
		DrawDefaultInspector();

		var locationContainer = target as ArticyLocation;
		var targetGameObject = locationContainer.gameObject;
		var mdl = locationContainer.location.GetObject() as ILocation;

		// without an actual articy location we can't create anything
		if (mdl == null)
		{
			EditorGUILayout.HelpBox("Selected object is not a valid location", MessageType.Warning, true);
		}

		UnityEngine.GUI.enabled = mdl != null;

		if (GUILayout.Button("Generate/Update location game objects"))
		{
			// this is a bit brute force, and will clear all children underneath our current game object
			// the user should make sure not to place anything important in it, because it will get overwritten
			targetGameObject.transform.Clear();
			// start the process
			ArticyLocationGenerator.GenerateLocation(targetGameObject, mdl, locationContainer.pixelsToUnits);
			// make sure a save will catch that
			EditorUtility.SetDirty(targetGameObject);
			EditorSceneManager.MarkAllScenesDirty();
		}
		UnityEngine.GUI.enabled = true;

		serializedObject.ApplyModifiedProperties();
	}
}


