// User types
export interface User {
    id: number;
    userName: string;
    email: string;
    passwordHash?: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    userName: string;
    email: string;
    password: string;
}

export interface AuthResponse {
    success: boolean;
    data: {
        user: User;
        token: string;
    };
    message?: string;
}

// Transaction types
export interface Item {
    id: number;
    name: string;
    description: string;
    price: number;
}

export interface Transaction {
    id: number;
    transactionDate: string;
    userId: number;
    totalAmount?: number;
    items?: Item[];
}

export interface CreateTransactionRequest {
    userId: number;
    itemIds: number[];
}

// API Response types
export interface ApiResponse<T = any> {
    success: boolean;
    data?: T;
    message?: string;
    statusCode?: number;
}

// Auth Context types
export interface AuthContextType {
    user: User | null;
    token: string | null;
    loading: boolean;
    error: string | null;
    login: (credentials: LoginRequest) => Promise<AuthResponse>;
    register: (userData: RegisterRequest) => Promise<ApiResponse>;
    logout: () => void;
    isAuthenticated: boolean;
}
