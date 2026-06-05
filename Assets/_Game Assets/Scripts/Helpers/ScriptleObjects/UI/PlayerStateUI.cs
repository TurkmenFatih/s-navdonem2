using System;
using System.Collections;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStateUI : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private RectTransform _playerWalkingTransform;
    [SerializeField] private RectTransform _playerSlidingTransform;
    [SerializeField] private RectTransform _boosterSpeedTransform;
    [SerializeField] private RectTransform _boosterJumpTransform;
    [SerializeField] private RectTransform _boosterSlowTransform;

    [Header("Images")]
    [SerializeField] private Image _goldBoosterWheatImage;

    [SerializeField] private Image _holyBoosterWheatImage;

    [SerializeField] private Image _rottenBoosterWheatImage;


    [Header("Sprites")]

    [SerializeField] private Sprite _playerSlidingActiveSprite;
     
    [SerializeField] private Sprite _playerSlidingPassiveSprite;

    [SerializeField] private Sprite _playerWalkingActiveSprite;
     
    [SerializeField] private Sprite _playerWalkingPassiveSprite;
     
    [Header("Settings")]
    [SerializeField] private float _moveDuration; 
    [SerializeField] private Ease _moveEase; 

    public RectTransform GetBoosterSpeedTransform => _boosterSpeedTransform;

    public RectTransform GetBoosterJumpTransform => _boosterJumpTransform;
    public RectTransform GetBoosterSlowtransform => _boosterSpeedTransform;

    public Image GetGoldWheatImage => _goldBoosterWheatImage;
    public Image GetHolyWheatImage => _goldBoosterWheatImage;

    
    private Image   _playerWalkingImage;
    private Image  _playerSlidingImage;

    void Awake()
    {
        _playerWalkingImage = _playerWalkingTransform.GetComponent<Image>();
        _playerSlidingImage = _playerSlidingTransform.GetComponent<Image>();
    }

    void Start()
    {
        _playerController.OnPlayerStateChanged += PlayerState_OnplayerStateChanged;
        SetStateUserInterfaces(_playerWalkingActiveSprite, _playerSlidingPassiveSprite, _playerWalkingTransform , _playerSlidingTransform);
    }

    private void PlayerState_OnplayerStateChanged(PlayerState playerState)
    {
        switch(playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Move:
            //ÜSTTEKİ KART AÇILACAK
            SetStateUserInterfaces(_playerWalkingActiveSprite, _playerSlidingPassiveSprite, _playerWalkingTransform , _playerSlidingTransform);
            break;

             case PlayerState.SlideIdle:
            case PlayerState.Slide:
            //ALTTAKi KART AÇILACAK
             SetStateUserInterfaces(_playerWalkingPassiveSprite, _playerSlidingActiveSprite,  _playerSlidingTransform, _playerWalkingTransform);
            break;
            
        }
    }
    private void SetStateUserInterfaces(Sprite playerWalkingSprite, Sprite playerSlidingSprite, RectTransform activeTransform, RectTransform passiveTransform)
    {
        _playerWalkingImage.sprite = playerWalkingSprite;
        _playerSlidingImage.sprite = playerSlidingSprite;

        activeTransform.DOAnchorPosX(-25f , _moveDuration).SetEase(_moveEase);
        passiveTransform.DOAnchorPosX(-90f , _moveDuration).SetEase(_moveEase);
    }

    private IEnumerator SetBoosterUserInterface
    (RectTransform activeTransform, Image boosterImage, Image wheatImage, Sprite activeSprite, Sprite passiveSprite,Sprite 
    activeWheatSprite,Sprite passiveWheatSprite, float duration)
    {
        boosterImage.sprite =activeSprite;
        wheatImage.sprite =activeWheatSprite;
        activeTransform.DOAnchorPosX(25f, _moveDuration).SetEase(_moveEase);

        yield return new WaitForSeconds(duration);
        boosterImage.sprite =passiveSprite;
        wheatImage.sprite =passiveWheatSprite;
        activeTransform.DOAnchorPosX(90f, _moveDuration).SetEase(_moveEase);
    }
    public void PlayBoosterUIanimator(RectTransform activeTransform, Image boosterImage, Image wheatImage, Sprite activeSprite, Sprite passiveSprite,Sprite 
    activeWheatSprite,Sprite passiveWheatSprite, float duration)
    {
        StartCoroutine(SetBoosterUserInterface(activeTransform,boosterImage,wheatImage,activeSprite,passiveSprite ,activeWheatSprite,passiveWheatSprite,duration));
    }
}
