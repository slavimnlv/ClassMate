import { jwtDecode } from 'jwt-decode';

const isTokenExpired = (token) => {
  if (!token) {
    return true;
  }

  const decodedToken = jwtDecode(token);
  const currentTime = Date.now() / 1000;

  return decodedToken.exp < currentTime;
};

export const checkTokenValidity = () => {
  const token = localStorage.getItem('ClassMateAppToken');
  return !isTokenExpired(token);
};

export const validateToken = (token) => {
  return !isTokenExpired(token);
};
