import { Form, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const action = async ({ request }) => {
  const formData = await request.formData();
  const data = Object.fromEntries(formData);
  try {
    await customRequest.post('/todos', data);
    toast.success('ToDo added successfully');
    return redirect('../');
  } catch (error) {
    toast.error(error?.response?.data?.message);
    return error;
  }
};

const AddToDo = () => {
  return (
    <section className='form-section'>
      <div className='form-container'>
        <Form method='post'>
          <h2>Add ToDo</h2>
          <div className='input-group'>
            <label>Title</label>
            <input
              type='text'
              name='title'
              className='biger-input-font'
              required
            />
          </div>
          <div className='input-group'>
            <label>Description</label>
            <input type='text' name='description' />
          </div>
          <div className='input-group'>
            <label>Deadline</label>
            <input type='datetime-local' name='deadline' required />
          </div>
          <button type='submit'>Add</button>
        </Form>
      </div>
    </section>
  );
};

export default AddToDo;
