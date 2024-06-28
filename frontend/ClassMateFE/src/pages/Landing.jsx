import { Link, useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { checkTokenValidity } from '../utils/validateJWT';

const Landing = () => {
  const navigate = useNavigate();

  useEffect(() => {
    if (checkTokenValidity()) {
      navigate('/dashboard');
    }
  }, []);

  return (
    <>
      <div className='landing'>
        <h1>Welcome to Class Mate</h1>
        <h3>A platform to manage your classes efficiently.</h3>
        <div className='landing-links'>
          <Link to='/login'>
            <button>Login</button>
          </Link>
          <Link to='/register'>
            <button>Register</button>
          </Link>
        </div>
      </div>
    </>
  );
};
export default Landing;
