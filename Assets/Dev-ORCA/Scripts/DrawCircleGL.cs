using UnityEngine;

public class DrawCircleGL : MonoBehaviour
{
    public float radius = 5f;   // 圆的半径
    public int segments = 36;   // 圆的分段数
    public Vector2 pos = Vector2.zero;

    void OnRenderObject()
    {
        GL.PushMatrix();
        GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.red); // 设置颜色

        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pointOnCircle = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            GL.Vertex3(pointOnCircle.x + pos.x, 0, pointOnCircle.y);
        }

        GL.End();
        GL.PopMatrix();
    }
}