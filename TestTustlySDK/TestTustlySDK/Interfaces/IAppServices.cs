using System.Threading.Tasks;

namespace TestTustlySDK.Interfaces
{
    public interface IAppServices
    {
        Task<string> OpenGatewayPage(string url);
    }
}
