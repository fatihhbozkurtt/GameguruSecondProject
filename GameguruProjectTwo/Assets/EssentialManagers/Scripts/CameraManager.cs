using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoSingleton<CameraManager>
{
    public enum CamType
    {
        Menu, Game, Win, Fail
    }

    public Camera mainCam;
    public CinemachineVirtualCamera menuCam;
    public CinemachineVirtualCamera gameCam;
    public CinemachineVirtualCamera winCam;
    public CinemachineVirtualCamera failCam;
    public ParticleSystem confetti;

    CinemachineVirtualCamera[] vcamArr;

    protected override void Awake()
    {
        base.Awake();

        vcamArr = new CinemachineVirtualCamera[System.Enum.GetNames(typeof(CamType)).Length];

        vcamArr[(int)CamType.Menu] = menuCam;
        vcamArr[(int)CamType.Game] = gameCam;
        vcamArr[(int)CamType.Win] = winCam;
        vcamArr[(int)CamType.Fail] = failCam;
    }

    private void Start()
    {
        GameManager.instance.LevelStartedEvent += () => { SetCam(CamType.Game); };
        GameManager.instance.LevelFailedEvent += () => { SetCam(CamType.Fail); };

        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnCharArrivedFinish;
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
    }

    private void OnNextLevelStarted()
    {
        winCam.transform.SetParent(transform);
        SetCam(CamType.Game);
    }

    private void OnCharArrivedFinish()
    {
        Transform rotater = ConstantRotater.instance.transform;
        winCam.transform.position = new Vector3(rotater.position.x, 10, rotater.position.z - 10);
        winCam.transform.SetParent(ConstantRotater.instance.transform);
    }

    public void SetCam(CamType camType)
    {
        for (int i = 0; i < vcamArr.Length; i++)
        {
            if (i == (int)camType)
            {
                vcamArr[i].Priority = 50;
            }

            else
            {
                vcamArr[i].Priority = 0;
            }
        }
    }

    public CinemachineVirtualCamera GetCam(CamType camType)
    {
        return vcamArr[(int)camType];
    }
}
