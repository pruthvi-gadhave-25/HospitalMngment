import { useEffect, useState } from "react";
import Register from "./components/Register";
import Home from "./components/Home";
import Login from "./components/Login";
import {Routes,Route, Navigate } from "react-router-dom";
import Navbar from "./components/Navbar";

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




  useEffect(() => {
  if (!token && page === "home") {
    setPage("login");
  }
  
}, [token, page]);

  return (
    <>  
    <div>
       <Navbar/>
       <Routes>
        <Route path="/" element={ <Navigate to ="/Register"/>} />

        <Route path="/home" element={<Home/>}/>
        <Route path="/login" element={<Login/ >}/>
        <Route path="/register" element={<Register/>}/>
       </Routes>
 
      </div> 
      </>
  );
}

