using AppEventGrid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWebHookConsumer2.Repositories
{
    public interface IRepositorioCliente
    {
        void Salvar(Cliente cliente);
    }
}
