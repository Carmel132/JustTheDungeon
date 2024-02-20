using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
#endif


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
    [field:SerializeField]
    public float duration { get; set; }
    public bool isAvailable { get { return (Time.time - last) > duration; } set { isAvailable = value; } }

    public void Reset()
    {
        last = Time.time;
    }

    public TimeCooldown(float ?_d =null)
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

public interface IBasicAbility<T1, T2> : IAbility<T2>
{
    void Activate(T1 args);
    ICooldown cd { get; set; }
}

public interface IGunBasicAbilityInfo
{
    public ICooldown cd { get; set; }
    public IBasicProjectileInfo proj { get; set; }
}

public class GunBasicAbilityInfo : IGunBasicAbilityInfo
{
    public ICooldown cd { get; set; }
    public IBasicProjectileInfo proj { get; set; }
    public GunBasicAbilityInfo(ICooldown _cd, IBasicProjectileInfo _info)
    {
        cd = _cd;
        proj = _info;
    }
}

public class GunAbilityInfo : IBasicProjectileInfo
{
    public float projSpeed { get; set; }
    public Vector3 target { get; set; }
    public GameObject proj { get; set; }
    public float lifetime { get; set; }
    public float bloom { get; set; }
    public Transform parent { get; set; }
    public Vector3 start { get; set; }

    public GunAbilityInfo(float projSpeed, Vector3 target, Vector3 start, GameObject proj, float lifetime, float bloom, Transform parent)
    {
        this.projSpeed = projSpeed;
        this.target = target;
        this.proj = proj;
        this.lifetime = lifetime;
        this.bloom = bloom;
        this.parent = parent;
        this.start = start;
    }
}

public interface IBasicProjectileInfo
{
    public float projSpeed { get; set; }
    public Vector3 target { get; set; }
    public GameObject proj { get; set; }
    public float lifetime { get; set; }
    public float bloom { get; set; }
    public Transform parent { get; set; }
    public Vector3 start { get; set; }
}

//keep
public class BasicProjectile
{
    IBasicProjectileInfo info;
    GameObject proj;
    public BasicProjectile(IBasicProjectileInfo _info)
    {
        info = _info;
    }

    public void Start()
    {
        proj = GameObject.Instantiate(info.proj);
        proj.transform.position = (Vector2)info.start;
        proj.transform.SetParent(info.parent);
        //transform.right = target.position - transform.position;
        Vector2 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-info.bloom, info.bloom)) * ((Vector2)info.target - (Vector2)proj.transform.position).normalized * info.projSpeed;
        proj.transform.right = dir - (Vector2)proj.transform.position;
        proj.GetComponent<Rigidbody2D>().AddForce(dir);
        GameObject.Destroy(proj, info.lifetime);
    }
}

public class GunBasicAbility : IBasicAbility<Vector3, IGunBasicAbilityInfo>
{
    public IGunBasicAbilityInfo info;
    //List<BasicProjectile> bullets = new List<BasicProjectile>();
    public ICooldown cd { get; set; }

    public void Activate(Vector3 target)
    {
        if (cd.isAvailable)
        {
            info.proj.target = target;
            var p = new BasicProjectile(info.proj);
            //bullets.Add(p);
            p.Start();
            cd.Reset();
        }
    }
    public void Start(IGunBasicAbilityInfo proj)
    {
        info = proj;
        cd = info.cd;
        //cd.Reset();
    }
}

