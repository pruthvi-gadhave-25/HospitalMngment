import { useState } from "react"
import { Navigate, useNavigate } from "react-router-dom";
import { register } from "../services/userService";

function Register() {
  const navigate = useNavigate();

  
  const [formData, setFormData] = useState({
    userName: "",
    email: "",
    password: "",
    role: "Admin"
  });



  const handleChange = (e) => {
    console.log(e.target.value);

    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    })
  }

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.username || !formData.password) {
      alert("All fields are required");
      return;
    }
    else {
      register(formData)
      navigate("/login");
    }

    console.log("User Registered:", formData);
    // TODO: send data to backend API
    // fetch("https://your-api/register", { method: "POST", body: JSON.stringify(formData) })

  }

  return (

    <>
      <form onSubmit={handleSubmit}>

        <div style={{ marginBottom: "10px" }}>
          <label>Username</label>
          <input
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            style={{ width: "100%", padding: "8px" }}
          />
        </div>

        <div style={{ marginBottom: "10px" }}>
          <label>Email</label>
          <input
            type="text"
            name="email"
            value={formData.email}
            onChange={handleChange}
            style={{ width: "100%", padding: "8px" }}
          />
        </div>

        <div style={{ marginBottom: "10px" }}>
          <label>Password</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            style={{ width: "100%", padding: "8px" }}
          />
        </div>
       

        <button type="submit" onSubmit={handleSubmit}>Register</button>

      </form>

    </>
  )
}

export default Register;