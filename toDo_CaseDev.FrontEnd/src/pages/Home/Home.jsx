import { useState, useEffect, useMemo } from 'react';
import { Sidebar } from '../../components/Sidebar/Sidebar';
import { TaskList } from '../../components/TaskList/TaskList';
import { TaskForm } from '../../components/Taskform/Taskform';
import { taskService } from '../../service/api/endpoints/tasks.endpoints'
import styles from './Home.module.css';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const Home = () => {
  const [tasks, setTasks] = useState([]);
  const [filter, setFilter] = useState('all');
  const [isLoading, setIsLoading] = useState(true);
  const userName = "Usuário";

  // Carrega tarefas ao montar o componente
  useEffect(() => {
    const loadTasks = async () => {
      try {
        const fetchedTasks = await taskService.getAll();
        setTasks(fetchedTasks);
      } catch (error) {
        toast.error('Erro ao carregar tarefas');
        console.error('Erro ao carregar tarefas:', error);
      } finally {
        setIsLoading(false);
      }
    };

    loadTasks();
  }, [])

  // Filtra as tarefas localmente
  const filteredTasks = useMemo(() => {
    switch(filter) {
      case 'pending':
        return tasks.filter(task => task.status === 0);
      case 'in-progress':
        return tasks.filter(task => task.status === 1);
      case 'completed':
        return tasks.filter(task => task.status === 2);
      default:
        return tasks;
    }
  }, [tasks, filter]);

  // Adiciona nova tarefa
  const addTask = async (newTask) => {
    try {
      const createdTask = await taskService.create(newTask);
      setTasks([...tasks, createdTask]);
      toast.success('Tarefa criada com sucesso!');
    } catch (error) {
      toast.error('Erro ao criar tarefa');
      console.error('Erro ao criar tarefa:', error);
    }
  };

  // Atualiza status da tarefa
  const updateTaskStatus = async (taskId, newStatus) => {
    try {
      await taskService.updateStatus(taskId, { status: newStatus });
      setTasks(tasks.map(task => 
        task.id === taskId ? { ...task, status: newStatus } : task
      ));
    } catch (error) {
      toast.error('Erro ao atualizar status');
      console.error('Erro ao atualizar status:', error);
    }
  };

  // Remove tarefa
  const deleteTask = async (taskId) => {
    try {
      await taskService.delete(taskId);
      setTasks(tasks.filter(task => task.id !== taskId));
      toast.success('Tarefa removida com sucesso!');
    } catch (error) {
      toast.error('Erro ao remover tarefa');
      console.error('Erro ao remover tarefa:', error);
    }
  };

  // Alterna entre importante/não importante
  const toggleTaskImportance = (taskId) => {
    setTasks(tasks.map(task => 
      task.id === taskId ? { ...task, important: !task.important } : task
    ));
  };

  // Edita tarefa existente
  const editTask = async (taskId, updatedData) => {
    try {
      const updatedTask = await taskService.update(taskId, updatedData);
      setTasks(tasks.map(task => 
        task.id === taskId ? { ...task, ...updatedTask } : task
      ));
      toast.success('Tarefa atualizada com sucesso!');
    } catch (error) {
      toast.error('Erro ao editar tarefa');
      console.error('Erro ao editar tarefa:', error);
      console.error('Json enviado:', updatedData)
    }
  };

  if (isLoading) {
    return <div className={styles.loading}>Carregando tarefas...</div>;
  }

  return (
    <div className={styles.homeContainer}>
      <Sidebar 
      userName={userName}
      onFilterChange={setFilter} 
      currentFilter={filter}
       />
      
      <main className={styles.mainContent}>
        <h1 className={styles.pageTitle}>
          {filter === 'all' && 'Minhas Tarefas'}
          {filter === 'important' && 'Tarefas Importantes'}
          {filter === 'pending' && 'Tarefas Pendentes'}
          {filter === 'in-progress' && 'Tarefas em Andamento'}
          {filter === 'completed' && 'Tarefas Concluídas'}
        </h1>
        
        <TaskList 
          tasks={filteredTasks} 
          onStatusChange={updateTaskStatus} 
          onDelete={deleteTask}
          onToggleImportant={toggleTaskImportance}
          onEdit={editTask}
        />
        
        {filter === 'all' && <TaskForm onSubmit={addTask} />}
      </main>
    </div>
  );
};