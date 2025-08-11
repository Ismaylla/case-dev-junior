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

export const Sidebar = ({ userName, onFilterChange, currentFilter }) => {
  const [statusOpen, setStatusOpen] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState('');

const statusOptions = [
  { name: 'Pendentes', value: 'pending', icon: <FaRegCircle className={styles.statusIcon} /> },
  { name: 'Em Andamento', value: 'in-progress', icon: <FaClock className={styles.statusIcon} /> },
  { name: 'Concluídas', value: 'completed', icon: <FaCheckCircle className={styles.statusIcon} /> }
];

  const handleStatusSelect = (status) => {
    onFilterChange(status);
    setStatusOpen(false);
  };

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
        <button 
          className={`${styles.navButton} ${currentFilter === 'all' ? styles.active : ''}`}
          onClick={() => onFilterChange('all')}
        >
          <FaHome className={styles.navIcon} />
          Minhas Tarefas
        </button>
        
        <button 
          className={styles.navButton}
        >
          <FaStar className={styles.navIcon} />
          Importantes
        </button>
        
        {/* Dropdown de Status */}
        <div className={styles.statusDropdown}>
          <button 
            className={`${styles.navButton} ${styles.withArrow} ${currentFilter.includes('status') ? styles.active : ''}`} 
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
                    handleStatusSelect(option.value);
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