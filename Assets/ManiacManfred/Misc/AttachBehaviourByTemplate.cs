using System;


/// <summary>
/// This is a custom attribute and we use it to mark our custom scripts for the location creator. 
/// This will tell the location creator "If you create a game object for an object with a template, please attach an instance of the attributed class to it"
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AttachBehaviourByTemplateAttribute : Attribute
{
	/// <summary>
	/// Gets the name of the template.
	/// </summary>
	public string TemplateName { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="AttachBehaviourByTemplateAttribute"/> class.
	/// </summary>
	/// <param name="aTemplateName">Name of a template.</param>
	public AttachBehaviourByTemplateAttribute(string aTemplateName)
	{
		if (!aTemplateName.EndsWith("Template"))
			TemplateName = aTemplateName + "Template";
		TemplateName = aTemplateName;
	}
}


