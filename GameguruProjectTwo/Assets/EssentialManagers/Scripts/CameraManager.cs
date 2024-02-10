using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public enum CamType
    {
        Menu, Game, Win, Fail
    }

    [Header("References")]
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
        GameManager.instance.LevelSuccessEvent += () => { SetCam(CamType.Win); };

        CharacterInteractionController.instance.ArrivedToTheFinishEvent += OnCharArrivedFinish;
        GameManager.instance.NextLevelStartedEvent += OnNextLevelStarted;
    }

    private void OnNextLevelStarted()
    {
        SetCam(CamType.Game);
        winCam.transform.SetParent(transform);
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
       // winCam.transform.localRotation = Quaternion.Euler(45, 0, 0);
    }

    public CinemachineVirtualCamera GetCam(CamType camType)
    {
        return vcamArr[(int)camType];
    }
}
