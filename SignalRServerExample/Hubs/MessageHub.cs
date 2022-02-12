using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServerExample.Hubs
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("getConnectionId", Context.ConnectionId);
        }

        public async Task SendMEssageAsync(string message, IEnumerable<string> connectionIds, string groupName, IEnumerable<string> groups)
        {
            #region Caller
            //sadece server'a bildirim gönderen client
            await Clients.Caller.SendAsync("receiveMessage", message);

            #endregion

            #region All
            //server'a bağlı olan tüm clientler
            await Clients.All.SendAsync("receiveMessage", message);

            #endregion

            #region Other
            //sadece server'a bildirim gönderen client hariç server'a bağlı olan tüm clientler
            await Clients.Others.SendAsync("receiveMessage", message);

            #endregion

            #region Client Methodlari

            #region AllExcept
            //belirtilen clientlar hariç server'a bağlı tüm clientlar
            await Clients.AllExcept(connectionIds).SendAsync("receiveMessage", message);

            #endregion

            #region Client
            //mevcuttaki clientlardan herhangi birine mesaj atmak istiyorsa
            await Clients.Client(connectionIds.FirstOrDefault()).SendAsync("receiveMessage", message);

            #endregion

            #region Clients
            //server'a bağlı olan clientlar arasında belirtilenlere mesaj yollar
            await Clients.Clients(connectionIds).SendAsync("receiveMessage", message);

            #endregion

            #region Group
            //belirtilen gruptaki tüm clientlara bildiride bulunur
            //önce gruplar oluşturulmalı sonrasında subscribe olmalı
            await Clients.Group(groupName).SendAsync("receiveMessage", message);

            #endregion

            #region GroupExcept
            //belirtilen gruptaki, belirtilen clientlar hariç tüm clientlara mesaj yollar
            await Clients.GroupExcept(groupName, connectionIds).SendAsync("receiveMessage", message);//tek connectiona da gödnerir. overload ile

            #endregion

            #region Groups
            //birden çok gruptaki tüm clientlara bildiride bulunur
            await Clients.Groups(groups).SendAsync("receiveMessage", message);

            #endregion

            #region OthersInGroup
            //bildiride bulunan client haricinde gruptaki tüm clientlara bildiride bulunur
            await Clients.OthersInGroup(groupName).SendAsync("receiveMessage", message);

            #endregion

            #region User

            #endregion

            #region Users

            #endregion

            #endregion
        }

        public async Task AddGroup(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }
    }
}
