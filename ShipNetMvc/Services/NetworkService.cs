using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ShipNetMvc.Services;

public interface INetworkService
{
    string ObtenerMacCliente(HttpContext context);
}

public class NetworkService : INetworkService
{
    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern int SendARP(int destIp, int srcIp, byte[] macAddr, ref int phyAddrLen);

    public string ObtenerMacCliente(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress;

        // Si es nulo o localhost (127.0.0.1 o ::1), obtener la MAC de la interfaz de red activa de la máquina
        if (remoteIp == null || IPAddress.IsLoopback(remoteIp))
        {
            try
            {
                var nic = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                                n.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                n.GetPhysicalAddress().GetAddressBytes().Length == 6)
                    .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    .FirstOrDefault();

                if (nic != null)
                {
                    var bytes = nic.GetPhysicalAddress().GetAddressBytes();
                    return string.Join(":", bytes.Select(b => b.ToString("X2")));
                }
            }
            catch
            {
                // Fallback silencioso
            }
            return "00:00:00:00:00:00";
        }

        // Si es una IP mapeada a IPv6 (como ::ffff:192.168.0.x)
        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        if (remoteIp.AddressFamily == AddressFamily.InterNetwork)
        {
            try
            {
                byte[] macBytes = new byte[6];
                int len = macBytes.Length;
                byte[] ipBytes = remoteIp.GetAddressBytes();
                int destIp = BitConverter.ToInt32(ipBytes, 0);

                int result = SendARP(destIp, 0, macBytes, ref len);
                if (result == 0 && len == 6)
                {
                    return string.Join(":", macBytes.Select(b => b.ToString("X2")));
                }
            }
            catch
            {
                // Si falla SendARP por alguna razón de permisos/plataforma
            }
        }

        // Si no se pudo resolver, devolvemos la IP o fallback 00:00:00:00:00:00
        return "00:00:00:00:00:00";
    }
}
