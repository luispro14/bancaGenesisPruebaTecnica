import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./pages/login";
import DashboardPage from "./pages/DashboardPage";
import CuentasPage from "./pages/CuentasPage"; 
import TarjetasPage from "./pages/TarjetasPage"; 
import PrivateRoute from "./components/PrivateRoute";
import TransferenciaPage from "./components/TransferenciaPage"; // Importa el componente


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/dashboard" element={
          <PrivateRoute>
            <DashboardPage />
          </PrivateRoute>
        } />
        <Route path="/cuentas" element={
          <PrivateRoute>
            <CuentasPage />
          </PrivateRoute>
        } />
        <Route path="/tarjetas" element={
          <PrivateRoute>
            <TarjetasPage />
          </PrivateRoute>
        } />
        <Route path="/transferencias" element={
          <PrivateRoute>
            <TransferenciaPage />
          </PrivateRoute>
        } />
      </Routes>
    </Router>
  );
}


export default App;