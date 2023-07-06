using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Login
    {

        public static async void HandleUserLogin(LoginMessage login, Client client)
        {
            var _loginAck = new { succes = true, id = login.UserId };
            string _body = JSONHelper.Serialize(_loginAck);
            client.userID = _loginAck.id;
            Console.WriteLine("GOT HERE LOGIN " + login.UserId);
            await GameManager.instance.CreateSession(client, login);

            Console.WriteLine("GOT HERE LOGIN, I PASS");

            Console.WriteLine("Successfully logged in to playfab");
            

            Datagram _datagram = new Datagram(EventType.Login, _body);
            client.SendDataGram(_datagram);

            return;
        }
    }
}
