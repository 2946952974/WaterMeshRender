/****************************************************
    文件：PlayerController.cs
	作者：别离或雪
    邮箱: 2946952974@qq.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public Rigidbody2D m_rig;
    public float speed;
    public float jumpforce;
    public bool isJump;
    public float jumpTime;
    private void Start() {
        m_rig = transform.GetComponent<Rigidbody2D>(); 
    }
    private void Update() {
        if (Input.GetKey(KeyCode.A)) {
            m_rig.velocity = new Vector2(-3f,m_rig.velocity.y);
            //transform.Translate(Vector2.left * speed);
        }
        if (Input.GetKey(KeyCode.D)) {
            m_rig.velocity=new Vector2(3f, m_rig.velocity.y);
            //transform.Translate(Vector2.right * speed);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            m_rig.AddForce(new Vector2(0, jumpforce*2), ForceMode2D.Impulse);
            isJump = true;
            jumpTime = 2;
        }
        if (isJump&&jumpTime>0) {
            jumpTime -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space)) {
                m_rig.AddForce(new Vector2(0,jumpforce/3), ForceMode2D.Impulse);
            }
        }
    }
}