import { useEffect, useState } from "react";

export default function Home({ token, onLogout }) {
  const [data, setData] = useState(null);

  // useEffect(() => {
  //   fetch("https://api.example.com/profile", {
  //     headers: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   })
  //     .then((res) => res.json())
  //     .then(setData);
  // }, [token]);
  useEffect( () => {
    setData("WElcome to Home");
  })

  return (
    <div>
      <h1>Home</h1>
      <pre>{JSON.stringify(data, null, 2)}</pre>
      <button onClick={onLogout}>Logout</button>
    </div>
  );
}