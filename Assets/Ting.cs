using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public bool Inverse = false;

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector = false, bool inverse = false)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
        Inverse = inverse;
    }
}