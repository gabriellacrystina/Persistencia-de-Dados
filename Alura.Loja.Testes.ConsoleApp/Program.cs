using Alura.Loja.Testes.ConsoleApp.Utils;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfiguration configuration = new AppConfiguration();
            
            GravarUsandoAdoNet(configuration);
        }

        private static void GravarUsandoAdoNet(AppConfiguration configuration)
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAO(configuration))
            {
                repo.Adicionar(p);
            }
        }
    }
}
