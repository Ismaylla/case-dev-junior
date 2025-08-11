import styles from './EditTaskModal.module.css';
import { useState } from 'react';

export const EditTaskModal = ({ task, onSave, onClose }) => {
  const [editedTask, setEditedTask] = useState({
    title: task.title,
    description: task.description
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditedTask(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(task.id, editedTask);
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <h2>Editar Tarefa</h2>
        <form onSubmit={handleSubmit}>
          <div className={styles.formGroup}>
            <label>Título</label>
            <input
              type="text"
              name="title"
              value={editedTask.title}
              onChange={handleChange}
              required
            />
          </div>
          <div className={styles.formGroup}>
            <label>Descrição</label>
            <textarea
              name="description"
              value={editedTask.description}
              onChange={handleChange}
              rows="4"
            />
          </div>
          <div className={styles.modalActions}>
            <button type="button" onClick={onClose} className={styles.cancelButton}>
              Cancelar
            </button>
            <button type="submit" className={styles.saveButton}>
              Salvar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};