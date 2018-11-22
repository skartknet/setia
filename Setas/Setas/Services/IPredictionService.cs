using System.Threading.Tasks;
using Setas.Models;

namespace Setas.Services
{
    public interface IPredictionService
    {
        Task<PredictionResponse> Analyse(byte[] byteData);
    }
}