const API_BASE_URL = 'http://localhost:5037/api';

export const ENDPOINTS = {
    // User endpoints
    USER: {
        BASE: `${API_BASE_URL}/user`,
        REGISTER: `${API_BASE_URL}/user/register`,
        LOGIN: `${API_BASE_URL}/user/login`,
        GET_BY_ID: (id: number) => `${API_BASE_URL}/user/${id}`,
    },

    // Transaction endpoints
    TRANSACTION: {
        BASE: `${API_BASE_URL}/transaction`,
        GET_BY_ID: (id: number) => `${API_BASE_URL}/transaction/${id}`,
        GET_BY_USER_ID: (userId: number) => `${API_BASE_URL}/transaction/user/${userId}`,
    },

    // Item endpoints
    ITEM: {
        BASE: `${API_BASE_URL}/item`,
        GET_BY_ID: (id: number) => `${API_BASE_URL}/item/${id}`,
    },

    // AuthToken endpoints
    AUTH_TOKEN: {
        BASE: `${API_BASE_URL}/authtoken`,
        GET_BY_ID: (id: number) => `${API_BASE_URL}/authtoken/${id}`,
    },
} as const;

export default ENDPOINTS;
