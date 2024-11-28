import { BrowserRouter, Routes, Route } from "react-router";
import Signin from "./pages/Signin";
import Signup from "./pages/Signup";
import Home from "./pages/Home";
import Layout from "./components/custom/Layout";

export default function App() {
   return (
      <BrowserRouter>
         <Routes>
            <Route path="/" element={<Signin />} />
            <Route path="/signup" element={<Signup />} />
            <Route path="/home" element={<Layout />}>
               <Route index element={<Home />} />
            </Route>
         </Routes>
      </BrowserRouter>
   );
}
