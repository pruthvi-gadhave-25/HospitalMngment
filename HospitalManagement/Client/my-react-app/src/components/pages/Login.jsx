import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../../services/userService";

export default function Login() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: "",
    password: ""
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
      const response = await login(formData);

      if (!response.data.success) {
        setError(response.data.message || "Login failed");
        return;
      }

      localStorage.setItem("token", response.data.token);
      navigate("/dashboard");
    } catch (requestError) {
      const message = requestError.response?.data?.message;
      const responseBody = requestError.response?.data;
      setError(
        (typeof message === "string" && message) ||
          (typeof responseBody === "string" && responseBody) ||
          "Login failed"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="auth-page">
      <section className="auth-card" aria-labelledby="login-heading">
        <h1 className="auth-heading" id="login-heading">Welcome back</h1>
        <p className="auth-description">Sign in to your hospital account.</p>
        <form className="auth-form" onSubmit={handleSubmit}>
          <div className="auth-field">
            <label htmlFor="login-email">Email</label>
            <input
              id="login-email"
              type="email"
              name="email"
              placeholder="you@example.com"
              autoComplete="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="auth-field">
            <label htmlFor="login-password">Password</label>
            <input
              id="login-password"
              type="password"
              name="password"
              placeholder="Enter your password"
              autoComplete="current-password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          {error && <p className="auth-error" role="alert">{error}</p>}
          <button className="auth-submit" type="submit" disabled={loading}>
            {loading ? "Logging in..." : "Login"}
          </button>
        </form>
        <p className="auth-switch">
          No account?{" "}
          <button type="button" onClick={() => navigate("/register")}>
            Register
          </button>
        </p>
      </section>
    </main>
  );
}
