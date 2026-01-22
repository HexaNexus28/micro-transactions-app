# Frontend Documentation - React TypeScript

Documentation spécifique au développement frontend de l'application Micro-Transactions RPG.

---

## Architecture Frontend

### Structure des Dossiers
```
src/
├── styles/             # Styles CSS globaux
├── components/         # Composants réutilisables
├── pages/             # Pages de l'application
├── contexts/          # Contextes React (état global)
├── services/          # Services API (Axios)
├── config/            # Configuration (endpoints)
├── types/             # Types TypeScript
└── assets/            # Images et ressources
```

### Stack Technique
- **React 19** avec **TypeScript** strict
- **Vite** comme bundler ultra-rapide
- **Axios** pour les requêtes HTTP
- **React Router** pour la navigation
- **CSS Modules** pour le styling

---

## Composants & Pages

### Layout Component
```typescript
// src/components/Layout.tsx
interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout, isAuthenticated } = useAuth();
  // Header + Main + Footer
};
```

### Pages Principales

#### Login Page
- **Fonctionnalités**: Connexion & Inscription
- **States**: Formulaire, loading, erreurs
- **Validation**: Email format, password requirements

#### Dashboard Page
- **Fonctionnalités**: Vue d'ensemble, transactions, items
- **States**: Données utilisateur, loading, modals
- **Interactions**: Création transaction, navigation

---

## Gestion d'Authentification

### AuthContext Pattern
```typescript
// src/contexts/AuthContext.tsx
interface AuthContextType {
  user: User | null;
  token: string | null;
  loading: boolean;
  error: string | null;
  login: (credentials: LoginRequest) => Promise<AuthResponse>;
  register: (userData: RegisterRequest) => Promise<ApiResponse>;
  logout: () => void;
  isAuthenticated: boolean;
}
```

### Flow Authentification
1. **Login**: Appel API → Token JWT → Stockage localStorage
2. **Auto-login**: Vérification token au démarrage
3. **Logout**: Nettoyage token + redirection
4. **Token Expired**: Interceptor Axios → Déconnexion auto

---

## Services API

### Configuration Axios
```typescript
// src/services/api.ts
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  timeout: 10000,
  headers: { 'Content-Type': 'application/json' }
});

// Interceptor pour JWT
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### Endpoints Typés
```typescript
// src/config/endpoints.ts
export const ENDPOINTS = {
  USER: {
    LOGIN: `${API_BASE_URL}/user/login`,
    REGISTER: `${API_BASE_URL}/user/register`,
  },
  TRANSACTION: {
    BASE: `${API_BASE_URL}/transaction`,
  }
} as const;
```

---

## Types TypeScript

### Types Principaux
```typescript
// src/types/index.ts
export interface User {
  id: number;
  userName: string;
  email: string;
}

export interface Transaction {
  id: number;
  transactionDate: string;
  userId: number;
  totalAmount?: number;
  items?: Item[];
}

export interface ApiResponse<T = any> {
  success: boolean;
  data?: T;
  message?: string;
  statusCode?: number;
}
```

### Types de Requêtes
```typescript
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
}

export interface CreateTransactionRequest {
  userId: number;
  itemIds: number[];
}
```

---

## Styling & CSS

### Organisation CSS
```
src/styles/
├── App.css          # Styles globaux
├── Layout.css       # Header, Footer, Navigation
├── Login.css        # Page d'authentification
├── Dashboard.css    # Tableau de bord
└── index.css        # Variables et reset
```

### Conventions CSS
```css
/* Variables CSS */
:root {
  --primary-color: #667eea;
  --secondary-color: #764ba2;
  --success-color: #3c3;
  --error-color: #c33;
  --text-color: #213547;
  --bg-color: #f8f9fa;
}

/* Classes utilitaires */
.btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-primary {
  background: var(--primary-color);
  color: white;
}

/* Responsive Design */
@media (max-width: 768px) {
  .dashboard {
    padding: 1rem;
  }
}
```

---

## State Management

### Context API Usage
```typescript
// Dans un composant
const { user, login, logout, isAuthenticated } = useAuth();

// Login handler
const handleLogin = async (credentials: LoginRequest) => {
  try {
    await login(credentials);
    navigate('/dashboard');
  } catch (error) {
    setError(error.message);
  }
};
```

### Local State Patterns
```typescript
// Form state avec useState
const [formData, setFormData] = useState<LoginRequest>({
  email: '',
  password: ''
});

// Loading state
const [loading, setLoading] = useState(false);

