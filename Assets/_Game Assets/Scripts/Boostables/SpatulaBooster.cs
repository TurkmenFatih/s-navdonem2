using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoosttable
{
    [Header ("References")]
    [SerializeField] private Animator _spatulaAnimator;

     [Header ("Settings")]
    [SerializeField] private float _jumpForce;

    private bool _isActivated;
    public void Boost(PlayerController playerController)
    {
        if(_isActivated) {return;}
        
        PlayBoostAnimation();

        Rigidbody playerRigibody = playerController.GetPlayerRigibody();

        playerRigibody.linearVelocity = new Vector3 (playerRigibody.linearVelocity.x , 0f , playerRigibody.linearVelocity.z);
        playerRigibody.AddForce(transform.forward* _jumpForce, ForceMode.Impulse);
        _isActivated=true;
        Invoke(nameof(ResetActivation),0.2f);
    }
     private void PlayBoostAnimation()
    {
        _spatulaAnimator.SetTrigger(Consts.OtherAnimations.IS_SPATULA_JUMPING);
    }
    private void ResetActivation()
    {
        _isActivated=false;
    }
}
