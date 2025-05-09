using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class RelayManager : MonoBehaviour
{
   [SerializeField]private TMP_Text m_Text;
    [SerializeField]private TMP_InputField m_InputField;
    [SerializeField] private TMP_Text count;
    private int playerCount;

    private int maxConnection = 2;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void StartRelay()
    {
        string joinCode = await StartHostwithRelay();
        m_Text.text = joinCode;
    }

    private async Task<string> StartHostwithRelay(int maxConnection = 3)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));
        playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        count.text = "Players Connected: " + playerCount;
        string joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return NetworkManager.Singleton.StartHost() ? joincode : null;
    }

    public async void JoinRelay()
    {
        await StartclientwithRelay(m_InputField.text);
    }

    private async Task<bool>StartclientwithRelay(string joincode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joincode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "dtls"));
        playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        count.text = "Players Connected: " + playerCount;
        return !string.IsNullOrEmpty(joincode) && NetworkManager.Singleton.StartClient();
    }
}
