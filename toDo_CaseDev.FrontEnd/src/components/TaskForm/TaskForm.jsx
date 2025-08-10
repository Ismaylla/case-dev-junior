import { useState } from 'react';
import styles from './TaskForm.module.css';

export const TaskForm = ({ onSubmit }) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!title.trim()) return;
    
    onSubmit({
      title,
      description,
      status: 'pending'
    });
    
    setTitle('');
    setDescription('');
  };

  return (
    <form className={styles.taskForm} onSubmit={handleSubmit}>
      <div className={styles.formGroup}>
        <input
          type="text"
          placeholder="Título da tarefa"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          className={styles.input}
          required
        />
      </div>
      <div className={styles.formGroup}>
        <textarea
          placeholder="Descrição (opcional)"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className={styles.textarea}
          rows="2"
        />
      </div>
      <button type="submit" className={styles.submitButton}>
        Adicionar Tarefa
      </button>
    </form>
  );
};