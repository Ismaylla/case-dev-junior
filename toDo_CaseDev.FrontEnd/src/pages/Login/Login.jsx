import styles from './Login.module.css';
import { AuthForm } from '../../components/AuthForm/AuthForm';
import logoMoura from '../../assets/logo-moura.svg';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../service/api/endpoints/auth.endpoints';
import { useState } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const Login = () => {
  const navigate = useNavigate();
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e, formData) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      // Extrai email e password do formData
      const { email, password } = formData;
      
      // Faz a chamada à API de login
      const response = await authService.login({ email, password });
      
      // Armazena o token no localStorage 
      localStorage.setItem('authToken', response.data.access_token);
      
      // Redireciona para a página home após login bem-sucedido
      navigate('/home');
    } catch (err) {
      toast.error(err.response?.data?.message || 'Erro ao fazer login.');
      setError(err.response?.data?.message || 'Erro ao fazer login. Verifique suas credenciais.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      {/* Navbar */}
      <nav className={styles.navbar}>
        <img src={logoMoura} alt="Logo Moura" className={styles.navbarLogo} />
      </nav>

      {/* Painel com formulário */}
      <div className={styles.leftPanel}>
        <div className={styles.authWrapper}>
          <h1 className={styles.title}>Bem-vindo de volta</h1>
          <p className={styles.subtitle}>Faça login para acessar sua conta</p>
          
          {/* Exibe mensagem de erro se houver */}
          {error && <div className={styles.errorMessage}>{error}</div>}
          
          <AuthForm 
            isLogin={true} 
            onSubmit={handleSubmit} 
            isLoading={isLoading}
          />
          
          <p className={styles.switchText}>
            Não tem uma conta?{' '}
            <Link to="/register" className={styles.link}>
              Cadastre-se
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};