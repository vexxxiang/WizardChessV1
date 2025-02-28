using UnityEngine;
using System.Reflection;

public static class ComponentExtensions
{
    public static T DeepCopy<T>(this T original, GameObject destination) where T : Component
    {
        T copy = destination.AddComponent<T>();
        CopyComponent(original, copy);
        return copy;
    }

    private static void CopyComponent<T>(T original, T copy) where T : Component
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        FieldInfo[] fields = typeof(T).GetFields(flags);

        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
    }
}
