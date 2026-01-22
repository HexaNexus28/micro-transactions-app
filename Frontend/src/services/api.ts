import axios, { AxiosInstance, AxiosResponse } from 'axios';
import ENDPOINTS from '../config/endpoints';
import {
    User,
    LoginRequest,
    RegisterRequest,
    AuthResponse,
    Item,
    Transaction,
    CreateTransactionRequest,
    ApiResponse
} from '../types';

class ApiService {
    private api: AxiosInstance;

    constructor() {
        this.api = axios.create({
            baseURL: 'http://localhost:5037/api',
            timeout: 10000,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // Request interceptor to add auth token
        this.api.interceptors.request.use(
            (config) => {
                const token = localStorage.getItem('token');
                if (token) {
                    config.headers.Authorization = `Bearer ${token}`;
                }
                return config;
            },
            (error) => {
                return Promise.reject(error);
            }
        );

        // Response interceptor for error handling
        this.api.interceptors.response.use(
            (response: AxiosResponse) => response,
            (error) => {
                if (error.response?.status === 401) {
                    // Token expired or invalid
                    localStorage.removeItem('token');
                    localStorage.removeItem('user');
                    window.location.href = '/login';
                }
                return Promise.reject(error);
            }
        );
    }

    // User endpoints
    async registerUser(userData: RegisterRequest): Promise<AuthResponse> {
        const response: AxiosResponse<AuthResponse> = await this.api.post(
            ENDPOINTS.USER.REGISTER,
            userData
        );
        return response.data;
    }

    async loginUser(credentials: LoginRequest): Promise<AuthResponse> {
        const response: AxiosResponse<AuthResponse> = await this.api.post(
            ENDPOINTS.USER.LOGIN,
            credentials
        );
        return response.data;
    }

    async getUsers(): Promise<ApiResponse<User[]>> {
        const response: AxiosResponse<ApiResponse<User[]>> = await this.api.get(
            ENDPOINTS.USER.BASE
        );
        return response.data;
    }

    async getUserById(id: number): Promise<ApiResponse<User>> {
        const response: AxiosResponse<ApiResponse<User>> = await this.api.get(
            ENDPOINTS.USER.GET_BY_ID(id)
        );
        return response.data;
    }

    // Transaction endpoints
    async createTransaction(transactionData: CreateTransactionRequest): Promise<ApiResponse<Transaction>> {
        const response: AxiosResponse<ApiResponse<Transaction>> = await this.api.post(
            ENDPOINTS.TRANSACTION.BASE,
            transactionData
        );
        return response.data;
    }

    async getTransactions(): Promise<ApiResponse<Transaction[]>> {
        const response: AxiosResponse<ApiResponse<Transaction[]>> = await this.api.get(
            ENDPOINTS.TRANSACTION.BASE
        );
        return response.data;
    }

    async getTransactionById(id: number): Promise<ApiResponse<Transaction>> {
        const response: AxiosResponse<ApiResponse<Transaction>> = await this.api.get(
            ENDPOINTS.TRANSACTION.GET_BY_ID(id)
        );
        return response.data;
    }

    async getTransactionsByUserId(userId: number): Promise<ApiResponse<Transaction[]>> {
        const response: AxiosResponse<ApiResponse<Transaction[]>> = await this.api.get(
            ENDPOINTS.TRANSACTION.GET_BY_USER_ID(userId)
        );
        return response.data;
    }

    // Item endpoints
    async getItems(): Promise<ApiResponse<Item[]>> {
        const response: AxiosResponse<ApiResponse<Item[]>> = await this.api.get(
            ENDPOINTS.ITEM.BASE
        );
        return response.data;
    }

    async getItemById(id: number): Promise<ApiResponse<Item>> {
        const response: AxiosResponse<ApiResponse<Item>> = await this.api.get(
            ENDPOINTS.ITEM.GET_BY_ID(id)
        );
        return response.data;
    }

    // AuthToken endpoints
    async getAuthTokens(): Promise<ApiResponse<any[]>> {
        const response: AxiosResponse<ApiResponse<any[]>> = await this.api.get(
            ENDPOINTS.AUTH_TOKEN.BASE
        );
        return response.data;
    }
}

export const apiService = new ApiService();
export default apiService;
