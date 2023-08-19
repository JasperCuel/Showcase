using System.Collections.Generic;
using UnityEngine;

namespace Cue.Core
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DebugLoggerConfig", menuName = "Configs/DebugLoggerConfig")]
    public class DebugLogConfig : ScriptableObject
    {
        public List<SerKeyValuePair<DebugType, DebugLogSetting>> debugTypes;
    }
}