import styles from './Task.module.css';

export const Task = ({ task, onStatusChange, onDelete }) => {
  const getStatusColor = () => {
    switch (task.status) {
      case 'pending':
        return styles.pending;
      case 'in-progress':
        return styles.inProgress;
      case 'completed':
        return styles.completed;
      default:
        return '';
    }
  };

  const getStatusDot = () => {
    switch (task.status) {
      case 'pending':
        return styles.pendingDot;
      case 'in-progress':
        return styles.inProgressDot;
      case 'completed':
        return styles.completedDot;
      default:
        return '';
    }
  };

  return (
    <div className={`${styles.task} ${getStatusColor()}`}>
      <div className={styles.taskContent}>
        <h3 className={styles.taskTitle}>{task.title}</h3>
        <p className={styles.taskDescription}>{task.description}</p>
      </div>
      <div className={styles.taskActions}>
        <div className={styles.selectContainer}>
          <div className={`${styles.statusIndicator} ${getStatusDot()}`}></div>
          <select
            className={styles.statusSelect}
            value={task.status}
            onChange={(e) => onStatusChange(task.id, e.target.value)}
          >
            <option value="pending">Pendente</option>
            <option value="in-progress">Em Andamento</option>
            <option value="completed">Concluída</option>
          </select>
        </div>
        <button className={styles.deleteButton} onClick={() => onDelete(task.id)}>
          ×
        </button>
      </div>
    </div>
  );
};