import styles from './Sidebar.module.css';
import logoMoura from '../../assets/logo-moura.svg';
import { useState } from 'react';
import { 
  FaHome, 
  FaStar, 
  FaUserCircle,
  FaCircle, 
  FaRegCircle,
  FaCheckCircle,
  FaClock 
} from 'react-icons/fa';
import { IoIosArrowDown } from 'react-icons/io';

export const Sidebar = ({ userName }) => {
  const [statusOpen, setStatusOpen] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState('');

  const statusOptions = [
    { name: 'Pendentes', icon: <FaRegCircle className={styles.statusIcon} /> },
    { name: 'Em Andamento', icon: <FaClock className={styles.statusIcon} /> },
    { name: 'Concluídas', icon: <FaCheckCircle className={styles.statusIcon} /> }
  ];

  return (
    <aside className={styles.sidebar}>
      {/* Logo da Moura */}
      <div className={styles.logoContainer}>
        <img src={logoMoura} alt="Logo Moura" className={styles.logo} />
      </div>

      {/* Container do usuário */}
      <div className={styles.userContainer}>
        <div className={styles.userCard}>
          <FaUserCircle className={styles.userIcon} />
          <span className={styles.userName}>{userName}</span>
        </div>
      </div>

      {/* Navegação */}
      <nav className={styles.nav}>
        <button className={styles.navButton}>
          <FaHome className={styles.navIcon} />
          Minhas Tarefas
        </button>
        
        <button className={styles.navButton}>
          <FaStar className={styles.navIcon} />
          Importantes
        </button>
        
        {/* Dropdown de Status */}
        <div className={styles.statusDropdown}>
          <button 
            className={`${styles.navButton} ${styles.withArrow}`} 
            onClick={() => setStatusOpen(!statusOpen)}
          >
            <div className={styles.statusTitle}>
              <FaCircle className={styles.statusIcon} />
              Status
            </div>
            <IoIosArrowDown className={`${styles.arrowIcon} ${statusOpen ? styles.arrowOpen : ''}`} />
          </button>
          
          {statusOpen && (
            <div className={styles.statusOptions}>
              {statusOptions.map((option) => (
                <button
                  key={option.name}
                  className={`${styles.statusOption} ${selectedStatus === option.name ? styles.selected : ''}`}
                  onClick={() => {
                    setSelectedStatus(option.name);
                    setStatusOpen(false);
                  }}
                >
                  {option.icon}
                  {option.name}
                </button>
              ))}
            </div>
          )}
        </div>
      </nav>
    </aside>
  );
};