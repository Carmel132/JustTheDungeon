#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Util
{
    /// <summary>
    /// Gets a component from game object that implements an interface
    /// </summary>
    /// <typeparam name="T">What component should implement</typeparam>
    /// <param name="obj"> object to search component in</param>
    /// <returns>Component as T</returns>
    public static T? GetComponentThatImplements<T>(GameObject obj)
    {
        foreach (Component _ in obj.GetComponents(typeof(Component)))
        {
            if (_ is T t)
            {
                return t;
            }
        }
        return default;
    }
    /// <summary>
    /// Searches root for an event manager object
    /// </summary>
    /// <returns>The event manager component of the event manager</returns>
    public static EventManager GetEventManager()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(it => it.GetComponent<EventManager>() != null).First().GetComponent<EventManager>();
    }
    /// <summary>
    /// Sends a message to the event manager
    /// </summary>
    /// <typeparam name="T">Message group</typeparam>
    /// <param name="func">function to be called</param>
    public static void CallEvent<T>(ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
    {
        ExecuteEvents.Execute(GetEventManager().gameObject, null, func);
    }
    /// <summary>
    /// Searches object for child by name
    /// </summary>
    /// <param name="parent">parent object</param>
    /// <param name="name">name of child</param>
    /// <returns>child with name `name`</returns>
    public static Transform GetChildByName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; ++i) 
        {
            if (parent.GetChild(i).name == name) return parent.GetChild(i);
        }
        return parent;
    }
    /// <summary>
    /// Attempts to get a component from a game object, and instantiates one if failed
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <param name="gameObject">The game object being referenced</param>
    /// <param name="component">The found/added component</param>
    public static void TryGetElseAddComponent<T>(GameObject gameObject, out T component) where T: Component
    {
        if (!gameObject.TryGetComponent(out component)) component = gameObject.AddComponent<T>();
    }
}
