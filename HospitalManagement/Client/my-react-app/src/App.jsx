import { useEffect, useState } from "react";


import Home from "./components/Home";
import Login from "./components/Login";
import Register from "./components/Register";
import { BrowserRouter ,Routes,Route } from "react-router-dom";

export default function App() {
  const [token, setToken] = useState(null);
const [page, setPage] = useState("login");


// restore token on refresh
useEffect(() => {
  const savedToken = localStorage.getItem("token");
  console.log(savedToken);
if (savedToken) {
setToken(savedToken);
setPage("home");
}
}, []);


const handleAuthSuccess = (token) => {
localStorage.setItem("token", token); // 🔥 HERE
setToken(token);
setPage("home");
};


const logout = () => {
localStorage.removeItem("token");
setToken(null);
setPage("login");
};

  if (!token && page === "home") {
    setPage("login"); // hard guard
  }

  return (
    <>  
   <BrowserRouter>
    <Routes>
        <Route path="/" element={<Home />} />
        {/* <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} /> */}
    </Routes>
  </BrowserRouter>
      {/* {page === "login" && (
        <Login
          goRegister={() => setPage("register")}
          onSuccess={handleAuthSuccess}
        />
      )}

      {page === "register" && (
        <Register
          goLogin={() => setPage("login")}
          onSuccess={handleAuthSuccess}
        />
      )}

      {page === "home" && <Home token={token} onLogout={logout} />} */}
    </>
  );
}