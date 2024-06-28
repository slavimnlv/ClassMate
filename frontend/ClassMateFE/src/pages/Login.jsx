import { Link, Form, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const action = async ({ request }) => {
  const formData = await request.formData();
  const data = Object.fromEntries(formData);
  try {
    const response = await customRequest.post('/auth/login', data);
    toast.success('Login successful');
    localStorage.setItem('ClassMateAppToken', response?.data?.token);
    return redirect('/dashboard');
  } catch (error) {
    toast.error(error?.response?.data?.message);
    return error;
  }
};

const Login = () => {
  return (
    <section className='form-section'>
      <div className='form-container bigger-form-margin'>
        <Form method='post'>
          <h2>Login</h2>
          <div className='input-group'>
            <label>Email</label>
            <input type='email' name='email' required />
          </div>
          <div className='input-group'>
            <label>Password</label>
            <input type='password' name='password' required />
          </div>
          <button type='submit'>Login</button>
          <p>
            Not a member yet?
            <Link to='/register' className='link'>
              Register
            </Link>
          </p>
        </Form>
      </div>
    </section>
  );
};

export default Login;
