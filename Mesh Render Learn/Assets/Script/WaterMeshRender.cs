/****************************************************
    文件：WaterMeshRender.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：#CreateTime#
	功能：水面模拟
*****************************************************/

using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class WaterMeshRender : MonoBehaviour 
{

    [Header("水")]
    public float length;//水面长度
    public float width;//水深
    public int quality;//顶点数量

    public Material mat;//材质
    [Header("组件")]
    private BoxCollider2D boxCollider;
    private Bounds bounds;
    Vector3[] vertices;//顶点
    int[] triangles;//三角形

    float[] leftdetails;//向左扩散速度
    float[] rightdetails;//向右扩散速度
    float[] velocitys;//velocityY方向的速度
    float[] accelerated;//加速度

    public float damping=0.2f;//速度对恢复波动的影响力
    public float heighspeed=2f;//高度对恢复波动的影响力
    public float spread;//传播衰减值

    //public float MaxOffsetTop=1;//水面最大高度
    //public float MaxOffsetBottom=1;//水面最低深度

    public float collForce=0.1f;//物体落水力影响值
    private void Start() {
        vertices = new Vector3[quality*2];
        velocitys = new float[quality];
        accelerated = new float[quality * 2];
        leftdetails = new float[quality];
        rightdetails = new float[quality];
        DrawWater(length, width, quality);
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(4.8f, 0.74f);
        boxCollider.size = new Vector2(length, width);
        bounds = boxCollider.bounds;

        //可以自己波动范围限制
        //MaxOffsetTop = width+2;
        //MaxOffsetBottom = 0.5f;

        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 1;
    }
    private void Update() {
        //持续绘制
        //物理
        for (int i = 0; i < quality; i++) {
            //加速度的力
            float force =heighspeed*(vertices[i].y-width)+velocitys[i]*damping;
            //顶点变换
            vertices[i].y +=velocitys[i]
            //这里可以设置力的最大最小范围
                ;
            accelerated[i] = -force;
            //速度收到加速度和阻力衰减（这个数值自行设置，这里我设置速度大小的1/5）
            velocitys[i] += accelerated[i]-velocitys[i]/5;
        }
        //水扩散和衰减
        for (int i = 0; i < quality; i++) {
            if (i > 0) {
                leftdetails[i] = spread * (vertices[i].y - vertices[i - 1].y);
                velocitys[i - 1] += leftdetails[i];
            }
            if (i < quality - 1) {
                rightdetails[i] = spread * (vertices[i].y - vertices[i + 1].y);
                velocitys[i + 1] += rightdetails[i];
            }
        }
        //重新绘制顶点
        ReDraw(vertices);
    }
    //绘制水
    public void DrawWater(float length,float width,int quality) {
        //材质
        gameObject.GetComponent<MeshRenderer>().material = mat;
        //顶点
        float interval_length = length/quality;
        //分开设置数组前一部分是上顶点，后一半是下顶点
        //上面顶点
        for (int i = 0; i < quality; i++) {
            vertices[i] = new Vector3(i * interval_length, width, 0);
        }
        //下面顶点
        for (int i = quality; i < 2*quality; i++) {
            vertices[i] = new Vector3((i-quality) * interval_length, 0, 0);
        }
        //设置三角形
        int anglescount = (quality*2-2)*3;
        triangles = new int[anglescount];

        int current=0;
        for (int i = quality; i < 2 * quality - 1; i++) {

            triangles[current++] = i;
            triangles[current++] = i-quality;
            triangles[current++] = i-quality+1;

            triangles[current++] = i;
            triangles[current++] = i - quality + 1;
            triangles[current++] = i + 1;
        }
        //对应坐标UV设置
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x/length,vertices[i].y/width);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }
    public void ReDraw(Vector3[] currentvertices) {
        //设置三角形
        int anglescount = (quality * 2 - 2) * 3;
        triangles = new int[anglescount];

        int current = 0;
        for (int i = quality; i < 2 * quality - 1; i++) {

            triangles[current++] = i;
            triangles[current++] = i - quality;
            triangles[current++] = i - quality + 1;

            triangles[current++] = i;
            triangles[current++] = i - quality + 1;
            triangles[current++] = i + 1;
        }
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x / length, Mathf.Max(vertices[i].y / width,1));
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.vertices = currentvertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Vector2 velocity = col.GetComponent<Rigidbody2D>().velocity;
        FallIn(col, velocity.y*collForce);
        Debug.Log("Enter Water");
    }
    private void OnTriggerStay2D(Collider2D col) {
        Vector2 velocity = col.GetComponent<Rigidbody2D>().velocity;
        //持续呆在水中力为上方向，这里设置速度的绝对值
        StayWater(col, Mathf.Abs(velocity.x) * collForce);
    }
    private void OnTriggerExit2D(Collider2D col) {
        Vector2 velocity = col.GetComponent<Rigidbody2D>().velocity;
        JumpOut(col,velocity.y*collForce);
        Debug.Log("Out Water");
    }
    //落入水中
    private void FallIn(Collider2D col,float force) {
        //原点，因为绘制的顶点并不是世界坐标，需要加上原点的位置才是世界坐标
        Vector2 originalpos = transform.position;
        //计算物体包围盒宽度
        float radius = col.bounds.max.x - col.bounds.min.x;
        //计算中心点
        Vector2 center = new Vector2(col.bounds.center.x,width);
        //计算每个受力的大小，赋予初始速度
        for (int i = 0; i < quality; i++) {
            //只计算X距离
            float dis = Vector2.Distance(new Vector2(originalpos.x+vertices[i].x,width), center);
            if (dis < radius) {
                velocitys[i] = force * (radius - dis) / radius;
            }
        }
    }
    //跃出水面
    private void JumpOut(Collider2D col,float force) {
        Vector2 originalpos = transform.position;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, width);
        for (int i = 0; i < quality; i++) {
            float dis = Vector2.Distance(new Vector2(originalpos.x + vertices[i].x, width), center);
            if (dis < radius) {
                velocitys[i] = force * (radius - dis) / radius;
            }
        }
    }
    //水中移动
    private void StayWater(Collider2D col,float force) {
        //水中移动影响当前速度，不是赋予初始速度
        Vector2 originalpos = transform.position;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, width);
        for (int i = 0; i < quality; i++) {
            float dis = Vector2.Distance(new Vector2(originalpos.x + vertices[i].x, width), center);
            if (dis < radius) {
                velocitys[i] += (force) * dis /5;
            }
        }
    }
}