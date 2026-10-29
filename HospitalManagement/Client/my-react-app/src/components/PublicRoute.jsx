import { Navigate } from "react-router-dom";
// protect dashboard
const PublicRoute = ({ children }) => {
  const token = localStorage.getItem("token");

  return token ? <Navigate to="/dashboard" /> : children;
};

export default PublicRoute;