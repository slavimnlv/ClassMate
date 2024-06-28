import { Outlet } from 'react-router-dom';
import Nav from '../components/Nav';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { checkTokenValidity } from '../utils/validateJWT';

const DashboardLayout = () => {
  const navigate = useNavigate();
  useEffect(() => {
    if (!checkTokenValidity()) {
      navigate('/login');
    }
  }, [navigate]);

  return (
    <>
      <Nav />
      <div className='main-content'>
        <Outlet />
      </div>
    </>
  );
};
export default DashboardLayout;
