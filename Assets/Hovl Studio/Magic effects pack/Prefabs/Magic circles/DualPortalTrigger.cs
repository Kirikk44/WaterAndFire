using UnityEngine.SceneManagement;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public string nextSceneName;

    private bool isPlayer1InPortal1 = false;
    private bool isPlayer2InPortal2 = false;

    public void OnPlayerEntered(PortalTrigger.PortalID portalId, GameObject player)
    {
        if (portalId == PortalTrigger.PortalID.Portal1 && player == player1)
        {
            isPlayer1InPortal1 = true;
            Debug.Log("Player1 вошёл в Portal1");
        }

        if (portalId == PortalTrigger.PortalID.Portal2 && player == player2)
        {
            isPlayer2InPortal2 = true;
            Debug.Log("Player2 вошёл в Portal2");
        }

        CheckSceneTransition();
    }

    public void OnPlayerExited(PortalTrigger.PortalID portalId, GameObject player)
    {
        if (portalId == PortalTrigger.PortalID.Portal1 && player == player1)
            isPlayer1InPortal1 = false;

        if (portalId == PortalTrigger.PortalID.Portal2 && player == player2)
            isPlayer2InPortal2 = false;
    }

    private void CheckSceneTransition()
    {
        if (isPlayer1InPortal1 && isPlayer2InPortal2)
        {
            Debug.Log("Оба игрока на своих порталах. Загружается следующая сцена...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
