using System;
using System.Collections;
using UnityEngine;

public class PlayerAtkNormal1State : PlayerStateBase
{
    public override void Enter()
    {
        
        _player._PlayerModle.SetRootMotionAction(OnRootMotion);
        StandAttck();
    }

    private void StandAttck()
    {
        //TODO：实现连续普攻
        _player.StartAttack(_player.standAttckCongig[0]);
    }
    public override void Update()
    {
        if (CheckAnimatorStateName(_player.standAttckCongig[0].AnimationName, out float animTime) && animTime >= 0.9f)
        {

            _player.ChangeState(PlayerStateType.Idle);
            return;
        }
    }

    
    private void OnRootMotion(Vector3 deltaPosition, Quaternion deltaRotation)
    {
        deltaPosition.y = _player._gravity * Time.deltaTime;
        _player._CharacterController.Move(deltaPosition);
    }

    public override void Exit()
    {
        _player.PlayAnimation("Idle");
    }
}
