import { useState } from "react";

export default function Login({ goRegister, onSuccess }) {
 const [email, setEmail] = useState("");
 const [password, setPassword] = useState("");
 const [error, setError] = useState("");


const handleLogin = async () => {
if (!email || !password) {
alert("Email and password required");
return;
}


const res = await fetch("http://localhost:5128/api/Auth/login", {
method: "POST",
headers: { "Content-Type": "application/json" },
body: JSON.stringify({ email, password }),
});
    
const data = await res.json(); // 
if (!res.ok || data.success === false) {
setError(data.message || "Login failed");
alert(error);
return;
}


onSuccess(data.token);
    if (!res.ok) {
      alert("Login failed");
      return;
    }
  };

  return (
    <div>   
        <h2>Login</h2>


{/* EMAIL INPUT */}
<input
type="email"
placeholder="Email"
value={email}
onChange={(e) => setEmail(e.target.value)}
/>


{/* PASSWORD INPUT */}
<input
type="password"
placeholder="Password"
value={password}
onChange={(e) => setPassword(e.target.value)}
/>
    
          <button onClick={handleLogin}>Login</button>
      <p>
        No account?
        <button onClick={goRegister}>Register</button>
      </p>
    </div>
  );
}