import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../../services/userService";

function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    userName: "",
    email: "",
    password: "",
    role: "Admin"
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (event) => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value
    });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setError("");

    try {
      const response = await register(formData);

      if (!response.data.success) {
        setError(response.data.message || "Registration failed");
        return;
      }

      navigate("/login");
    } catch (requestError) {
      const message = requestError.response?.data?.message;
      const responseBody = requestError.response?.data;
      setError(
        (typeof message === "string" && message) ||
          (typeof responseBody === "string" && responseBody) ||
          "Registration failed"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="auth-page">
      <section className="auth-card" aria-labelledby="register-heading">
        <h1 className="auth-heading" id="register-heading">Create your account</h1>
        <p className="auth-description">Register to access the hospital management system.</p>
        <form className="auth-form" onSubmit={handleSubmit}>
          <div className="auth-field">
            <label htmlFor="register-userName">Username</label>
            <input
              id="register-userName"
              type="text"
              name="userName"
              value={formData.userName}
              onChange={handleChange}
              autoComplete="username"
              required
            />
          </div>
          <div className="auth-field">
            <label htmlFor="register-email">Email</label>
            <input
              id="register-email"
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              autoComplete="email"
              required
            />
          </div>
          <div className="auth-field">
            <label htmlFor="register-password">Password</label>
            <input
              id="register-password"
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              autoComplete="new-password"
              required
            />
          </div>
          {error && <p className="auth-error" role="alert">{error}</p>}
          <button className="auth-submit" type="submit" disabled={loading}>
            {loading ? "Registering..." : "Register"}
          </button>
        </form>
      </section>
    </main>
  );
}

export default Register;
