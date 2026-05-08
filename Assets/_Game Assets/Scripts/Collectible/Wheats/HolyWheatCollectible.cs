using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
  [SerializeField] private PlayerController  _playerController;

     [SerializeField] private float  _ForceIncrase;

     [SerializeField] private float  _resetBoostDirection;

     public void Collect()
    {
      _playerController.SetJumpForce(_ForceIncrase, _resetBoostDirection);
      Destroy(gameObject);
    }
}
