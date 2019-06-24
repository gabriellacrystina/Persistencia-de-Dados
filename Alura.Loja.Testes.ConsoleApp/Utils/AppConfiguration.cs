using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Alura.Loja.Testes.ConsoleApp.Utils
{
    [ExcludeFromCodeCoverage]
    public class AppConfiguration
    {
        public string OracleConnection { get => ConfigurationManager.AppSettings["OracleConnectionString"]; }
    }
}
