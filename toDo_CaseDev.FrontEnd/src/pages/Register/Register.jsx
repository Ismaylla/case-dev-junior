import styles from './Register.module.css';
import { AuthForm } from '../../components/AuthForm/AuthForm';
import empresaBg from '../../assets/fabrica-moura.webp';
import logoMoura from '../../assets/logo-moura.svg';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

export const Register = () => {
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    // Lógica de cadastro
    navigate('/home');
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
          
          <AuthForm isLogin={false} onSubmit={handleSubmit} />
          
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