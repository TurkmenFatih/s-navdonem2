using UnityEngine;

[CreateAssetMenu(fileName ="WheatDesignSO",menuName ="ScriptableObjects/WheatDesignSO")]
public class WheatDesignSO : ScriptableObject
{
    

    [SerializeField] private float _increaseDecreaseMultiplier;

    [SerializeField] private float _resetBoostDirection;

    public float IncreaseDecreaseMultiplier => _increaseDecreaseMultiplier;

    public float ResetBoostDirection => _resetBoostDirection;

}
