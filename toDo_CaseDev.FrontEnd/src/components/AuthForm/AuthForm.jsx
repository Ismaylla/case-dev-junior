import styles from './AuthForm.module.css';

export const AuthForm = ({ isLogin, onSubmit }) => {
  return (
    <form className={styles.authForm} onSubmit={onSubmit}>
      {!isLogin && (
        <div className={styles.formGroup}>
          <label htmlFor="name">Nome completo</label>
          <input type="text" id="name" required />
        </div>
      )}
      
      <div className={styles.formGroup}>
        <label htmlFor="email">E-mail</label>
        <input type="email" id="email" required />
      </div>
      
      <div className={styles.formGroup}>
        <label htmlFor="password">Senha</label>
        <input type="password" id="password" required minLength={6} />
      </div>
      
      {!isLogin && (
        <div className={styles.formGroup}>
          <label htmlFor="confirmPassword">Confirme sua senha</label>
          <input type="password" id="confirmPassword" required minLength={6} />
        </div>
      )}
      
      <button type="submit" className={styles.submitButton}>
        {isLogin ? 'Entrar' : 'Cadastrar'}
      </button>
    </form>
  );
};