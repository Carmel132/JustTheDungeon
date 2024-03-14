#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Util
{

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
    public static EventManager GetEventManager()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(it => it.GetComponent<EventManager>() != null).First().GetComponent<EventManager>();
    }
}
