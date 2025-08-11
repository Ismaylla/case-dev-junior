import { useState } from 'react';
import { Sidebar } from '../../components/Sidebar/Sidebar';
import { TaskList } from '../../components/TaskList/TaskList';
import { TaskForm } from '../../components/Taskform/Taskform';
import styles from './Home.module.css';

export const Home = () => {
  const [tasks, setTasks] = useState([]);
  const userName = "Usuário";

  // Adiciona nova tarefa
  const addTask = (newTask) => {
    setTasks([...tasks, { 
      ...newTask, 
      id: Date.now(),
      status: 0, // Define como pendente por padrão
      important: false // Por padrão não é importante
    }]);
  };

  // Atualiza status da tarefa
  const updateTaskStatus = (taskId, newStatus) => {
    setTasks(tasks.map(task => 
      task.id === taskId ? { ...task, status: newStatus } : task
    ));
  };

  // Remove tarefa
  const deleteTask = (taskId) => {
    setTasks(tasks.filter(task => task.id !== taskId));
  };

  // Alterna entre importante/não importante
  const toggleTaskImportance = (taskId) => {
    setTasks(tasks.map(task => 
      task.id === taskId ? { ...task, important: !task.important } : task
    ));
  };

  // Edita tarefa existente
  const editTask = (taskId, updatedData) => {
    setTasks(tasks.map(task => 
      task.id === taskId ? { ...task, ...updatedData } : task
    ));
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
          onToggleImportant={toggleTaskImportance}
          onEdit={editTask}
        />
        
        <TaskForm onSubmit={addTask} />
      </main>
    </div>
  );
};