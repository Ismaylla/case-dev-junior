import styles from './Task.module.css';
import { useState } from 'react';
import { FaEdit } from 'react-icons/fa';
import { EditTaskModal } from '../EditTaskModal/EditTaskModal';

export const Task = ({ task, onStatusChange, onDelete, onEdit }) => {
  const [isEditing, setIsEditing] = useState(false);

  const statusMap = {
    0: { text: 'Pendente', class: styles.pending, dot: styles.pendingDot },
    1: { text: 'Em Andamento', class: styles.inProgress, dot: styles.inProgressDot },
    2: { text: 'Concluída', class: styles.completed, dot: styles.completedDot }
  };

  const currentStatus = statusMap[task.status] || statusMap[0];

  const handleEditSubmit = (editedData) => {
    onEdit(task.id, editedData);
    setIsEditing(false);
  };

  return (
    <>
      <div className={`${styles.task} ${currentStatus.class}`}>
        <div className={styles.taskHeader}>
          <h3 className={styles.taskTitle}>{task.title}</h3>
        </div>
        
        <p className={styles.taskDescription}>{task.description}</p>
        
        <div className={styles.taskActions}>
          <div className={styles.selectContainer}>
            <div className={`${styles.statusIndicator} ${currentStatus.dot}`}></div>
            <select
              className={styles.statusSelect}
              value={task.status}
              onChange={(e) => onStatusChange(task.id, parseInt(e.target.value))}
            >
              <option value={0}>Pendente</option>
              <option value={1}>Em Andamento</option>
              <option value={2}>Concluída</option>
            </select>
          </div>
          
          <div className={styles.actionButtons}>
            <button 
              onClick={() => setIsEditing(true)}
              className={styles.editButton}
              aria-label="Editar tarefa"
            >
              <FaEdit />
            </button>
            
            <button 
              onClick={() => onDelete(task.id)}
              className={styles.deleteButton}
              aria-label="Excluir tarefa"
            >
              ×
            </button>
          </div>
        </div>
      </div>
      
      {isEditing && (
        <EditTaskModal
          task={task}
          onSave={handleEditSubmit}
          onClose={() => setIsEditing(false)}
        />
      )}
    </>
  );
};