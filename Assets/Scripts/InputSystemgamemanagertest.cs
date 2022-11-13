using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputSystemgamemanagertest : MonoBehaviour
{
    public List<PlayerInput> playerlist = new List<PlayerInput>();
    GameObject SpawnPoint;
    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leftAction;
    // Start is called before the first frame update
    public event System.Action<PlayerInput> playerjoinedGame;
    public event System.Action<PlayerInput> playerleftGame;


    float fCanJoinTime = 1.0f;
    bool isMainScnene;
    bool iseventset;

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        Sceneinit();

    }
    private void Update()
    {
        if (fCanJoinTime >= 0)
            fCanJoinTime -= Time.deltaTime;
    }
    //private void OnDestroy()
    //{

    //    joinAction.performed -= context => JoinAction(context);
    //    joinAction.Disable();

    //    leftAction.performed -= context => LeftAction(context);
    //    leftAction.Disable();
    //}
    private void Sceneinit()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            isMainScnene = true;
            GetComponent<InputSystemgamemanagertest>().enabled = true;
            GetComponent<PlayerInputManager>().enabled = true;
            if (iseventset == false)
            {
                joinAction.Enable();
                joinAction.performed += context => JoinAction(context);

                leftAction.Enable();
                leftAction.performed += context => LeftAction(context);
                iseventset = true;
            }
        }
        else
        {
            isMainScnene = false;
            GetComponent<InputSystemgamemanagertest>().enabled = false;
            GetComponent<PlayerInputManager>().enabled = false;
            if (iseventset == true)
            {
                joinAction.performed -= context => JoinAction(context);
                joinAction.Disable();

                leftAction.performed -= context => LeftAction(context);
                leftAction.Disable();
                iseventset = false;
            }
        }
    }
    void Start()
    {
            Sceneinit();
        SceneManager.sceneLoaded += LoadedsceneEvent;
        SpawnPoint = GameObject.Find("SpawnPoints");
            joinAction.Enable();
            joinAction.performed += context => JoinAction(context);

            leftAction.Enable();
            leftAction.performed += context => LeftAction(context);
        iseventset = true;

    }
    void JoinAction(InputAction.CallbackContext context)
    {
        if (isMainScnene == false) return;
        if (fCanJoinTime < 0)
        {
            PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
            fCanJoinTime = 0.5f;
        }
    }
    public void DeleatAllDevide()
    {
        foreach (var player in playerlist)
        {
            UnregisterPlayer(player);
        }
    }
    void LeftAction(InputAction.CallbackContext context)
    {
        if (isMainScnene == false) return;
        if (playerlist.Count > 0)
        {
            foreach (var player in playerlist)
            {
                if (player.GetComponentInChildren<LobiInfomation>().IsReady == false)
                {
                    foreach (var device in player.devices)
                    {
                        if (device != null && context.control.device == device)
                        {
                            player.GetComponentInChildren<LobiInfomation>().TextOff();
                            UnregisterPlayer(player);
                            return;
                        }
                    }
                }
            }
        }
    }
    void UnregisterPlayer(PlayerInput _playerInput)
    {
        if (isMainScnene == false) return;
        playerlist.Remove(_playerInput);
        if (playerleftGame != null)
        {
            playerleftGame(_playerInput);
        }
        Destroy(_playerInput.transform.gameObject);
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (isMainScnene == false) return;
        if (fCanJoinTime < 0)
        {
            fCanJoinTime = 0.5f;
            CharaterManager.instance.MaxPlayerIndex++;
            playerlist.Add(playerInput);
            playerInput.gameObject.transform.parent =CharaterManager.instance.transform;
            playerInput.gameObject.transform.position = SpawnPoint.transform.GetChild(CharaterManager.instance.MaxPlayerIndex - 1).transform.position;
            playerInput.gameObject.transform.rotation = SpawnPoint.transform.GetChild(CharaterManager.instance.MaxPlayerIndex - 1).transform.rotation;
            if (playerjoinedGame != null)
            {
                playerjoinedGame(playerInput);
            }
        }
    }
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (isMainScnene == false) return;
        CharaterManager.instance.MaxPlayerIndex--;
        playerlist.Remove(playerInput);
    }
}
