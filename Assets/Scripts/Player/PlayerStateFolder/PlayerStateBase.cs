using UnityEngine;

public class PlayerStateBase : StateBase
{
    protected PlayerController _player;
    protected float BeseGravityVelocity = 200;
    //所有派生类都可以获取并赋值，适用于来控制不同状态下的跳跃的距离控制。
    protected static float jumpPower;
    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        _player = owner as PlayerController;
    }

    protected virtual bool CheckAnimatorStateName(string stateName,out float normalizedTime)
    {
        AnimatorStateInfo state = _player._PlayerModle._Animator.GetCurrentAnimatorStateInfo(0);
        normalizedTime = state.normalizedTime;
        return state.IsName(stateName);
    }

    protected void UpdataGravity()
    {
        if (!_player._CharacterController.isGrounded)
        {
            BeseGravityVelocity -= _player._gravity * Time.deltaTime;
            _player._CharacterController.Move(Vector3.down * BeseGravityVelocity * Time.deltaTime);
            Debug.Log(BeseGravityVelocity);
        }
    }

    protected void SyncModelToController()
    {
        // 获取控制器位置
        Vector3 controllerPosition = _player.transform.position;

        // 同步模型位置到控制器位置
        // 假设模型是控制器的子对象或同一对象
        _player._PlayerModle.transform.position = controllerPosition;

        //// 如果使用根运动，可能需要禁用或调整
        //_player._PlayerModle._Animator.applyRootMotion = false;
    }
}
