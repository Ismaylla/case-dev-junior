import axios from 'axios'

const api = axios.create({
    baseURL: 'http://localhost:5161/api/',
    timeout: 1000,
      headers: {
    'Content-Type': 'application/json'
  }
})

api.interceptors.request.use(config => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;