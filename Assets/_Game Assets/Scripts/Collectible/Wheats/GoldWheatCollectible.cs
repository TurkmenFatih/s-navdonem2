using UnityEngine;

public class GoldWheatCollectibels : MonoBehaviour
{
     [SerializeField] private PlayerController  _playerController;

     [SerializeField] private float  _movementIncraseSpeed;

     [SerializeField] private float  _resetBoostDirection;

     public void Collect()
    {
      _playerController.SetMovementSpeed(_movementIncraseSpeed, _resetBoostDirection);
      Destroy(gameObject);
    }

}
