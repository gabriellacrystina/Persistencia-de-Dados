using Alura.Loja.Testes.ConsoleApp.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Alura.Loja.Testes.ConsoleApp
{
    internal class ProdutoDAO : IDisposable
    {
        private OracleConnection conexao;
        private string insertProdutos;

        public ProdutoDAO(AppConfiguration configuration)
        {
            conexao = new OracleConnection(configuration.OracleConnection);
            conexao.Open();
            FillComandText();
        }

        private void FillComandText()
        {
            insertProdutos = "INSERT INTO Produtos(Id, Nome, Categoria, Preco) VALUES(prod_seq.nextval, :nome, :categoria, :preco)";
        }

        public void Dispose() 
        {
            conexao.Close();
        }

        internal void Adicionar(Produto p)
        {
            using (OracleCommand cmd = new OracleCommand(insertProdutos, conexao))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.Add("nome", p.Nome);
                cmd.Parameters.Add("categoria", p.Categoria);
                cmd.Parameters.Add("preco", p.Preco);

                try { cmd.ExecuteNonQuery(); }
                catch (Exception) { throw ; }
            }
        }

        internal void Atualizar(Produto p)
        {
            try
            {
                OracleCommand updateCmd = conexao.CreateCommand();
                updateCmd.CommandText = "UPDATE Produtos SET Nome = @nome, Categoria = @categoria, Preco = @preco WHERE Id = @id";

                OracleParameter paramNome = new OracleParameter("nome", p.Nome);
                OracleParameter paramCategoria = new OracleParameter("categoria", p.Categoria);
                OracleParameter paramPreco = new OracleParameter("preco", p.Preco);
                OracleParameter paramId = new OracleParameter("id", p.Id);
                updateCmd.Parameters.Add(paramNome);
                updateCmd.Parameters.Add(paramCategoria);
                updateCmd.Parameters.Add(paramPreco);
                updateCmd.Parameters.Add(paramId);

                updateCmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal void Remover(Produto p)
        {
            try
            {
                OracleCommand deleteCmd = conexao.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Produtos WHERE Id = @id";

                SqlParameter paramId = new SqlParameter("id", p.Id);
                deleteCmd.Parameters.Add(paramId);

                deleteCmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal IList<Produto> Produtos()
        {
            List<Produto> lista = new List<Produto>();

            OracleCommand selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Produtos";

            OracleDataReader resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                Produto p = new Produto
                {
                    Id = Convert.ToInt32(resultado["Id"]),
                    Nome = Convert.ToString(resultado["Nome"]),
                    Categoria = Convert.ToString(resultado["Categoria"]),
                    Preco = Convert.ToDouble(resultado["Preco"])
                };
                lista.Add(p);
            }
            resultado.Close();

            return lista;
        }
    }
}