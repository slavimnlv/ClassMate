import { Link, Form, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const action = async ({ request }) => {
  const formData = await request.formData();
  const data = Object.fromEntries(formData);
  try {
    await customRequest.post('/auth/register', data);
    toast.success('Register successful');
    return redirect('/');
  } catch (error) {
    toast.error(
      error?.response?.data?.message ||
        (error?.response?.data?.errors?.Password &&
          'Password should contain: uppercase letter, lowercase letter, number, speacial character, and should be mininmum 8 characters long')
    );
    return error;
  }
};

const Register = () => {
  return (
    <section className='form-section'>
      <div className='form-container bigger-form-margin'>
        <Form method='post'>
          <h2>Register</h2>
          <div className='input-group'>
            <label>Name</label>
            <input type='text' name='name' required />
          </div>
          <div className='input-group'>
            <label>Email</label>
            <input type='email' name='email' required />
          </div>
          <div className='input-group'>
            <label>Password</label>
            <input type='password' name='password' required />
          </div>
          <button type='submit'>Register</button>
          <p>
            Already a member?
            <Link to='/login' className='link'>
              Login
            </Link>
          </p>
        </Form>
      </div>
    </section>
  );
};

export default Register;
