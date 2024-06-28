import { Form, useLoaderData, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useState, useEffect } from 'react';
import customRequest from '../utils/customRequest';

export const loader = async ({ params }) => {
  const { id } = params;
  try {
    const response = await customRequest.get(`/classes/${id}`);
    return response.data;
  } catch (error) {
    toast.error('Failed to fetch class data');
    return redirect('/classes');
  }
};

export const action = async ({ request, params }) => {
  const { id } = params;
  const formData = await request.formData();
  const date = formData.get('start');
  const fromTime = formData.get('from');
  const toTime = formData.get('to');

  const startDate = `${date}T${fromTime}`;
  const endDate = `${date}T${toTime}`;
  const repeatUntilDate = formData.get('repeatUntil');

  let repeatUntil = null;
  if (repeatUntilDate) {
    repeatUntil = `${repeatUntilDate}T${toTime}`;
  }

  const data = {
    id,
    ...Object.fromEntries(formData),
    weekRepetition: formData.get('weekRepetition')
      ? parseInt(formData.get('weekRepetition'), 10)
      : null,
    startDate,
    endDate,
    repeatUntil,
  };

  try {
    await customRequest.put('/classes', data);
    toast.success('Class updated successfully');
    return redirect('/dashboard/classes');
  } catch (error) {
    toast.error(error?.response?.data?.message || 'Failed to update class');
    return error;
  }
};

const EditClass = () => {
  const classData = useLoaderData();
  const [weekRepetition, setWeekRepetition] = useState('');
  const [repeatUntil, setRepeatUntil] = useState('');

  useEffect(() => {
    if (classData.weekRepetition !== null) {
      setWeekRepetition(classData.weekRepetition.toString());
      setRepeatUntil(
        classData.repeatUntil ? classData.repeatUntil.split('T')[0] : ''
      );
    }
  }, [classData]);

  const handleWeekRepetitionChange = (event) => {
    const value = event.target.value;
    setWeekRepetition(value);
    if (value !== '' && repeatUntil === '') {
      const today = new Date().toISOString().split('T')[0];
      setRepeatUntil(today);
    } else if (value === '') {
      setRepeatUntil('');
    }
  };

  const handleRepeatUntilChange = (event) => {
    const value = event.target.value;
    setRepeatUntil(value);
    if (value !== '' && weekRepetition === '') {
      setWeekRepetition('1');
    } else if (value === '') {
      setWeekRepetition('');
    }
  };

  return (
    <section className='form-section'>
      <div className='form-container'>
        <Form method='post'>
          <h2>Edit Class</h2>
          <div className='input-group'>
            <label>Title</label>
            <input
              type='text'
              name='title'
              className='biger-input-font'
              defaultValue={classData.title}
              required
            />
          </div>
          <div className='input-group'>
            <label>Description</label>
            <input
              type='text'
              name='description'
              defaultValue={classData.description}
            />
          </div>
          <div className='input-group'>
            <label>Repeat every</label>
            <input
              type='number'
              name='weekRepetition'
              min='0'
              value={weekRepetition}
              onChange={handleWeekRepetitionChange}
            />
            <p>weeks</p>
          </div>
          <div className='input-group'>
            <label>Date</label>
            <input
              type='date'
              name='start'
              defaultValue={classData.startDate.split('T')[0]}
              required
            />
          </div>
          <div className='input-group'>
            <label>From (Time)</label>
            <input
              type='time'
              name='from'
              defaultValue={classData.startDate.split('T')[1]}
              required
            />
          </div>
          <div className='input-group'>
            <label>To (Time)</label>
            <input
              type='time'
              name='to'
              defaultValue={classData.endDate.split('T')[1]}
              required
            />
          </div>
          <div className='input-group'>
            <label>Repeat Until</label>
            <input
              type='date'
              name='repeatUntil'
              value={repeatUntil}
              onChange={handleRepeatUntilChange}
            />
          </div>
          <button type='submit'>Update</button>
        </Form>
      </div>
    </section>
  );
};

export default EditClass;
