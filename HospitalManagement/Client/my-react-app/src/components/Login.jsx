import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login({  onSuccess }) {

  const navigate = useNavigate();


  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");


  // const handleLogin = async () => {
  //   if (!email || !password) {
  //     alert("Email and password required");
  //     return;
  //   }

  const handleLogin = async () => {
    navigate("/");
    return;
  }
  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);
    setError("");

    try {
      const res = await login(formData);

      // Store token
      localStorage.setItem("token", res.data.token);

      // Redirect after login
      navigate("/dashboard");

    } catch (err) {
      setError(err.response?.data?.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  // onSuccess(data.token);
  // if (!res.ok) {
  //   alert("Login failed");
  //   return;
  // }

const  goRegister = () => {
  navigate("/register")
}

return (
  <div>
    <h2>Login</h2>


    {/* EMAIL INPUT */}
    {/* <input
      type="email"
      placeholder="Email"
      value={email}
      onChange={(e) => setEmail(e.target.value)}
    /> */}



    {/* PASSWORD INPUT */}
    {/* <input
      type="password"
      placeholder="Password"
      value={password}
      onChange={(e) => setPassword(e.target.value)}
    /> */}

    <button onClick={handleLogin}>Login</button>
    <p>
      No account?
      <button onClick={goRegister}>Register</button>
    </p>
  </div>
);
}
