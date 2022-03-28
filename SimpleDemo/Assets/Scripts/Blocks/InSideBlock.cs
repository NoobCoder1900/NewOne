using UnityEngine;

public class InSideBlock : MonoBehaviour
{
    //这个脚本仅用于砖块的子物体，用于检测Player是否在砖块内部
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCore.instance.insideBlock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCore.instance.insideBlock = false;
        }
    }
}
