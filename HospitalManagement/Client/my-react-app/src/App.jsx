import { useEffect, useState } from "react";
import Register from "./components/pages/Register";
import Dashboard from "./components/pages/Dashboard";
import Login from "./components/pages/Login";
import { Routes, Route, Navigate, useNavigate } from "react-router-dom";
import AuthRedirect from "./components/AuthRedirect";
import PrivateRoute from "./components/PrivateRoute";
import PublicRoute from "./components/PublicRoute";

export default function App() {

  const navigate = useNavigate();
  const [token, setToken] = useState(null);
  const [user, setUser] = useState({
    name: ""
  });


  // restore token on refresh
  useEffect(() => {
    const savedToken = localStorage.getItem("token");

    if (savedToken) {
      setToken(savedToken);
      navigate("/dashboard");    
    }
  }, []);


  return (
    <>
      <div>

        <Routes>
          <Route path="/" element={<AuthRedirect />} />
           <Route path="/" element={<Navigate to="/Register" />} /> 
          <Route path="login" element={
            <PublicRoute>
              <Login />
            </PublicRoute>
          } />        
          <Route path="/register" element={
            <PublicRoute>
              <Register />
            </PublicRoute>
          } />
          <Route path="/dashboard" element={
            <PrivateRoute>
              <Dashboard />
            </PrivateRoute>
          } />
        </Routes>

      </div>
    </>
  );
}

