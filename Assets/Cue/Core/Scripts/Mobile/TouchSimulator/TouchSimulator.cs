using System.Collections.Generic;
using UnityEngine;

namespace Cue.Core
{
    public class TouchSimulator : MonoBehaviour
    {
        private static TouchCreator lastSimulatedTouch;

        public static List<Touch> GetTouches()
        {
            List<Touch> touches = new List<Touch>();
            touches.AddRange(Input.touches);
#if UNITY_EDITOR
            if (lastSimulatedTouch == null)
                lastSimulatedTouch = new TouchCreator();
            if (Input.GetMouseButtonDown(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Began;
                lastSimulatedTouch.DeltaPosition = new Vector2(0, 0);
                lastSimulatedTouch.Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.FingerId = 0;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Ended;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.DeltaPosition = newPosition - lastSimulatedTouch.Position;
                lastSimulatedTouch.Position = newPosition;
                lastSimulatedTouch.FingerId = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Moved;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.DeltaPosition = newPosition - lastSimulatedTouch.Position;
                lastSimulatedTouch.Position = newPosition;
                lastSimulatedTouch.FingerId = 0;
            }
            else
                lastSimulatedTouch = null;
            if (lastSimulatedTouch != null)
                touches.Add(lastSimulatedTouch.Create());
#endif
            return touches;
        }
    }
}