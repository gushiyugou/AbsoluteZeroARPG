using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAtkNormal1State : PlayerStateBase
{
    public override void Enter()
    {
        
        _player._PlayerModle.SetRootMotionAction(OnRootMotion);
        _player.PlayAnimation(_player._skillConfig.AnimationName);
    }

    public override void Update()
    {
        if(CheckAnimatorStateName(_player._skillConfig.AnimationName,out float animTime) && animTime >= 0.9f)
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
