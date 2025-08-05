using System.Numerics;

namespace bancaGenesisAPIs.Models
{
    public class Cuenta
    {
        public int id { get; set; }
        public decimal disponible { get; set; }
        public int idCliente { get; set; }
    }
}