// Error handling
const [error, setError] = useState<string | null>(null);
```

---

## Gestion des Erreurs

### Error Boundaries
```typescript
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Error caught by boundary:', error, errorInfo);
  }
}
```

### API Error Handling
```typescript
try {
  const response = await apiService.loginUser(credentials);
  // Succès
} catch (error: any) {
  const errorMessage = error.response?.data?.message || 
                       error.message || 
                       'Login failed';
  setError(errorMessage);
}
```

---

## Performance & Optimisations

### Optimisations React
```typescript
// React.memo pour composants optimisés
const TransactionCard = React.memo(({ transaction }) => {
  return <div>{/* contenu */}</div>;
});

// useCallback pour fonctions stables
const handleSubmit = useCallback(async (data) => {
  await createTransaction(data);
}, [createTransaction]);

// useMemo pour calculs coûteux
const totalAmount = useMemo(() => 
  transactions.reduce((sum, t) => sum + (t.totalAmount || 0), 0),
  [transactions]
);
```

### Optimisations Axios
```typescript
// Request cancellation avec AbortController
const controller = new AbortController();

const fetchData = async () => {
  try {
    const response = await api.get('/transactions', {
      signal: controller.signal
    });
    return response.data;
  } catch (error) {
    if (error.name !== 'CanceledError') {
      throw error;
    }
  }
};

// Cleanup
return () => controller.abort();
```

---

## Développement & Debug

### Outils de Développement
```bash
# Développement avec hot reload
npm run dev

# Build pour production
npm run build

# Linting TypeScript
npm run lint

# Preview build local
npm run preview
```

### Debug Tips
```typescript
// Console logging structuré
console.log('Login attempt:', { email, timestamp: new Date() });

// Debug React DevTools
// Ajouter displayName aux composants
Login.displayName = 'LoginPage';

// Network debugging
// Utiliser onglet Network du navigateur
```

---

## 📱 Responsive Design

### Breakpoints
```css
/* Mobile */
@media (max-width: 768px) {
  .dashboard-stats {
    grid-template-columns: 1fr;
  }
}

/* Tablet */
@media (min-width: 769px) and (max-width: 1024px) {
  .dashboard-content {
    grid-template-columns: 1fr 1fr;
  }
}

/* Desktop */
@media (min-width: 1025px) {
  .dashboard-content {
    grid-template-columns: 2fr 1fr;
  }
}
```

### Mobile-First Approach
```css
/* Base styles (mobile) */
.login-card {
  width: 100%;
  padding: 1rem;
}

/* Desktop enhancements */
@media (min-width: 768px) {
  .login-card {
    max-width: 400px;
    padding: 2rem;
  }
}
```

---

## Configuration Environnement

### Variables d'Environnement
```typescript
// .env.development
VITE_API_BASE_URL=http://localhost:5000/api
VITE_APP_TITLE=Micro-Transactions Dev
VITE_API_KEY=1234567890

// .env.production
VITE_API_BASE_URL=https://api.microtransactions.com
VITE_APP_TITLE=Micro-Transactions
```

### Usage dans le Code
```typescript
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const APP_TITLE = import.meta.env.VITE_APP_TITLE;
```

---

## Build & Déploiement

### Processus de Build
```bash
# 1. Installation dépendances
npm ci

# 2. Linting
npm run lint

# 3. Tests
npm test

# 4. Build
npm run build

# 5. Preview
npm run preview
```

### Configuration Vite
```typescript
// vite.config.js
export default defineConfig({
  plugins: [react()],
  build: {
    outDir: 'dist',
    sourcemap: true,
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom'],
          router: ['react-router-dom']
        }
      }
    }
  }
});
```

---

## Bonnes Pratiques

### Code Organization
- **Un composant par fichier**
- **Types dans fichiers séparés**
- **Constants dans config**
- **Utils dans dossier dédié**

### Naming Conventions
```typescript
// Composants: PascalCase
const TransactionCard: React.FC = () => {};

// Fonctions: camelCase
const handleSubmit = async () => {};

// Variables: camelCase
const isLoading = false;

// Constants: UPPER_SNAKE_CASE
const API_ENDPOINTS = { /* ... */ };
```

### TypeScript Best Practices
```typescript
// Préférer les interfaces aux types pour les objets
interface User {
  id: number;
  name: string;
}

// Utiliser les types union pour les valeurs fixes
type Status = 'pending' | 'completed' | 'failed';

// Types génériques réutilisables
interface ApiResponse<T> {
  data: T;
  success: boolean;
}
```

---

## Next Steps

### Évolutions Frontend
- [ ] **Tests unitaires** avec Jest + React Testing Library
- [ ] **Storybook** pour documentation composants
- [ ] **PWA** pour offline support
- [ ] **Internationalisation** avec react-i18next
- [ ] **Theming** avec CSS variables
- [ ] **Animations** avec Framer Motion

### Performance
- [ ] **Code splitting** par route
- [ ] **Lazy loading** composants
- [ ] **Virtual scrolling** pour listes longues
- [ ] **Service Worker** pour caching

---

*Documentation maintenue par l'équipe frontend. Dernière mise à jour: Janvier 2024*
