import { Form, useLoaderData, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const loader = async ({ params }) => {
  const { id } = params;
  try {
    const response = await customRequest.get(`/notes/${id}`);
    return response.data;
  } catch (error) {
    toast.error('Failed to fetch note data');
    return redirect('/notes');
  }
};

export const action = async ({ request, params }) => {
  const { id } = params;
  const formData = await request.formData();
  const data = {
    id,
    ...Object.fromEntries(formData),
  };

  try {
    await customRequest.put('/notes', data);
    toast.success('Note updated successfully');
    return redirect('/dashboard/notes');
  } catch (error) {
    toast.error(error?.response?.data?.message || 'Failed to update note');
    return error;
  }
};

const EditNote = () => {
  const noteData = useLoaderData();

  return (
    <section className='form-section'>
      <div className='form-container note'>
        <Form method='post'>
          <h2>Edit Note</h2>
          <input type='hidden' name='id' value={noteData.id} />
          <div className='input-group'>
            <label>Title</label>
            <input
              type='text'
              name='title'
              className='biger-input-font'
              defaultValue={noteData.title}
              required
            />
          </div>
          <div className='input-group'>
            <label>Content</label>
            <textarea
              name='content'
              defaultValue={noteData.content}
              required
              rows='10'
              style={{ width: '100%', resize: 'vertical' }}
            />
          </div>
          <button type='submit'>Update</button>
        </Form>
      </div>
    </section>
  );
};

export default EditNote;
