import { useState } from 'react';
import { Sidebar } from '../../components/Sidebar/Sidebar';
import { TaskList } from '../../components/TaskList/TaskList';
import { TaskForm } from '../../components/Taskform/Taskform';
import styles from './Home.module.css';

export const Home = () => {
  const [tasks, setTasks] = useState([]);
  const userName = "Usuário";

  const addTask = (newTask) => {
    setTasks([...tasks, { ...newTask, id: Date.now() }]);
  };

  const updateTaskStatus = (taskId, newStatus) => {
    setTasks(tasks.map(task => 
      task.id === taskId ? { ...task, status: newStatus } : task
    ));
  };

  const deleteTask = (taskId) => {
    setTasks(tasks.filter(task => task.id !== taskId));
  };

  return (
    <div className={styles.homeContainer}>
      <Sidebar userName={userName} />
      <main className={styles.mainContent}>
        <h1 className={styles.pageTitle}>Minhas Tarefas</h1>
        <TaskList 
          tasks={tasks} 
          onStatusChange={updateTaskStatus} 
          onDelete={deleteTask} 
        />
        <TaskForm onSubmit={addTask} />
      </main>
    </div>
  );
};