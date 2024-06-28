import { Form, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const action = async ({ request }) => {
  const formData = await request.formData();
  const data = Object.fromEntries(formData);
  try {
    await customRequest.post('/notes', data);
    toast.success('Note added successfuly');
    return redirect('../');
  } catch (error) {
    toast.error(error?.response?.data?.errors?.Content[0]);
    return error;
  }
};

const AddNote = () => {
  return (
    <section className='form-section'>
      <div className='form-container note'>
        <Form method='post'>
          <h2>Add Note</h2>
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
            <label>Content</label>
            <textarea
              name='content'
              required
              rows='10'
              style={{ width: '100%', resize: 'vertical' }}
            />
          </div>
          <button type='submit'>Add</button>
        </Form>
      </div>
    </section>
  );
};

export default AddNote;
