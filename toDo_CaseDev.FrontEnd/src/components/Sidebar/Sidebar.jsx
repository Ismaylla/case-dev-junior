import styles from './Sidebar.module.css';
import logoMoura from '../../assets/logo-moura.svg';
import { useState } from 'react';
import { FaHome } from 'react-icons/fa';
import { IoIosArrowDown } from 'react-icons/io';

export const Sidebar = ({ userName }) => {
  const [statusOpen, setStatusOpen] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState('');

  const statusOptions = ['Pendentes', 'Em Andamento', 'Concluídas'];

  const userPhoto = "https://thispersondoesnotexist.com/"

  return (
    <aside className={styles.sidebar}>
      {/* Logo da Moura */}
      <div className={styles.logoContainer}>
        <img src={logoMoura} alt="Logo Moura" className={styles.logo} />
      </div>

      {/* Container do usuário */}
      <div className={styles.userContainer}>
        <div className={styles.userCard}>
          <img src={userPhoto} alt="User" className={styles.userPhoto} />
          <span className={styles.userName}>{userName}</span>
        </div>
      </div>

      {/* Navegação */}
      <nav className={styles.nav}>
        <button className={styles.navButton}>
          <FaHome className={styles.navIcon} />
          Minhas Tarefas
        </button>
        
        {/* Dropdown de Status */}
        <div className={styles.statusDropdown}>
          <button 
            className={`${styles.navButton} ${styles.withArrow}`} 
            onClick={() => setStatusOpen(!statusOpen)}
          >
            Status
            <IoIosArrowDown className={`${styles.arrowIcon} ${statusOpen ? styles.arrowOpen : ''}`} />
          </button>
          
          {statusOpen && (
            <div className={styles.statusOptions}>
              {statusOptions.map((option) => (
                <button
                  key={option}
                  className={`${styles.statusOption} ${selectedStatus === option ? styles.selected : ''}`}
                  onClick={() => {
                    setSelectedStatus(option);
                    setStatusOpen(false);
                  }}
                >
                  {option}
                </button>
              ))}
            </div>
          )}
        </div>
      </nav>
    </aside>
  );
};