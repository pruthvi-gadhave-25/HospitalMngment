import { useState } from "react";
export default function Register({ goLogin, onSuccess }) {
  
   const [userName, setUserName] = useState("");
const [email, setEmail] = useState("");
const [password, setPassword] = useState("");
const [error, setError] = useState("");


const role = "Admin"; // 🔥 default role for now

const handleRegister = async () => {
setError("");


if (!userName || !email || !password) {
setError("All fields are required");
return;
}


const res = await fetch("http://localhost:5128/api/Auth/register", {
method: "POST",
headers: { "Content-Type": "application/json" },
body: JSON.stringify({
userName,
email,
password,
role,
}),
});


const data = await res.json();


if (!res.ok || data.success === false) {
setError(data.message || "Register failed");
return;
}


onSuccess(data.token);
};

  return (
    <div>
      <h2>Register</h2>
{/* USERNAME */}
<input
type="text"
placeholder="Username"
value={userName}
onChange={(e) => setUserName(e.target.value)}
/>

{/* EMAIL */}
<input
type="email"
placeholder="Email"
value={email}
onChange={(e) => setEmail(e.target.value)}
/>


{/* PASSWORD */}
<input
type="password"
placeholder="Password"
value={password}
onChange={(e) => setPassword(e.target.value)}
/>


<button onClick={handleRegister}>Register</button>
      <p>
        Already have an account?
        <button onClick={goLogin}>Login</button>
      </p>
    </div>
  );
}