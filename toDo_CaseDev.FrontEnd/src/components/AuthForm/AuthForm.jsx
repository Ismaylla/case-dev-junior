import styles from './AuthForm.module.css';
import { useState } from 'react';

export const AuthForm = ({ isLogin, onSubmit, isLoading }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const [errors, setErrors] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const handleChange = (e) => {
    const { id, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [id]: value
    }));
    
    // Limpa o erro quando o usuário começa a digitar
    setErrors(prev => ({
      ...prev,
      [id]: ''
    }));
  };

  const validateForm = () => {
    let isValid = true;
    const newErrors = {
      name: '',
      email: '',
      password: '',
      confirmPassword: ''
    };

    // Validação do nome (apenas para registro)
    if (!isLogin && !formData.name.trim()) {
      newErrors.name = 'Por favor, preencha seu nome completo';
      isValid = false;
    }

    // Validação do email
    if (!formData.email.trim()) {
      newErrors.email = 'Por favor, informe seu e-mail';
      isValid = false;
    } else if (!/^\S+@\S+\.\S+$/.test(formData.email)) {
      newErrors.email = 'Por favor, informe um e-mail válido';
      isValid = false;
    }

    // Validação da senha
    if (!formData.password) {
      newErrors.password = 'Por favor, informe sua senha';
      isValid = false;
    } else if (formData.password.length < 8) {
      newErrors.password = 'A senha deve ter no mínimo 8 caracteres';
      isValid = false;
    }

    // Validação da confirmação de senha (apenas para registro)
    if (!isLogin && formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'As senhas não coincidem';
      isValid = false;
    }

    setErrors(newErrors);
    return isValid;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }
    
    // Prepara os dados para enviar
    const dataToSubmit = isLogin 
      ? { email: formData.email, password: formData.password }
      : { name: formData.name, email: formData.email, password: formData.password };

    // Chama a função onSubmit do componente pai com os dados
    onSubmit(e, dataToSubmit);
  };

  return (
    <form className={styles.authForm} onSubmit={handleSubmit} noValidate>
      {!isLogin && (
        <div className={styles.formGroup}>
          <label htmlFor="name">Nome completo</label>
          <input 
            type="text" 
            id="name" 
            value={formData.name}
            onChange={handleChange}
            required 
          />
          {errors.name && <span className={styles.errorMessage}>{errors.name}</span>}
        </div>
      )}
      
      <div className={styles.formGroup}>
        <label htmlFor="email">E-mail</label>
        <input 
          type="email" 
          id="email" 
          value={formData.email}
          onChange={handleChange}
          required 
        />
        {errors.email && <span className={styles.errorMessage}>{errors.email}</span>}
      </div>
      
      <div className={styles.formGroup}>
        <label htmlFor="password">Senha</label>
        <input 
          type="password" 
          id="password" 
          value={formData.password}
          onChange={handleChange}
          required 
          minLength={8}
        />
        {errors.password && <span className={styles.errorMessage}>{errors.password}</span>}
      </div>
      
      {!isLogin && (
        <div className={styles.formGroup}>
          <label htmlFor="confirmPassword">Confirme sua senha</label>
          <input 
            type="password" 
            id="confirmPassword" 
            value={formData.confirmPassword}
            onChange={handleChange}
            required 
            minLength={8}
          />
          {errors.confirmPassword && <span className={styles.errorMessage}>{errors.confirmPassword}</span>}
        </div>
      )}
      
      <button 
        type="submit" 
        className={styles.submitButton}
        disabled={isLoading}
      >
        {isLoading ? 'Processando...' : isLogin ? 'Entrar' : 'Cadastrar'}
      </button>
    </form>
  );
};