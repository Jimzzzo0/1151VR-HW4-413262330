using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("飛行移動設定")]
    public float forwardSpeed = 10f; // 前進速度
    public float turnSpeed = 50f;    // 左右轉向靈敏度

    [Header("音效設定")]
    public AudioClip crashClip;      // 撞擊音效 
    public AudioClip passClip;       // 通過音效

    private AudioSource audioSource;
    private bool isGameOver = false;

    void Start()
    {
        Application.targetFrameRate = 60; // 鎖定影格率
        audioSource = GetComponent<AudioSource>(); // 取得 AudioSource 
    }

    void Update()
{
    if (isGameOver) return;

    // 檢查前進/推進控制 按住 W 
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
        // 往前飛
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 左右轉向 (A/D 鍵)
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);
    }

    // 爬升與下降控制 (按 Space 往上飛，按 LeftShift 往下飛)
    if (Input.GetKey(KeyCode.Space))
    {
        // 往上方爬升
        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.LeftShift))
    {
        // 往下方下降
        transform.Translate(Vector3.down * forwardSpeed * Time.deltaTime);
    }
}

    // 實體碰撞障礙物
    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        // 碰撞到設有 Obstacle 的物件
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;

            // 停止原本持續發出的引擎聲
            if (audioSource != null)
            {
                audioSource.Stop();

                // 播放撞毀音效[cite: 66, 72]
                if (crashClip != null)
                {
                    audioSource.PlayOneShot(crashClip);
                }
            }
        }
    }

    // 穿過感應區
    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        // 穿過設有 Goal 標籤的感應區
        if (other.CompareTag("Goal") || other.CompareTag("Finish"))
        {
            isGameOver = true;

            // 停止引擎聲
            if (audioSource != null)
            {
                audioSource.Stop();

                // 播放通過音效[cite: 66, 72]
                if (passClip != null)
                {
                    audioSource.PlayOneShot(passClip);
                }
            }
        }
    }
}