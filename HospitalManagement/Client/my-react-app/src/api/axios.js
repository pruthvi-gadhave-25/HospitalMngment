import axios from "axios";


const baseApi = axios.create({
  baseURL: "https://api.myapp.com/api",
  headers: {
    "Content-Type": "application/json",
  },
});

export default baseApi;