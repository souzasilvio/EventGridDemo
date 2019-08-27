using AppEventGrid.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventWebHookConsumer2.Repositories
{
    public class RepositorioCliente : IRepositorioCliente
    {
        public IConfiguration Configuration { get; }
        private string stringConexao;

        public RepositorioCliente(IConfiguration configuration)
        {
            Configuration = configuration;
            stringConexao = Configuration["ConectionString"];
        }

        public void Salvar(Cliente cliente)
        {
            string sqlInsere = "Insert Into ClienteApp2(Nome, Email) Values(@Nome, @Email)";
            string sqlConsult = "Select Codigo, Nome, Email From ClienteApp2 Where Codigo = @Codigo";
            string sqlUpdate = "Update ClienteApp2 Set Nome = @Nome, Email = @Email, DataModificacao = @DataModificacao Where Codigo = @Codigo";
            using (SqlConnection conexao = new SqlConnection(stringConexao))
            {
                var reg = conexao.QueryFirstOrDefault<Cliente>(sqlConsult, new { Codigo = cliente.Codigo });
                if (reg == null)
                {
                    conexao.ExecuteScalar<Cliente>(sqlInsere, cliente);
                }
                else
                {
                    conexao.ExecuteScalar<Cliente>(sqlUpdate, cliente);
                }
            }
        }



    }
}
