import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { apiService } from '../services/api';
import { Transaction, Item, CreateTransactionRequest } from '../types';
import '../styles/Dashboard.css';

const Dashboard: React.FC = () => {
  const { user } = useAuth();
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [items, setItems] = useState<Item[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showCreateTransaction, setShowCreateTransaction] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [transactionsRes, itemsRes] = await Promise.all([
        apiService.getTransactions(),
        apiService.getItems()
      ]);

      if (transactionsRes.success && transactionsRes.data) {
        setTransactions(transactionsRes.data);
      }
      if (itemsRes.success && itemsRes.data) {
        setItems(itemsRes.data);
      }
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateTransaction = async (transactionData: CreateTransactionRequest) => {
    try {
      const response = await apiService.createTransaction(transactionData);
      if (response.success) {
        setShowCreateTransaction(false);
        loadData();
      }
    } catch (err: any) {
      setError(err.message);
    }
  };

  if (loading) {
    return (
      <div className="dashboard-loading">
        <div className="spinner"></div>
        <p>Chargement...</p>
      </div>
    );
  }

  return (
    <div className="dashboard">
      <div className="dashboard-header">
        <h1>Tableau de Bord</h1>
        <p>Bienvenue, {user?.userName || user?.email}!</p>
      </div>

      {error && (
        <div className="alert error">
          {error}
        </div>
      )}

      <div className="dashboard-stats">
        <div className="stat-card">
          <h3>Transactions</h3>
          <div className="stat-number">{transactions.length}</div>
        </div>
        <div className="stat-card">
          <h3>Items Disponibles</h3>
          <div className="stat-number">{items.length}</div>
        </div>
        <div className="stat-card">
          <h3>Total Transactions</h3>
          <div className="stat-number">
            {transactions.reduce((sum, t) => sum + (t.totalAmount || 0), 0)} €
          </div>
        </div>
      </div>

      <div className="dashboard-content">
        <div className="dashboard-section">
          <div className="section-header">
            <h2>Transactions Récentes</h2>
            <button
              onClick={() => setShowCreateTransaction(true)}
              className="btn btn-primary"
            >
              Nouvelle Transaction
            </button>
          </div>
          
          <div className="transactions-list">
            {transactions.length === 0 ? (
              <p>Aucune transaction pour le moment.</p>
            ) : (
              transactions.map((transaction) => (
                <div key={transaction.id} className="transaction-card">
                  <div className="transaction-info">
                    <h4>Transaction #{transaction.id}</h4>
                    <p>Date: {new Date(transaction.transactionDate).toLocaleDateString()}</p>
                    <p>Montant: {transaction.totalAmount || 0} €</p>
                  </div>
                  <div className="transaction-items">
                    {transaction.items && transaction.items.length > 0 && (
                      <div className="items-list">
                        {transaction.items.map((item) => (
                          <span key={item.id} className="item-tag">
                            {item.name}
                          </span>
                        ))}
                      </div>
                    )}
                  </div>
                </div>
              ))
            )}
          </div>
        </div>

        <div className="dashboard-section">
          <h2>Items Disponibles</h2>
          <div className="items-grid">
            {items.map((item) => (
              <div key={item.id} className="item-card">
                <h4>{item.name}</h4>
                <p>{item.description}</p>
                <div className="item-price">{item.price} €</div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {showCreateTransaction && (
        <CreateTransactionModal
          items={items}
          onSubmit={handleCreateTransaction}
          onClose={() => setShowCreateTransaction(false)}
        />
      )}
    </div>
  );
};

interface CreateTransactionModalProps {
  items: Item[];
  onSubmit: (data: CreateTransactionRequest) => void;
  onClose: () => void;
}

const CreateTransactionModal: React.FC<CreateTransactionModalProps> = ({ items, onSubmit, onClose }) => {
  const [selectedItems, setSelectedItems] = useState<Item[]>([]);
  const [loading, setLoading] = useState(false);

  const handleItemToggle = (item: Item) => {
    setSelectedItems(prev => {
      const exists = prev.find(i => i.id === item.id);
      if (exists) {
        return prev.filter(i => i.id !== item.id);
      } else {
        return [...prev, item];
      }
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (selectedItems.length === 0) return;

    setLoading(true);
    try {
      const transactionData: CreateTransactionRequest = {
        userId: 1, // À adapter avec l'ID utilisateur réel
        itemIds: selectedItems.map(item => item.id)
      };
      await onSubmit(transactionData);
    } finally {
      setLoading(false);
    }
  };

  const totalAmount = selectedItems.reduce((sum, item) => sum + (item.price || 0), 0);

  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal-header">
          <h3>Nouvelle Transaction</h3>
          <button onClick={onClose} className="modal-close">&times;</button>
        </div>
        
        <form onSubmit={handleSubmit} className="modal-body">
          <div className="items-selection">
            {items.map((item) => (
              <label key={item.id} className="item-checkbox">
                <input
                  type="checkbox"
                  checked={selectedItems.some(i => i.id === item.id)}
                  onChange={() => handleItemToggle(item)}
                />
                <div className="item-info">
                  <span>{item.name}</span>
                  <span>{item.price} €</span>
                </div>
              </label>
            ))}
          </div>
          
          <div className="modal-footer">
            <div className="total-amount">
              Total: {totalAmount} €
            </div>
            <div className="modal-actions">
              <button type="button" onClick={onClose} className="btn btn-secondary">
                Annuler
              </button>
              <button
                type="submit"
                disabled={selectedItems.length === 0 || loading}
                className="btn btn-primary"
              >
                {loading ? 'Création...' : 'Créer'}
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Dashboard;
