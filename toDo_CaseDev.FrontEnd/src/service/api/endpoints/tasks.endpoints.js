import api from "../api"

/**
 * @typedef {Object} TaskDTO
 * @property {number} id
 * @property {string} title
 * @property {string} description
 * @property {number} status (0-pendente, 1-em andamento, 2-concluída)
 */

const taskService = {
  /**
   * Obtém todas as tarefas
   * @returns {Promise<Array<TaskDTO>>}
   */
  getAll: () => api.get("Todo").then(res => res.data),

  /**
   * Cria uma nova tarefa
   * @param {Object} taskData
   * @param {string} taskData.title
   * @param {string} taskData.description
   * @returns {Promise<TaskDTO>}
   */
  create: (taskData) => api.post("Todo", taskData).then(res => res.data),

  /**
   * Atualiza uma tarefa
   * @param {number} id 
   * @param {Object} taskData
   * @param {string} taskData.title
   * @param {string} taskData.description
   * @returns {Promise<TaskDTO>}
   */
  update: (id, taskData) => api.put(`Todo/${id}`, taskData).then(res => res.data),

  /**
   * Atualiza o status de uma tarefa
   * @param {number} id 
   * @param {Object} statusData
   * @param {number} statusData.status (0, 1, 2)
   * @returns {Promise<TaskDTO>}
   */
  updateStatus: (id, statusData) => 
    api.put(`Todo/${id}/status`, statusData).then(res => res.data),

  /**
   * Deleta uma tarefa
   * @param {number} id 
   * @returns {Promise<void>}
   */
  delete: (id) => api.delete(`Todo/${id}`),
};

export { taskService };