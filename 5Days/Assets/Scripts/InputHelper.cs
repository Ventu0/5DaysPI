
using UnityEngine;
public static class InputHelper {
    public static bool GetPrimaryDown() {
        if (Input.touchCount > 0) {
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }
        return Input.GetMouseButtonDown(0);
    }
    public static bool GetPrimary() {
        if (Input.touchCount > 0) {
            var p = Input.GetTouch(0).phase;
            return p == TouchPhase.Moved || p == TouchPhase.Stationary;
        }
        return Input.GetMouseButton(0);
    }
    public static bool GetPrimaryUp() {
        if (Input.touchCount > 0) {
            return Input.GetTouch(0).phase == TouchPhase.Ended;
        }
        return Input.GetMouseButtonUp(0);
    }
    public static Vector3 PrimaryPosition {
        get {
            if (Input.touchCount > 0) return Input.GetTouch(0).position;
            return Input.mousePosition;
        }
    }
}
