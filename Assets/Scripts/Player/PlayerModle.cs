using System;
using UnityEngine;

public class PlayerModle : MonoBehaviour
{
    private Animator _animator;
    public Animator _Animator { get { return _animator; } }
    private Action<Vector3, Quaternion> rootMotionAction;
    //private CharacterController _controller;

    public void OnInit(Action footStepAction)
    {
        this.footStepAction = footStepAction;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //_controller = GetComponent<CharacterController>();
    }
    public void SetRootMotionAction(Action<Vector3, Quaternion> rootMotionAction)
    {
        this.rootMotionAction = rootMotionAction;
    }

    public void ClearRootMotionAction()
    {
        rootMotionAction = null;
    }

    


    #region 根运动
    private void OnAnimatorMove()
    {
        this.rootMotionAction?.Invoke(_animator.deltaPosition,_animator.deltaRotation);
        //_controller.Move(_animator.deltaPosition);

    }
    #endregion



    #region 动画事件

    private Action footStepAction;
    private Action<string> runStopAction;
    //private void FootStep()
    //{
    //    footStepAction?.Invoke();
    //}

    //private void JumpAudio()
    //{
    //    jumpAction?.Invoke();
    //}



    #endregion
}
