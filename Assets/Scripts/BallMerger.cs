using System;
using UnityEngine;

public class BallMerger : MonoBehaviour
{
    [SerializeField] private BallFactory _ballFactory;

    public event Action<int> OnMerged;

    private void Awake()
    {
        _ballFactory.OnCreatedBall += RegisterMergeBall;
    }

    private void MergeBall(GameObject ball1, GameObject ball2, int mergeBallSize)
    {
        // 重複呼び出し防止のために、インスタンスIDの小さい方を無視する
        if (ball1.GetInstanceID() < ball2.GetInstanceID())
            return; 

        Vector3 mergePosition = (ball1.transform.position + ball2.transform.position) / 2;

        if (mergeBallSize < 2)
        {
            GameObject newBall = _ballFactory.CreateBall(mergeBallSize + 1, mergePosition);
            var ballPhysicsManager = newBall.GetComponent<BallPhysicsManager>();
            ballPhysicsManager.BallDrop();
        }

        OnMerged?.Invoke(mergeBallSize);

        Destroy(ball1);
        Destroy(ball2);
    }

    private void RegisterMergeBall(BallPhysicsManager ballPhysicsManager)
    {
        ballPhysicsManager.OnBallCollided += MergeBall;
    }
}
