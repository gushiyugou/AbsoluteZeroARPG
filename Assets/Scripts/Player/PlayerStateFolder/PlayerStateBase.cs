using UnityEngine;

public class PlayerStateBase : StateBase
{

    protected static bool isAnimationInverted = false;
    protected PlayerController _player;
    protected float BaseGravityVelocity = 200;
    //所有派生类都可以获取并赋值，适用于来控制不同状态下的跳跃的距离控制。
    protected static float moveStatePower;
    protected static StateBase previousState;
    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        _player = owner as PlayerController;
    }

    protected virtual bool CheckAnimatorStateName(string stateName,out float normalizedTime)
    {
        AnimatorStateInfo nextState = _player._PlayerModle._Animator.GetNextAnimatorStateInfo(0);
        if (nextState.IsName(stateName))
        {
            normalizedTime = nextState.normalizedTime;
            return true;
        }


        AnimatorStateInfo currentState = _player._PlayerModle._Animator.GetCurrentAnimatorStateInfo(0);
        normalizedTime = currentState.normalizedTime;
        return currentState.IsName(stateName);
    }

    //主动运用重力
    protected void UpdataGravity()
    {
        if (!_player._CharacterController.isGrounded)
        {
            BaseGravityVelocity += _player._gravity * Time.deltaTime;
            _player._CharacterController.Move(Vector3.down * BaseGravityVelocity * Time.deltaTime);
            Debug.Log(BaseGravityVelocity);
        }
    }

    protected void SyncModelToController()
    {
        // 获取控制器位置
        Vector3 controllerPosition = _player.transform.position;

        // 同步模型位置到控制器位置
        _player._PlayerModle.transform.position = controllerPosition;

        //// 如果使用根运动，可能需要禁用或调整
        //_player._PlayerModle._Animator.applyRootMotion = false;
    }

    public static void GetPerviousState(StateBase state)
    {
        previousState = state;
    }

    protected void SetRootAnima(bool setState)
    {
        _player._PlayerModle._Animator.applyRootMotion = setState;
    }

    protected bool GetRootAnimaState()
    {
        return _player._PlayerModle._Animator.applyRootMotion;
    }
}
