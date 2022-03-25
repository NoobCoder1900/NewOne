using UnityEngine;

public class InSideBlock : MonoBehaviour
{
    //这个脚本仅用于砖块的子物体，用于检测Player是否在砖块内部
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("在砖块里");
            PlayerCore.instance.insideBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("从砖块出来了");
            PlayerCore.instance.insideBlock = false;
        }
    }
}
