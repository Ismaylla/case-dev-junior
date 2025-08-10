import styles from './Register.module.css';
import { AuthForm } from '../../components/AuthForm/AuthForm';
import empresaBg from '../../assets/fabrica-moura.webp';
import logoMoura from '../../assets/logo-moura.svg';
import { Link, useNavigate } from 'react-router-dom';
import { authService } from '../../service/api/endpoints/auth.endpoints';
import { useState } from 'react';

export const Register = () => {
  const navigate = useNavigate();
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState(null);

  const handleSubmit = async (e, formData) => {
    e.preventDefault();
    setError(null);
    setSuccessMessage(null);
    setIsLoading(true);

    try {
      // Extrai os dados do formulário
      const {email, name, password } = formData;
      
      // Faz a chamada à API de registro
      await authService.register({ email, name, password });
      
      // Exibe mensagem de sucesso
      setSuccessMessage('Cadastro realizado com sucesso! Redirecionando para login...');
      
      // Redireciona para a página de login após 2 segundos
      setTimeout(() => {
        navigate('/login');
      }, 2000);
    } catch (err) {
      console.error('Erro no cadastro:', err);
      setError(err.response?.data?.message || 'Erro ao realizar cadastro. Tente novamente.');
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

      {/* Painel ESQUERDO (formulário) */}
      <div className={styles.leftPanel}>
        <div className={styles.authWrapper}>
          <h1 className={styles.title}>Crie sua conta</h1>
          <p className={styles.subtitle}>Preencha os campos para se cadastrar</p>
          
          {/* Exibe mensagem de erro se houver */}
          {error && <div className={styles.errorMessage}>{error}</div>}
          
          {/* Exibe mensagem de sucesso se houver */}
          {successMessage && <div className={styles.successMessage}>{successMessage}</div>}
          
          <AuthForm 
            isLogin={false} 
            onSubmit={handleSubmit} 
            isLoading={isLoading}
          />
          
          <p className={styles.switchText}>
            Já tem uma conta?{' '}
            <Link to="/login" className={styles.link}>
              Faça login
            </Link>
          </p>
        </div>
      </div>

      {/* Painel DIREITO (imagem) */}
      <div className={styles.rightPanel}>
        <img 
          src={empresaBg} 
          alt="Background da empresa" 
          className={styles.bgImage}
        />
      </div>
    </div>
  );
};