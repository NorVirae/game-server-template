


using PlayFab;
using PlayFab.MultiplayerModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server.Playfab
{

    public class PlayfabManager
    {
        public enum FriendIdType { PlayFabId, Username, Email, DisplayName };



        public PlayfabManager(string titleId) {
            PlayFabSettings.staticSettings.TitleId = titleId;
        }
        public Task<PlayFabResult<PlayFab.ClientModels.LoginResult>> LoginPlayfab(string customId)
        {
             // Please change this value to your own titleId from PlayFab Game Manager

            var request = new PlayFab.ClientModels.LoginWithCustomIDRequest { CustomId = customId, CreateAccount = true };
            Task<PlayFabResult<PlayFab.ClientModels.LoginResult>> loginTask = PlayFabClientAPI.LoginWithCustomIDAsync(request);
            // If you want a synchronous result, you can call loginTask.Wait() - Note, this will halt the program until the function returns

         

            Console.WriteLine("Done! Press any key to close playfabId: "+ loginTask.Result.Result.PlayFabId);
            //Console.ReadKey(); // This halts the program and waits for the user

            return loginTask;
        }


        private void OnLoginComplete(Task<PlayFabResult<PlayFab.ClientModels.LoginResult>> taskResult)
        {
            var apiError = taskResult.Result.Error;
            var apiResult = taskResult.Result.Result;

            if (apiError != null)
            {
                Console.ForegroundColor = ConsoleColor.Red; // Make the error more visible
                Console.WriteLine("Something went wrong with your first API call.  :(");
                Console.WriteLine("Here's some debug information:");
                Console.WriteLine(PlayFabUtil.GenerateErrorReport(apiError));
                Console.ForegroundColor = ConsoleColor.Gray; // Reset to normal
            }
            else if (apiResult != null)
            {
                Console.WriteLine("Congratulations, you made your first successful API call!");
            }
        }

        public async Task<PlayFabResult<PlayFab.ClientModels.AddFriendResult>> AddFriend(FriendIdType idType, string friendId)
        {
            try
            {
                var request = new PlayFab.ClientModels.AddFriendRequest();
                switch (idType)
                {
                    case FriendIdType.PlayFabId:
                        request.FriendPlayFabId = friendId;
                        break;
                    case FriendIdType.Username:
                        request.FriendUsername = friendId;
                        break;
                    case FriendIdType.Email:
                        request.FriendEmail = friendId;
                        break;
                    case FriendIdType.DisplayName:
                        request.FriendTitleDisplayName = friendId;
                        break;
                }
                // Execute request and update friends when we are done
                PlayFabResult<PlayFab.ClientModels.AddFriendResult> result = await PlayFabClientAPI.AddFriendAsync(request);
                if (result != null && result.Result != null)
                {
                    Console.WriteLine($"result: {result.Result.Created} ADDFRIEND CALL");

                }
                return result;

            }
            catch (PlayFabException e)
            {
                Console.WriteLine(e.Message + " ERROR");
                return new PlayFabResult<PlayFab.ClientModels.AddFriendResult>();
            }
            
        }


       public void RemoveFriend(PlayFab.ClientModels.FriendInfo friendInfo)
        {
           Task<PlayFabResult<PlayFab.ClientModels.RemoveFriendResult>> result =
            PlayFabClientAPI.RemoveFriendAsync(new PlayFab.ClientModels.RemoveFriendRequest
            {
                FriendPlayFabId = friendInfo.FriendPlayFabId
            });
        }

        public Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> GetPlayerPublicData(string playfabId, params string[] key)
        {
            try
            {
                var request = new PlayFab.AdminModels.GetUserDataRequest { PlayFabId = playfabId };
                Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> result = PlayFabAdminAPI.GetUserDataAsync(request);

                return result;
            }
            catch (PlayFabException e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult(new PlayFabResult<PlayFab.AdminModels.GetUserDataResult>());
            }
            
        }

        public void SetPlayerPublicData( Dictionary<string, string> data, string playFabId)
        {
            var request = new PlayFab.AdminModels.UpdateUserDataRequest { Data = data, PlayFabId = playFabId };
            Task<PlayFabResult<PlayFab.AdminModels.UpdateUserDataResult>> result = PlayFabAdminAPI.UpdateUserDataAsync(request);
            if (result.Result != null && result.Result.Result != null)
            {
                Console.WriteLine(result.Result.Result.DataVersion);

            }
        }

    }
}