import axios from "axios";


const baseApi = axios.create({
  baseURL: "http://localhost:5128/api",
  headers: {
    "Content-Type": "application/json",
  },
});



//attah token to request automatically  
baseApi.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);
export default baseApi;