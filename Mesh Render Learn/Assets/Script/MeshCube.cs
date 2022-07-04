/****************************************************
    文件：MeshCube.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：#CreateTime#
	功能：创建mesh
*****************************************************/

using UnityEngine;

public class MeshCube : MonoBehaviour 
{

    public Material mat;
    [SerializeField]
    public Vector2[] uvs;
    private void Start() {
        CreatCube();
    }
   void CreatCube() {
        GameObject obj = new GameObject("cube");
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[24];
        int[] triangles = new int[36];

        //forward
        vertices[0].Set(0.5f, -0.5f, 0.5f);//左下
        vertices[1].Set(-0.5f, -0.5f, 0.5f);//右下
        vertices[2].Set(0.5f, 0.5f, 0.5f);//左上
        vertices[3].Set(-0.5f, 0.5f, 0.5f);//右上
        //back
        vertices[4].Set(vertices[2].x, vertices[2].y, -0.5f);//后左上
        vertices[5].Set(vertices[3].x, vertices[3].y, -0.5f);//后右上
        vertices[6].Set(vertices[0].x, vertices[0].y, -0.5f);//后左下
        vertices[7].Set(vertices[1].x, vertices[1].y, -0.5f);//后右下
        //up
        vertices[8] = vertices[2];//顶左下
        vertices[9] = vertices[3];//顶右下
        vertices[10] = vertices[4];//顶左上
        vertices[11] = vertices[5];//顶右上
        //down
        vertices[12].Set(vertices[10].x, -0.5f, vertices[10].z);
        vertices[13].Set(vertices[11].x, -0.5f, vertices[11].z);
        vertices[14].Set(vertices[8].x, -0.5f, vertices[8].z);
        vertices[15].Set(vertices[9].x, -0.5f, vertices[9].z);
        //right
        vertices[16] = vertices[6];
        vertices[17] = vertices[0];
        vertices[18] = vertices[4];
        vertices[19] = vertices[2];
        //left
        vertices[20].Set(-0.5f, vertices[18].y, vertices[18].z);
        vertices[21].Set(-0.5f, vertices[19].y, vertices[19].z);
        vertices[22].Set(-0.5f, vertices[16].y, vertices[16].z);
        vertices[23].Set(-0.5f, vertices[17].y, vertices[17].z);

        int currentCount = 0;
        for (int i = 0; i < 24; i = i + 4) {
            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 3;
            triangles[currentCount++] = i + 1;

            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 2;
            triangles[currentCount++] = i + 3;

        }

        //UV
        uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int width = 2;
        int heigth = 2;
        int length = 10;
        uvs[0] = new Vector2(1 * width, 1 * heigth);
        uvs[1] = new Vector2(1 * width, 0);
        uvs[2] = new Vector2(0, 1 * heigth);
        uvs[3] = new Vector2(0, 0);

        uvs[4] = new Vector2(0, 0);
        uvs[5] = new Vector2(0, 1 * heigth);
        uvs[6] = new Vector2(1 * width, 0);
        uvs[7] = new Vector2(1 * width, 1 * heigth);

        uvs[8] = new Vector2(0, 0);
        uvs[9] = new Vector2(0, 1 * heigth);
        uvs[10] = new Vector2(1 * length, 0);
        uvs[11] = new Vector2(1 * length, 1 * heigth);

        uvs[12] = new Vector2(0, 0);
        uvs[13] = new Vector2(0, 1 * heigth);
        uvs[14] = new Vector2(1 * length, 0);
        uvs[15] = new Vector2(1 * length, 1 * heigth);

        uvs[16] = new Vector2(1 * width, 1 * length);   //上
        uvs[17] = new Vector2(1 * width, 0);
        uvs[18] = new Vector2(0, 1 * length);
        uvs[19] = new Vector2(0, 0);

        uvs[20] = new Vector2(1 * width, 1 * length);    //下
        uvs[21] = new Vector2(1 * width, 0);
        uvs[22] = new Vector2(0, 1 * length);
        uvs[23] = new Vector2(0, 0);

        mr.material = mat;
        mf.mesh.uv =uvs;
        
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.RecalculateTangents();
        mf.mesh.RecalculateNormals();

        
       
    }
    public void Reset() {
        CreatCube();
    }
}