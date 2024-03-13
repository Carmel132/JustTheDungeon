using System;
using UnityEngine;
#if UNITY_EDITOR
#endif
/*
[System.AttributeUsage(System.AttributeTargets.Field)]
public class SerializeProperty : PropertyAttribute
{
    public string PropertyName { get; private set; }

    public SerializeProperty(string propertyName)
    {
        PropertyName = propertyName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SerializeProperty))]
public class SerializePropertyAttributeDrawer : PropertyDrawer
{
    private PropertyInfo propertyFieldInfo = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        UnityEngine.Object target = property.serializedObject.targetObject;

        // Find the property field using reflection, in order to get access to its getter/setter.
        if (propertyFieldInfo == null)
            propertyFieldInfo = target.GetType().GetProperty(((SerializeProperty)attribute).PropertyName,
                                                 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (propertyFieldInfo != null)
        {

            // Retrieve the value using the property getter:
            object value = propertyFieldInfo.GetValue(target, null);

            // Draw the property, checking for changes:
            EditorGUI.BeginChangeCheck();
            value = DrawProperty(position, property.propertyType, propertyFieldInfo.PropertyType, value, label);

            // If any changes were detected, call the property setter:
            if (EditorGUI.EndChangeCheck() && propertyFieldInfo != null)
            {

                // Record object state for undo:
                Undo.RecordObject(target, "Inspector");

                // Call property setter:
                propertyFieldInfo.SetValue(target, value, null);
            }

        }
        else
        {
            EditorGUI.LabelField(position, "Error: could not retrieve property.");
        }
    }

    private object DrawProperty(Rect position, SerializedPropertyType propertyType, Type type, object value, GUIContent label)
    {
        switch (propertyType)
        {
            case SerializedPropertyType.Integer:
                return EditorGUI.IntField(position, label, (int)value);
            case SerializedPropertyType.Boolean:
                return EditorGUI.Toggle(position, label, (bool)value);
            case SerializedPropertyType.Float:
                return EditorGUI.FloatField(position, label, (float)value);
            case SerializedPropertyType.String:
                return EditorGUI.TextField(position, label, (string)value);
            case SerializedPropertyType.Color:
                return EditorGUI.ColorField(position, label, (Color)value);
            case SerializedPropertyType.ObjectReference:
                return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
            case SerializedPropertyType.ExposedReference:
                return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
            case SerializedPropertyType.LayerMask:
                return EditorGUI.LayerField(position, label, (int)value);
            case SerializedPropertyType.Enum:
                return EditorGUI.EnumPopup(position, label, (Enum)value);
            case SerializedPropertyType.Vector2:
                return EditorGUI.Vector2Field(position, label, (Vector2)value);
            case SerializedPropertyType.Vector3:
                return EditorGUI.Vector3Field(position, label, (Vector3)value);
            case SerializedPropertyType.Vector4:
                return EditorGUI.Vector4Field(position, label, (Vector4)value);
            case SerializedPropertyType.Rect:
                return EditorGUI.RectField(position, label, (Rect)value);
            case SerializedPropertyType.AnimationCurve:
                return EditorGUI.CurveField(position, label, (AnimationCurve)value);
            case SerializedPropertyType.Bounds:
                return EditorGUI.BoundsField(position, label, (Bounds)value);
            default:
                throw new NotImplementedException("Unimplemented propertyType " + propertyType + ".");
        }
    }

}
#endif*/


public interface ICooldown
{
    bool isAvailable { get; set; }
    public float duration { get; set; }

    public void Reset();
}
[System.Serializable]
public class TimeCooldown : ICooldown
{
    float last;
    [field: SerializeField]
    public float duration { get; set; }
    public bool isAvailable { get { return (Time.time - last) > duration; } set { isAvailable = value; } }

    public void Reset()
    {
        last = Time.time;
    }

    public TimeCooldown(float? _d = null)
    {
        if (_d is float) { duration = (float)_d; }
    }

}
[System.Serializable]
public class ChargeCooldown : ICooldown
{
    public bool isAvailable { get { return getCharges() > 0; } set { isAvailable = value; } }
    int maxCharges = 0;

    float last;

    int getCharges()
    {
        return (int)Mathf.Clamp(Mathf.Floor((Time.time - last) / duration), 0, maxCharges);
    }

    public ChargeCooldown(int max, int start = 0)
    {
        maxCharges = max;
        last -= start * duration;
    }

    public float duration { get; set; }

    public void Reset()
    {
        if (isAvailable)
        {
            if (getCharges() == maxCharges)
            {
                last = Time.time - duration * (maxCharges - 1);
            }
            else
            {
                last += duration;
            }

        }
        else
        {
            last = Time.time;
        }
    }
}