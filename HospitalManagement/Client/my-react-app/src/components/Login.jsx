import { useState } from "react";
import { useActionData, useNavigate } from "react-router-dom";
import { login } from "../services/userService";

export default function Login({ onSuccess }) {

  const navigate = useNavigate();

  const [formData ,setFormData]  = useState({
    email :"",
    password : "" 
  })  ;
  
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const[loading ,setLoading] = useState(false);


  // const handleLogin = async () => {
  //   if (!email || !password) {
  //     alert("Email and password required");
  //     return;
  //   }

  const handleChange = (e) => {
    console.log(e.target.value);
    
   setFormData( {
     ...formData , 
     [e.target.name] : e.target.value
   }) ;

  }

  const handleLogin = async () => {
    navigate("/");
    return;
  }
  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);
    setError("");

    try {
      const res = await  login(formData);
      debugger;
      if (res.data.success) {
        // Store token  
        localStorage.setItem("token", res.data.token);
        // Redirect after login
        navigate("/dashboard");
      } else {
        setError(err.message || "Something went wrong" + res.messgae);
      }
    } catch (err) {
      console.log(err);
      
      setError(err.response?.data?.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  const goRegister = () => {
    navigate("/register")
  }

  return (
    <div>
      <h2>Login</h2>


      {/* EMAIL INPUT */}
      <input
        type="email"
        placeholder="Email"
        value={formData.email}
        name ="email"
        onChange={handleChange}
      />



      {/* PASSWORD INPUT */}
      <input
        type="password"
        placeholder="Password"
        value={formData.password}
        name="password"
        onChange={handleChange}
      />

      <button type="submit" onClick={handleSubmit}>Login</button>
      <p>
        No account?
        <button onClick={goRegister}>Register</button>
      </p>
    </div>
  );
}
