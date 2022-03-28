using UnityEngine;

public class TimeChangeBlock : MonoBehaviour
{
    [SerializeField] private float timeDuration = 2f; //多久换一次颜色
    private float currentTime;
    private SpriteRenderer sp;
    private bool isBlackBox;

    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        TryGetComponent(out sp);
        TryGetComponent(out boxCollider2D);
        currentTime = timeDuration;
        isBlackBox = sp.color == Color.black;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        if (currentTime <= 0)
        {
            currentTime = timeDuration;

            sp.color = sp.color == Color.white ? Color.black : Color.white;
            isBlackBox = sp.color == Color.black;
        }
        
        switch (PlayerCore.instance.blackOrNot)
        {
            case false: //白砖块
                boxCollider2D.enabled = !isBlackBox;
                break;
            case true: //黑砖块
                boxCollider2D.enabled = isBlackBox;
                break;
        }
        
    }
    
}
