using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectFour.Hubs
{
    public class GameHub : Hub
    {
        public static Dictionary<string, int> quickPlayLobby = new Dictionary<string, int>();

        public void RemoveQuickPlayLobby(string lobbyId)
        {
            quickPlayLobby.Remove(lobbyId);
        }

        public void JoinQuickPlay()
        {
            if (quickPlayLobby.Count==0)
            {
                int playerOne = Random();
                string lobbyId = Convert.ToString(RandomQuickPlay());
                Clients.Caller.SendAsync("AssignPlayer", playerOne);
                Clients.Caller.SendAsync("GetRandomLobbyId", lobbyId);
                quickPlayLobby.Add(lobbyId, playerOne);
                JoinGroup(lobbyId);
            }
            else
            {
                bool availableLobbies = false;
                foreach (KeyValuePair<string, int> entry in quickPlayLobby)
                {
                    if (entry.Value == 1 || entry.Value == 2)
                    {
                        int playerTwo = entry.Value;
                        if (playerTwo == 1)
                        {
                            playerTwo = 2;
                        }
                        else
                        {
                            playerTwo = 1;
                        }
                        JoinGroup(entry.Key);
                        Clients.Caller.SendAsync("AssignPlayer", playerTwo);
                        Clients.Caller.SendAsync("GetRandomLobbyId", entry.Key);
                        Clients.Groups(entry.Key).SendAsync("OpenLobby");
                        quickPlayLobby.Remove(entry.Key);
                        availableLobbies = false;
                        break;
                    }
                    availableLobbies = true;
                }
                if (availableLobbies)
                {
                    int playerOne = Random();
                    string lobbyId = Convert.ToString(RandomQuickPlay());
                    Clients.Caller.SendAsync("AssignPlayer", playerOne);
                    Clients.Caller.SendAsync("GetRandomLobbyId", lobbyId);
                    quickPlayLobby.Add(lobbyId, playerOne);
                    JoinGroup(lobbyId);
                }
            }
        }
        public int RandomQuickPlay()
        {
            int random_number = new Random().Next(100000, 10000001); // Generates a number between 1 to 2
            return random_number;
        }

        public static Dictionary<string, int> lobbies = new Dictionary<string, int>();

        public void RemoveLobby(string lobbyId)
        {
            lobbies.Remove(lobbyId);
        }

        public void CheckLobbies(string lobbyId)
        {
            if (!lobbies.ContainsKey(lobbyId))
            {
                int playerOne = Random();
                lobbies.Add(lobbyId, playerOne);
                JoinGroup(lobbyId);
                Clients.Caller.SendAsync("AssignPlayer", playerOne);
            }
            else
            {
                Clients.Caller.SendAsync("RedirectWithMessage", "Oh! Something went wrong, please try again.");
            }
        }
        public void CheckPlayers(string lobbyId)
        {
            if (!lobbies.ContainsKey(lobbyId))
            {
                Clients.Caller.SendAsync("RedirectWithMessage", "The introduced Lobby does not exist.");
            }
            else if (lobbies[lobbyId] == 1 || lobbies[lobbyId] == 2)
            {
                int playerTwo = lobbies[lobbyId];
                if (playerTwo == 1)
                {
                    playerTwo = 2;
                }
                else
                {
                    playerTwo = 1;
                }
                lobbies[lobbyId] = 3;
                JoinGroup(lobbyId);
                Clients.Caller.SendAsync("AssignPlayer", playerTwo);
                Clients.Groups(lobbyId).SendAsync("OpenLobby");
            }
            else
            {
                Clients.Caller.SendAsync("RedirectWithMessage", "The lobby is already full.");
            }
        }

        public int Random()
        {
            int random_number = new Random().Next(1, 3); // Generates a number between 1 to 2
            return random_number;
        }

        public void JoinGroup(string lobbyId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }

        public async Task SendMove(int columnPosition, string lobbyId)
        {
            await Clients.Groups(lobbyId).SendAsync("ReceiveMove", columnPosition);
        }
        
        

        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        //}
    }
}
