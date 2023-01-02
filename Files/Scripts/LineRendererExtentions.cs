using UnityEngine;
public static class LineRendererExtentions {
    public static Vector3 MousePosInWorld() {
        return MousePosInWorld(Camera.main);
    }
    public static Vector3 MousePosInWorld(Camera cam) {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
    public static Vector3 ClampVectorsToLength(Vector3 startVector, Vector3 endVector, float length) {
        return startVector - Vector3.ClampMagnitude(startVector - endVector, length);
    }

    public static void ClampToLength(this LineRenderer lr, float length) {
        Vector3[] positions = new Vector3[2];
        lr.GetPositions(positions);
        Vector3 newEnd = ClampVectorsToLength(positions[0], positions[1], length);
        positions[1] = newEnd;
        lr.SetPositions(positions);
    }

    public static void ClampLineRendererLength(LineRenderer lr, float length) {
        Vector3[] positions = new Vector3[2];
        lr.GetPositions(positions);
        Vector3 newEnd = ClampVectorsToLength(positions[0], positions[1], length);
        positions[1] = newEnd;
        lr.SetPositions(positions);
    }
}

