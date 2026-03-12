import { Navigate } from "react-router-dom";

const AuthRedirect = () => {
  const token = localStorage.getItem("token");

  return token ? <Navigate to="/dashboard" /> : <Navigate to="/login" />;
};

export default AuthRedirect;