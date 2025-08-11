import api from "../api"

const authService = {

    register: (registerData) => api.post("auth/register", registerData),

    login: (loginData) => api.post("auth/login", loginData)
}

export { authService }