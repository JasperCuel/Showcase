using System;
using System.Collections.Generic;

namespace Cue.Networking
{
    /// <summary>
    /// Alternative for <seealso cref="KeyValuePair{TKey, TValue}"/> which is serializable by Unity
    /// </summary>
    [Serializable]
    public class BackendKeyValue
    {
        public string key;
        public string value;
    }
}