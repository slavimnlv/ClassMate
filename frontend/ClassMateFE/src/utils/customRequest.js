import axios from 'axios';
import { validateToken } from './validateJWT';

const customRequest = axios.create({
  baseURL: 'https://localhost:7135/api',
});

const getToken = () => localStorage.getItem('ClassMateAppToken');

customRequest.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (validateToken(token)) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default customRequest;
