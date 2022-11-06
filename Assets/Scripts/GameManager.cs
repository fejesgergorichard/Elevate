using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject HolePrefab;
    public GameObject CoinPrefab;
    public int HolesCount = 300;

    [Header("Coin fly settings")]
    public Canvas Canvas;
    public RectTransform CoinFlyTarget;
    public TMP_Text CoinTextTop;
    public GameObject CoinSprite;

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
        DontDestroyOnLoad(Instance);
    }

    #endregion

    public async void AddCoin(Vector3 coinWorldPos, int num)
    {
        //Vector2 coinScreenPos = Camera.main.WorldToScreenPoint(coinWorldPos);
        var initialTargetScale = 10f;

        LeanTween.scale(CoinFlyTarget, Vector3.one * initialTargetScale * 1.2f, 0.1f).setDelay(0.2f);

        //float pitch = 0.9f;
        //for (int i = 0; i < num; i++)
        //{
        Score++;

        //    pitch += 0.1f;
        //    //AudioManager.PlaySound("Coin", pitch);

        //    var coin = Instantiate(CoinSprite, coinScreenPos, Quaternion.identity);
        //    coin.transform.SetParent(CoinFlyTarget.parent.parent);
        //    coin.transform.localPosition = coinScreenPos;
        //    LeanTween.moveLocal(coin, CoinFlyTarget.localPosition, 0.4f).setEaseInQuad().setIgnoreTimeScale(true);
        //    Destroy(coin, 0.4f);


        //    if (num > 1)
        //        await Task.Run(() => Thread.Sleep(65));
        //}

        LeanTween.scale(CoinFlyTarget, Vector3.one * initialTargetScale, 0.2f).setDelay(0.3f);

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
                Instantiate(CoinPrefab, coinSpawnPosition, Quaternion.identity);
            }

            Instantiate(HolePrefab, holeSpawnPosition, Quaternion.identity);
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
