using UnityEngine;

public class SignOnTheLeft : MonoBehaviour
{
    [SerializeField] private GameObject signBox;

    public bool sceneStart;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            signBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            signBox.SetActive(false);
            sceneStart = true;
        }
    }
}
