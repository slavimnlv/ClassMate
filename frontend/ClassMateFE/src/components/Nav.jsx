import { NavLink, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

const Nav = () => {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem('ClassMateAppToken');
    navigate('/');
    toast.success('Logout successful');
  };

  return (
    <>
      {' '}
      <div className='headline'>Class &nbsp;&nbsp;&nbsp;&nbsp;Mate</div>
      <nav className='left-side-nav'>
        <ul>
          <li>
            <NavLink to='/dashboard' end>
              Home
            </NavLink>
          </li>
          <li>
            <NavLink to='/dashboard/classes'>Classes</NavLink>
          </li>
          <li>
            <NavLink to='/dashboard/notes'>Notes</NavLink>
          </li>
          <li>
            <NavLink to='/dashboard/todos'>Todos</NavLink>
          </li>
          <li>
            <a className='cursor-pointer' onClick={logout}>
              Logout
            </a>
          </li>
        </ul>
      </nav>
    </>
  );
};

export default Nav;
