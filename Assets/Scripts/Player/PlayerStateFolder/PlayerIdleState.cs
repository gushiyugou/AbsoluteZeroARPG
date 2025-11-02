using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static System.TimeZoneInfo;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerIdleState : PlayerStateBase
{
    private float velocity;
    Vector3 verticalDisplancement;
    private bool isTransitioningFromAir = false;
    private float transitionTimer = 0f;
    private const float TRANSITION_DURATION = 0.2f; // 过渡时间
    private LayerMask groundLayerMask = LayerMask.GetMask("Env");
    public override void Enter()
    {
        Debug.Log("Idle");
        //SyncModelToController();
        //_player._PlayerModle._Animator.applyRootMotion = false;
        //进入Idle状态时,播放Idle动画
        //_player.PlayAnimation("Idle");
        _player.PlayAnimationImmediately("Idle");
        _player._CharacterController.detectCollisions = false;
        velocity = 0f;
    }

    public override void Update()
    {
        Debug.Log("Idle状态");
        //UpdataGravity();
        if (_player._CharacterController.isGrounded)
        {
            velocity = -0.5f;
        }
        else
        {
            velocity -= _player._gravity * Time.deltaTime;
            velocity = Mathf.Max(velocity, -20f);
        }
        
        //TODO:检测攻击

        //TODO:检测跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPower = 0;
            _player.ChangeState(PlayerStateType.Jump);
            return;
        }
        //检测玩家移动
        _player._CharacterController.Move(new Vector3(0, velocity * Time.deltaTime, 0));
        bool isCast = Physics.CheckSphere(_player.transform.position, 0.2f, groundLayerMask);
        if (!_player._CharacterController.isGrounded && !isCast)
        {
            Debug.Log("idle检测的isGround：" + _player._CharacterController.isGrounded);
            _player.ChangeState(PlayerStateType.AirDown);
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal != 0f || vertical != 0f)
        {
            _player.ChangeState(PlayerStateType.Moevment);
            return;
        }
    }

    public override void Exit()
    {
        _player._PlayerModle._Animator.applyRootMotion = true;
        _player._CharacterController.detectCollisions = true;
        //SyncModelToController();
    }
}
