/****************************************************
    文件：MeshDraw.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MeshDraw : MonoBehaviour 
{
    public Material mat;
    private void Start() {
        //DrawTriangle();
        DrawCircle(0.5f,20,Vector3.zero);
        //DrawSquare();
    }
    public void DrawTriangle() {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        //设置顶点位置
        mesh.vertices = new Vector3[] {new Vector3(0,0,0),new Vector3(0,1,0),new Vector3(1,1,0) };
        //设置顶点顺序，顺时针为正方向，逆时针为反方向，三角数量为3的倍数
        mesh.triangles = new int[] { 1, 2, 0  };

        //UV
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
    }
    public void DrawSquare() {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        //设置顶点位置
        mesh.vertices = new Vector3[] { 
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0), 
            new Vector3(1, 1, 0),
            new Vector3(1,0,0)};
        //设置顶点顺序，顺时针为正方向，逆时针为反方向，三角数量为3的倍数
        mesh.triangles = new int[] { 
            1, 2, 0,
            0, 2, 3 };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    }
    public void DrawCircle(float radius,int segments,Vector3 centerCircle) {

        gameObject.GetComponent<MeshRenderer>().material = mat;
       
        //顶点
        Vector3[] vertices = new Vector3[segments + 1];
        vertices[0] = centerCircle;
        float deltaAngle = Mathf.Deg2Rad * 360f / segments;
        float currentAngle = 0;
        for (int i = 1; i < vertices.Length; i++) {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
            currentAngle += deltaAngle;
        }
        //三角形
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++) {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].y / radius / 2 + 0.5f);
        }

        //设置顶点位置
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }
}