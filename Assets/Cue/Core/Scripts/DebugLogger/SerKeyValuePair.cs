using System.Collections.Generic;

/// <summary>
/// A custom serializable version of a <seealso cref="KeyValuePair"/>
/// </summary>
/// <typeparam name="T1">Type of key</typeparam>
/// <typeparam name="T2">Type of value</typeparam>
[System.Serializable]
public class SerKeyValuePair<T1,T2>
{
    public T1 Key;
    public T2 Value;
}