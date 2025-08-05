namespace bancaGenesisAPIs.Models
{
    public class Movimiento
    {
        public int id { get; set; }
        public int idCuenta   { get; set; }
        public int idTarjeta { get; set; }
        public decimal monto { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set;}
    }
}
