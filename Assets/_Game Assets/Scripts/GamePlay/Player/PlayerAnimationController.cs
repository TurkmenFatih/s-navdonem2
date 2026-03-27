using UnityEngine;

public class PlayerAnimationcontroller : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private PlayerController _playerController;

    private StateController _stateController;

    private void Awake() 
    {
        _playerController= GetComponent<PlayerController>();
         _stateController= GetComponent<StateController>();

    }
}
