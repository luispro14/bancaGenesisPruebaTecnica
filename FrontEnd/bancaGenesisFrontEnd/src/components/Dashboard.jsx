import React from "react";
import "../styles/Dashboard.css";

export default function Dashboard() {
  return (
    <div>
      <h1 style={{ color: "#e76f51", marginBottom: "1.5rem" }}>
        Bienvenido al Panel de Control 👋
      </h1>
      <div style={{ display: "flex", gap: "2rem", flexWrap: "wrap" }}>
        <div className="dashboard-card">
          <h2>Disponible Total</h2>
          <p>Q 1,250</p>
        </div>
        <div className="dashboard-card">
          <h2>Tarjetas Activas</h2>
          <p>58</p>
        </div>
      </div>
      <p style={{ marginTop: "2rem", color: "#555" }}>
        Genesis Empresarial, tu banca de confianza.
      </p>
    </div>
  );
}