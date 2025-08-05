import React, { useState, useEffect } from "react";
import PrivateLayout from "../components/PrivateLayout";

export default function TransferenciaPage() {
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [cuentaOrigenId, setCuentaOrigenId] = useState(null);
  const [cuentaDestinoId, setCuentaDestinoId] = useState("");
  const [monto, setMonto] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");

  const userId = localStorage.getItem("Id");

  useEffect(() => {
    // Obtener cuenta origen del usuario (asumiendo que solo tiene una cuenta por ahora)
    async function obtenerCuenta() {
      try {
        const res = await fetch(`https://localhost:7008/api/Cuenta/Cuenta/${userId}`);
        const data = await res.json();
        if (data.length > 0) {
          setCuentaOrigenId(data[0].id); // asumimos la primera cuenta
        }
      } catch (err) {
        console.error("Error al obtener cuenta:", err);
      }
    }

    obtenerCuenta();
  }, [userId]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMensaje("");
    setError("");

    try {
      const res = await fetch("https://localhost:7008/api/movimientos/Movimientos/transferencia", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          cuentaOrigenId,
          cuentaDestinoId: parseInt(cuentaDestinoId),
          monto: parseFloat(monto),
          descripcion,
        }),
      });

      const data = await res.json();

      if (!res.ok) {
        throw new Error(data?.message || "Error en la transferencia.");
      }

      setMensaje("✅ Transferencia realizada con éxito.");
      setCuentaDestinoId("");
      setMonto("");
      setDescripcion("");
    } catch (err) {
      setError("❌ " + err.message);
    }
  };

  return (
    <PrivateLayout>
      <div className="transferencia-container" style={{ padding: "20px" }}>
        <h2>Transferencias</h2>

        <button
          onClick={() => setMostrarFormulario(!mostrarFormulario)}
          style={{
            padding: "10px 20px",
            backgroundColor: "#007bff",
            color: "#fff",
            border: "none",
            borderRadius: "6px",
            cursor: "pointer",
            marginBottom: "20px",
          }}
        >
          {mostrarFormulario ? "Ocultar Formulario" : "Nueva Transferencia"}
        </button>

        {mostrarFormulario && (
          <form onSubmit={handleSubmit} style={{ maxWidth: "400px" }}>
            <div>
              <label>Cuenta Destino:</label>
              <input
                type="number"
                required
                value={cuentaDestinoId}
                onChange={(e) => setCuentaDestinoId(e.target.value)}
                placeholder="ID de cuenta destino"
                style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
              />
            </div>

            <div>
              <label>Monto:</label>
              <input
                type="number"
                required
                step="0.01"
                value={monto}
                onChange={(e) => setMonto(e.target.value)}
                placeholder="Q"
                style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
              />
            </div>

            <div>
              <label>Descripción:</label>
              <input
                type="text"
                required
                value={descripcion}
                onChange={(e) => setDescripcion(e.target.value)}
                placeholder="Motivo de la transferencia"
                style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
              />
            </div>

            <button
              type="submit"
              style={{
                backgroundColor: "#28a745",
                color: "#fff",
                border: "none",
                padding: "10px 20px",
                borderRadius: "6px",
                cursor: "pointer",
              }}
            >
              Enviar Transferencia
            </button>
          </form>
        )}

        {mensaje && <p style={{ color: "green", marginTop: "20px" }}>{mensaje}</p>}
        {error && <p style={{ color: "red", marginTop: "20px" }}>{error}</p>}
      </div>
    </PrivateLayout>
  );
}
