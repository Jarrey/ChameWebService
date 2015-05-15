using Chame.WebService.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper.Services
{
    public interface IChameService : IDisposable
    {
        string ServiceName { get; }
        Task<IChameResponse> ProcessImage(string request);

        Task<IChameResponse> GetCache();

        Task<IChameResponse> CleanCache(string id);
    }
}
