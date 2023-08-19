using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Cue.Core
{
    public class TouchCreator
    {
        private static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly Dictionary<string, FieldInfo> fields;

        private readonly object touch;

        public float DeltaTime
        { get { return ((Touch)touch).deltaTime; } set { fields["m_timeDelta"].SetValue(touch, value); } }
        public int TapCount
        { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
        public TouchPhase Phase
        { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
        public Vector2 DeltaPosition
        { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
        public int FingerId
        { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
        public Vector2 Position
        { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
        public Vector2 RawPosition
        { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

        public static BindingFlags Flag { get => flag; set => flag = value; }

        public Touch Create() => (Touch)touch;

        public TouchCreator() => touch = new Touch();

        static TouchCreator()
        {
            fields = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo fieldInfo in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                fields.Add(fieldInfo.Name, fieldInfo);
            }
        }
    }
}