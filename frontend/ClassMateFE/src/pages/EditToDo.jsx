import { Form, useLoaderData, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

export const loader = async ({ params }) => {
  const { id } = params;
  try {
    const response = await customRequest.get(`/todos/${id}`);
    return response.data;
  } catch (error) {
    toast.error('Failed to fetch ToDo data');
    return redirect('/todos');
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
    await customRequest.put('/todos', data);
    toast.success('ToDo updated successfully');
    return redirect('/dashboard/todos');
  } catch (error) {
    toast.error(error?.response?.data?.message || 'Failed to update ToDo');
    return error;
  }
};

const EditToDo = () => {
  const toDoData = useLoaderData();

  return (
    <section className='form-section'>
      <div className='form-container'>
        <Form method='post'>
          <h2>Edit ToDo</h2>
          <input type='hidden' name='id' value={toDoData.id} />
          <div className='input-group'>
            <label>Title</label>
            <input
              type='text'
              name='title'
              className='biger-input-font'
              defaultValue={toDoData.title}
              required
            />
          </div>
          <div className='input-group'>
            <label>Description</label>
            <input
              type='text'
              name='description'
              defaultValue={toDoData.description}
            />
          </div>
          <div className='input-group'>
            <label>Deadline</label>
            <input
              type='datetime-local'
              name='deadline'
              defaultValue={toDoData.deadline}
              required
            />
          </div>
          <button type='submit'>Update</button>
        </Form>
      </div>
    </section>
  );
};

export default EditToDo;
