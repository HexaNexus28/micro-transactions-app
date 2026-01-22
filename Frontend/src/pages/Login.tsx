import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { LoginRequest, RegisterRequest } from '../types';
import '../styles/Login.css';

const Login: React.FC = () => {
  const [formData, setFormData] = useState<LoginRequest & { userName?: string }>({
    email: '',
    password: '',
    userName: ''
  });
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const { login, register } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      if (isLogin) {
        const loginData: LoginRequest = {
          email: formData.email,
          password: formData.password
        };
        await login(loginData);
        navigate('/dashboard');
      } else {
        const registerData: RegisterRequest = {
          userName: formData.userName || '',
          email: formData.email,
          password: formData.password
        };
        await register(registerData);
        setIsLogin(true);
        setError('Inscription réussie! Vous pouvez maintenant vous connecter.');
      }
    } catch (err: any) {
      setError(err.message || 'Une erreur est survenue');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="login-header">
          <h2>{isLogin ? 'Connexion' : 'Inscription'}</h2>
          <p className="login-subtitle">
            {isLogin ? 'Accédez à votre compte' : 'Créez votre compte'}
          </p>
        </div>

        {error && (
          <div className={`alert ${error.includes('réussie') ? 'success' : 'error'}`}>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="login-form">
          {!isLogin && (
            <div className="form-group">
              <label htmlFor="userName">Nom d'utilisateur</label>
              <input
                type="text"
                id="userName"
                name="userName"
                value={formData.userName || ''}
                onChange={handleChange}
                required={!isLogin}
                className="form-input"
              />
            </div>
          )}

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              className="form-input"
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Mot de passe</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
              className="form-input"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="login-btn"
          >
            {loading ? 'Chargement...' : (isLogin ? 'Se connecter' : "S'inscrire")}
          </button>
        </form>

        <div className="login-footer">
          <p>
            {isLogin ? "Pas encore de compte?" : "Déjà un compte?"}
            <button
              type="button"
              onClick={() => {
                setIsLogin(!isLogin);
                setError('');
              }}
              className="toggle-btn"
            >
              {isLogin ? "S'inscrire" : 'Se connecter'}
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Login;
