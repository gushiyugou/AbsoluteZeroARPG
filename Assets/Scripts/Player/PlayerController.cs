using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerController : MonoBehaviour,IStateMachineOwner,ISkillOwner
{
    [SerializeField]private PlayerModle _playerModle;
    public PlayerModle _PlayerModle 
    { 
        get => _playerModle;  
    }

    [SerializeField]private CharacterController _characterController;
    public CharacterController _CharacterController{ get => _characterController; }

    private StateMachine _stateMachine;
    [SerializeField]private AudioSource _audioSource;

    //TODO 测试的配置信息，直接拖拽，后续会改
    public SkillConfig _skillConfig;
    public List<string> enemyTagList;

    #region 配置信息
    public readonly float _gravity = -9.8f;
    public float _rotationSpeed = 2f;
    public float walkToRunTransition = 1f;
    public float walkSpeed = 1f;
    public float RunSpeed = 1f;

    public float jumpStartSpeed = 1f;
    public float moveSpeedForJump = 2f;
    public float moveSpeedForAirDown = 2f;
    public AudioClip[] footStepAudioClips;

    #endregion
    //拖尾组件
    [SerializeField] private MeleeWeaponTrail weaponTrail;




    private void Awake()
    {
        //_playerModle = GetComponentWi<PlayerModle>();
        _PlayerModle.OnInit(this, enemyTagList);
        _stateMachine = new StateMachine();
        _stateMachine.Init(this);
        _characterController = GetComponent<CharacterController>();
        
    }

    private void Start()
    {
        ChangeState(PlayerStateType.Idle);
        
    }

    //public void Update()
    //{
    //    Debug.Log(_stateMachine.CurrentStateType);
    //}
    public void ChangeState(PlayerStateType needState)
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
            
        //}
        
        switch (needState)
        {
            case PlayerStateType.Idle:
                _stateMachine.ChangeState<PlayerIdleState>();
                break;
            case PlayerStateType.Moevment:
                _stateMachine.ChangeState<PlayerMoveState>();
                break;
            case PlayerStateType.Jump:
                _stateMachine.ChangeState<PlayerJumpState>();
                break;
            case PlayerStateType.AirDown:
                _stateMachine.ChangeState<PlayerAirDownState>();
                break;
            case PlayerStateType.Sidestep:
                _stateMachine.ChangeState<PlayerSideStepState>();
                break;
            case PlayerStateType.SidestepReverse:
                _stateMachine.ChangeState<PlayerSidestepReverseState>();
                break;
            case PlayerStateType.AtkNormal1:
                _stateMachine.ChangeState<PlayerAtkNormal1State>();
                break;
        }
    }

    public void PlayAnimation(string animationName,float fixedTransitionDuration = 0.25f)
    {
        _playerModle._Animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
        
    }

    public void PlayAnimationImmediately(string animationName,int layer,int num)
    {
        _PlayerModle._Animator.Play(animationName, layer,num);
    }


    /// <summary>
    /// 检查当前是否正在播放指定动画
    /// </summary>
    public bool IsPlayingAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = _PlayerModle._Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }


    /// <summary>
    /// 强制动画过渡
    /// </summary>
    public void ForceAnimationTransition(string targetAnimation, float transitionDuration = 0.1f)
    {
        _PlayerModle._Animator.CrossFade(targetAnimation, transitionDuration);
    }

    private void OnAnimatorMove()
    {
        // 应用根运动位移
        _characterController.Move(_playerModle._Animator.deltaPosition);
        transform.rotation *= _playerModle._Animator.deltaRotation;
        
    }

    public void OnFootStep()
    {
        //_audioSource.PlayOneShot(footStepAudioClips[Random.Range(0, footStepAudioClips.Length)]);
    }

    public void OnJumpLoopComplete()
    {
        if (_stateMachine.CurrentStateType == typeof(PlayerAirDownState))
        {
            // 动画完成后立即检查是否需要切换状态
            if (_CharacterController.isGrounded)
            {
                ChangeState(PlayerStateType.Idle);
            }
        }
    }

    public void StartSkillHit(int weaponIndex)
    {
        weaponTrail.Emit = true;
    }

    public void StopSkillHit(int weaponIndex)
    {
        weaponTrail.Emit = false;
    }

    public void SkillCanSwitch()
    {
        
    }

    public void OnHit(IHurt target, Vector3 hitPosition)
    {
        Debug.Log("角色控制：我攻击到了" + ((Component)target).gameObject.name);
    }
}
