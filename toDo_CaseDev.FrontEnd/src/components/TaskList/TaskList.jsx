import { Task } from '../Task/Task';
import styles from './TaskList.module.css';

export const TaskList = ({ tasks, onStatusChange, onDelete }) => {
  const pendingTasks = tasks.filter(task => task.status === 'pending');
  const inProgressTasks = tasks.filter(task => task.status === 'in-progress');
  const completedTasks = tasks.filter(task => task.status === 'completed');

  return (
    <div className={styles.taskListContainer}>
      {/* Tarefas Pendentes */}
      {pendingTasks.length > 0 && (
        <div className={`${styles.taskSection} ${styles['pending-section']}`}>
          <h3 className={`${styles.sectionTitle} ${styles.pendingTitle}`}>Pendentes</h3>
          <div className={styles.tasks}>
            {pendingTasks.map(task => (
              <Task
                key={task.id}
                task={task}
                onStatusChange={onStatusChange}
                onDelete={onDelete}
              />
            ))}
          </div>
        </div>
      )}

      {/* Tarefas em Andamento */}
      {inProgressTasks.length > 0 && (
        <div className={`${styles.taskSection} ${styles['inProgress-section']}`}>
          <h3 className={`${styles.sectionTitle} ${styles.inProgressTitle}`}>Em Andamento</h3>
          <div className={styles.tasks}>
            {inProgressTasks.map(task => (
              <Task
                key={task.id}
                task={task}
                onStatusChange={onStatusChange}
                onDelete={onDelete}
              />
            ))}
          </div>
        </div>
      )}

      {/* Tarefas Concluídas */}
      {completedTasks.length > 0 && (
        <div className={`${styles.taskSection} ${styles['completed-section']}`}>
          <h3 className={`${styles.sectionTitle} ${styles.completedTitle}`}>Concluídas</h3>
          <div className={styles.tasks}>
            {completedTasks.map(task => (
              <Task
                key={task.id}
                task={task}
                onStatusChange={onStatusChange}
                onDelete={onDelete}
              />
            ))}
          </div>
        </div>
      )}

      {/* Mensagem quando não há tarefas */}
      {tasks.length === 0 && (
        <div className={styles.emptyState}>
          <p>Nenhuma tarefa encontrada. Adicione sua primeira tarefa!</p>
        </div>
      )}
    </div>
  );
};