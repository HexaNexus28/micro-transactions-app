import React from 'react';
import { useAuth } from '../contexts/AuthContext';
import '../styles/Layout.css';

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout, isAuthenticated } = useAuth();

  return (
    <div className="layout">
      <header className="header">
        <div className="header-content">
          <h1 className="logo">
            <span className="logo-icon">💎</span>
            Micro-Transactions RPG
          </h1>
          {isAuthenticated && (
            <div className="user-menu">
              <span className="user-info">
                Bienvenue, {user?.userName || user?.email}
              </span>
              <button onClick={logout} className="logout-btn">
                Déconnexion
              </button>
            </div>
          )}
        </div>
      </header>
      
      <main className="main">
        {children}
      </main>
      
      <footer className="footer">
        <p>&copy; 2024 Micro-Transactions RPG. Tous droits réservés.</p>
      </footer>
    </div>
  );
};

export default Layout;
