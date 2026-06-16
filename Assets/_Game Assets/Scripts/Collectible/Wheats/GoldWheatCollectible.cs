using UnityEngine.UI;
using UnityEngine;

public class GoldWheatCollectibels : MonoBehaviour , ICollectible
{
     [SerializeField] private WheatDesignSO _wheatDesignSO;

     [SerializeField] private PlayerController  _playerController;

     [SerializeField] private PlayerStateUI _PlayerStateUI;

     private RectTransform _playerBoosterTransform;

     private Image _playerBoosterImage;

    void Awake()
    {
        _playerBoosterTransform = _PlayerStateUI.GetBoosterSpeedTransform;
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

    public void Collect()
    {
      _playerController.SetMovementSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDirection);

      _PlayerStateUI.PlayBoosterUIanimator(
        _playerBoosterTransform, _playerBoosterImage, _PlayerStateUI, _wheatDesignSO.ActiveSprite, _wheatDesignSO.PassiveSprite, 
        _wheatDesignSO.ActiveWheatSprite, _wheatDesignSO.PassiveWheatSprite, _wheatDesignSO.ResetBoostDirection);

      Destroy(gameObject);
    }

}
