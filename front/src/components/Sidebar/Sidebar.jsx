import styles from './Sidebar.module.css';
import logoMoura from '../../assets/logo-moura.svg';
import userIcon from '../../assets/user-icon.svg';

export const Sidebar = ({ userName }) => {
  return (
    <aside className={styles.sidebar}>
      {/* Info do usuário */}
      <div className={styles.userInfo}>
        <div className={styles.avatar}>
            <img src={userIcon} alt="User avatar" />
        </div>
        <h2 className={styles.userName}>{userName}</h2>
      </div>

      {/* Navegação */}
      <nav className={styles.nav}>
        <button className={styles.navButton}>Minhas Tarefas</button>
        <button className={styles.navButton}>Importantes</button>
        <button className={styles.navButton}>Concluídas</button>
      </nav>

    {/* Logo da Moura*/}
      <div className={styles.logoContainer}>
        <img src={logoMoura} alt="Logo Moura" className={styles.logo} />
      </div>
    </aside>
  );
};
