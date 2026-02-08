export const ENDPOINTS = {
    // User endpoints
    USER: {
        BASE: '/user',
        REGISTER: '/user/register',
        LOGIN: '/user/login',
        GET_BY_ID: (id: number) => `/user/${id}`,
    },

    // Transaction endpoints
    TRANSACTION: {
        BASE: '/transaction',
        GET_BY_ID: (id: number) => `/transaction/${id}`,
        GET_BY_USER_ID: (userId: number) => `/transaction/user/${userId}`,
    },

    // Item endpoints
    ITEM: {
        BASE: '/item',
        GET_BY_ID: (id: number) => `/item/${id}`,
    },

    // AuthToken endpoints
    AUTH_TOKEN: {
        BASE: '/authtoken',
        GET_BY_ID: (id: number) => `/authtoken/${id}`,
    },
} as const;

export default ENDPOINTS;
