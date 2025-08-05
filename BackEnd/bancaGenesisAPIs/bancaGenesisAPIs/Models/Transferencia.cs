namespace bancaGenesisAPIs.Models
{
    public class Transferencia
    {
        public int CuentaOrigenId { get; set; }
        public int CuentaDestinoId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }

}
