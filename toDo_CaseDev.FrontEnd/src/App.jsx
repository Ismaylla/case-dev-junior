import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Home } from './pages/Home/Home';
import { Login } from './pages/Login/Login';
import { Register } from './pages/Register/Register';
import { ToastContainer } from 'react-toastify';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/home" element={<Home />} />
      </Routes>
      <ToastContainer />
    </BrowserRouter>
  );
}

export default App;