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

    bool[] isgmaobjectactive = new bool[4];

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
            fCanJoinTime = 0.5f;
            CharaterManager.instance.ResetScore();
            SpawnPoint = GameObject.Find("SpawnPoints");
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
          // //while (playerlist.Count > 0)
          // {
          //     print(playerlist.Count);
          //     DeleatAllDevide();
          //     print(playerlist.Count);
          // }
        }
        else
        {
            isMainScnene = false;
            GetComponent<InputSystemgamemanagertest>().enabled = false;
            GetComponent<PlayerInputManager>().enabled = false;
          //  if (iseventset == true)
          //  {
          //      joinAction.performed -= context => JoinAction(context);
          //      joinAction.Disable();
          //
          //      leftAction.performed -= context => LeftAction(context);
          //      leftAction.Disable();
          //      iseventset = false;
          //  }
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;
        Sceneinit();
        for (int i = 0; i < 4; i++)
            isgmaobjectactive[i] = false;

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
                 if (player != null)
                 {
                     foreach (var device in player.devices)
                     {
                         if (device != null)
                         {
                             player.GetComponentInChildren<LobiInfomation>().TextOff();
                             CharaterManager.instance.MaxPlayerIndex--;
                             playerlist.Remove(player);
                             Destroy(player.transform.gameObject);
                         }
                     }
                 }
            }
    }
    void LeftAction(InputAction.CallbackContext context)
    {
        if (isMainScnene == false) return;
        if (fCanJoinTime < 0)
        {
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
                                fCanJoinTime = 0.5f;
                                return;
                            }
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
            for (int i = 0; i < 4; i++)
            {
                if (isgmaobjectactive[i] == false)
                {
                    print(i.ToString() + "Asd");

                    Playerinput clone =  playerInput.GetComponentInChildren<Playerinput>();
                    clone.SetPlayerInDex(i+1);
                    playerlist.Add(playerInput);
                    playerInput.gameObject.transform.parent = CharaterManager.instance.transform;
                    if (SpawnPoint == null)
                        return;
                    playerInput.gameObject.transform.position = SpawnPoint.transform.GetChild(clone.GetPlayerInDex()-1).transform.position;
                    playerInput.gameObject.transform.rotation = SpawnPoint.transform.GetChild(clone.GetPlayerInDex() - 1).transform.rotation;
                    if (playerjoinedGame != null)
                    {
                        playerjoinedGame(playerInput);
                    }
                    isgmaobjectactive[i] = true;

                    CharaterManager.instance.MaxPlayerIndex++;
                    return;
                }
            }
        }
    }
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (isMainScnene == false) return;

        if (fCanJoinTime < 0)
        {
            int index = playerInput.GetComponentInChildren<Playerinput>().GetPlayerInDex();
            CharaterManager.instance.PlayerCharacterIndex[index-1] = -9;
            isgmaobjectactive[index - 1] = false;
            print(index - 1);
            CharaterManager.instance.MaxPlayerIndex--;
            playerlist.Remove(playerInput);
            fCanJoinTime = 0.5f;

        }
    }
}
