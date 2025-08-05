import React, { useEffect, useState } from "react";

export default function CuentasPage() {
  const [cuentas, setCuentas] = useState([]);
  const [loading, setLoading] = useState(true);

  // Supón que el id del usuario está en localStorage
  const userId = localStorage.getItem("Id");

  useEffect(() => {
    async function fetchCuentas() {
      try {
        const res = await fetch(`https://localhost:7008/api/Tarjeta/Tarjeta/${userId}`);
        const data = await res.json();
        setCuentas(data);
      } catch (err) {
        setCuentas([]);
      } finally {
        setLoading(false);
      }
    }
    fetchCuentas();
  }, [userId]);

  if (loading) return <p>Cargando tarjetas...</p>;

  return (
    <div>
      <h2>Cuentas</h2>
      {cuentas.length === 0 ? (
        <p>No hay tarjetas asignadas a este usuario.</p>
      ) : (
        <ul>
          {cuentas.map((cuenta) => (
            <li key={cuenta.id}>
              {cuenta.nombre} - Tipo: {cuenta.tipo}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}