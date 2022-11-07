using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject HolesContainer;
    public GameObject CoinsContainer;
    public GameObject HolePrefab;
    public GameObject CoinPrefab;
    public int HolesCount = 300;

    [Header("Coin fly settings")]
    public Transform FlyingCoinParent;
    public RectTransform CoinFlyTarget;
    public TMP_Text CoinTextTop;
    public GameObject CoinSprite;

    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Space]
    [Header("Animation settings")]
    [SerializeField][Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField][Range(0.9f, 2f)] float maxAnimDuration;

    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            CoinTextTop.text = _score.ToString();
        }
    }

    #region Singleton

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager in the scene!");
        }
        else
        {
            Instance = this;
        }

        PrepareCoins();
    }

    #endregion



    private void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(CoinSprite);
            coin.transform.SetParent(FlyingCoinParent.transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    private void Animate(Vector3 collectedCoinPosition, int amount)
    {
        // make the target icon larger
        var initialTargetScale = 35f;
        LeanTween.scale(CoinFlyTarget, Vector3.one * initialTargetScale * 1.2f, 0.1f).setDelay(0.2f);

        // Animate coins towards counter
        for (int i = 0; i < amount; i++)
        {
            if (coinsQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOLocalMove(CoinFlyTarget.localPosition, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                    
                    Score++;
                });
            }
        }

        // reset target icon scale to normal
        LeanTween.scale(CoinFlyTarget, Vector3.one * initialTargetScale, 0.2f).setDelay(0.3f);
    }

    public void AddCoins(Vector3 collectedCoinPosition, int amount)
    {
        Animate(collectedCoinPosition, amount);
    }

    void Start()
    {
        InitializeAppSettings();
        SpawnMap();
        Score = 0;
    }

    private void SpawnMap()
    {
        Vector3 holeSpawnPosition = new Vector3();
        Vector3 coinSpawnPosition = new Vector3();

        for (int i = 0; i < HolesCount; i++)
        {
            var prevY = holeSpawnPosition.y;
            var prevX = holeSpawnPosition.x;

            holeSpawnPosition.y += Random.Range(0.8f, 2.0f);
            holeSpawnPosition.x = Random.Range(-2f, 2f);

            coinSpawnPosition.x = prevX + (holeSpawnPosition.x - prevX) / 2;
            coinSpawnPosition.y = prevY + (holeSpawnPosition.y - prevY) / 2;

            if (i != 0 && Vector3.Distance(coinSpawnPosition, holeSpawnPosition) > 1f)
            {
                Instantiate(CoinPrefab, coinSpawnPosition, Quaternion.identity, CoinsContainer.transform);
            }

            Instantiate(HolePrefab, holeSpawnPosition, Quaternion.identity, HolesContainer.transform);
        }
    }
    
    private static void InitializeAppSettings()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;

        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }
}
