using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ABB.Web.Hubs
{
    public class ApplicationHub : Hub
    {
        public async Task SendPengajuanAkseptasiNotification(string userId, string nomor_pengajuan, string status)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("PengajuanAkseptasiNotification", userId, nomor_pengajuan, status) ;
        }
    }
}