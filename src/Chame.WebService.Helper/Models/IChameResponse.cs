using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper.Models
{
    public interface IChameResponse
    {
        HttpContent Content { get; }
        string MimeType { get; }

        HttpStatusCode Status { get; }
    }
}
