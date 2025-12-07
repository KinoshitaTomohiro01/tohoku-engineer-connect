using System;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] _ballPrefab;
    [SerializeField] private Transform _ballParent;

    public event Action<BallPhysicsManager> OnCreatedBall;

    public GameObject CreateBall()
    {
        int ballSize = UnityEngine.Random.Range(0, 2);
        return CreateBall(ballSize, this.transform.position, false);    // アクティブなボール生成時は落下させない
    }

    public GameObject CreateBall(int ballSize, Vector3 position, bool isDropped = true)
    {
        GameObject newBall = Instantiate(_ballPrefab[ballSize], position, Quaternion.identity, _ballParent);
        BallPhysicsManager ballPhysicsManager = newBall.GetComponent<BallPhysicsManager>();
        ballPhysicsManager.Initialize(ballSize, isDropped);
        OnCreatedBall.Invoke(ballPhysicsManager);  // ボール生成時に、同サイズのボールと接触したらマージ処理を登録するために、登録処理を呼び出すイベントを発火
        return newBall;
    }
}
