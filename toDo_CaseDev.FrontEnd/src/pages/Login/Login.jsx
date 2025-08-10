import styles from './Login.module.css';
import { AuthForm } from '../../components/AuthForm/AuthForm';
import empresaBg from '../../assets/fabrica-moura.webp';
import logoMoura from '../../assets/logo-moura.svg';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

export const Login = () => {

  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    // Lógica de login
    navigate('/home');
  };

  return (
    <div className={styles.container}>
      {/* Navbar */}
      <nav className={styles.navbar}>
        <img src={logoMoura} alt="Logo Moura" className={styles.navbarLogo} />
      </nav>

      {/* Painel ESQUERDO (agora com formulário) */}
      <div className={styles.leftPanel}>
        <div className={styles.authWrapper}>
          <h1 className={styles.title}>Bem-vindo de volta</h1>
          <p className={styles.subtitle}>Faça login para acessar sua conta</p>
          
          <AuthForm isLogin={true} onSubmit={handleSubmit} />
          
          <p className={styles.switchText}>
            Não tem uma conta?{' '}
            <Link to="/register" className={styles.link}>
              Cadastre-se
            </Link>
          </p>
        </div>
      </div>

      {/* Painel DIREITO (agora com imagem) */}
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