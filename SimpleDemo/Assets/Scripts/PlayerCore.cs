using System.Collections;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    #region Component References
    
    public static PlayerCore instance;
    private SpriteRenderer sp;
    [SerializeField] private Transform playerRevivePos;

    private Blocks[] blocks;
    
    #endregion
    
    // Invert Color
    public float changeRequest = 0.2f;
    private static readonly int Threshold = Shader.PropertyToID("_Threshold");
    
    // Fall to death height
    [SerializeField] private float deathHeight = -4.5f;
    
    public bool insideBlock;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this) 
                Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        blocks = FindObjectsOfType<Blocks>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (insideBlock == false)
            {
                InvertColor();
            }
            if (insideBlock)
            {
                InvertColor();
                Debug.Log("挤死了");
                PlayerDie();
            }

            foreach (var block in blocks)
            {
                block.ChangeState();
            }

        }
        
        
        FallToDeath();
        
    }
    

    private void InvertColor()
    {
        changeRequest = 1 - changeRequest;
        sp.material.SetFloat(Threshold, changeRequest);
    }

    private void PlayerDie()
    {
        sp.enabled = false;
        StartCoroutine(PlayerRevive());
    }

    private IEnumerator PlayerRevive()
    {
        yield return new WaitForSeconds(1f);
        transform.position = playerRevivePos.position;
        sp.enabled = true;
    }
    
    private void FallToDeath()
    {
        if (transform.position.y < deathHeight)
        {
            Debug.Log("摔死了");
            PlayerDie();
        }
    }
    
    
}
