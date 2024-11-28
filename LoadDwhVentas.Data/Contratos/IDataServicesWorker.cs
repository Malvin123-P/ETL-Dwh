using LoadDwhVentas.Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDwhVentas.Data.Contratos
{
    public interface IDataServicesWorker
    {
        Task<OperactionResult> LoadDwh();
    }
}
