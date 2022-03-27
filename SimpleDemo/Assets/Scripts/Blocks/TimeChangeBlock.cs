using UnityEngine;

public class TimeChangeBlock : MonoBehaviour
{
    //[SerializeField] private float invokeTime = 5f;
    [SerializeField] private float timeDuration = 5;
    [SerializeField] private float changeRate = 0.5f;
    private float currentTime;
    private SpriteRenderer sp;
    
    private void Start()
    {
        TryGetComponent(out sp);
        currentTime = 5;
    }

    private void Update()
    {
        Debug.Log(currentTime);

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime * changeRate;
        }

        if (currentTime <= 0)
        {
            currentTime = timeDuration;

            sp.color = sp.color == Color.white ? Color.black : Color.white;
        }
    }
    
}
