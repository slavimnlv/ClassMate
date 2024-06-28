import { Form, redirect } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useState } from 'react';
import customRequest from '../utils/customRequest';

export const action = async ({ request }) => {
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
    ...Object.fromEntries(formData),
    weekRepetition: formData.get('weekRepetition')
      ? parseInt(formData.get('weekRepetition'), 10)
      : null,
    startDate,
    endDate,
    repeatUntil,
  };

  delete data.start;
  delete data.from;
  delete data.to;

  try {
    await customRequest.post('/classes', data);
    toast.success('Class added successfuly');
    return redirect('../');
  } catch (error) {
    toast.error(error?.response?.data?.message);
    return error;
  }
};

const AddClass = () => {
  const [weekRepetition, setWeekRepetition] = useState('');
  const [repeatUntil, setRepeatUntil] = useState('');

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
          <h2>Add Class</h2>
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
            <label>Repeat every</label>
            <input
              type='number'
              name='weekRepetition'
              min='1'
              value={weekRepetition}
              onChange={handleWeekRepetitionChange}
            />
            <p>weeks</p>
          </div>
          <div className='input-group'>
            <label> Date</label>
            <input type='date' name='start' required />
          </div>
          <div className='input-group'>
            <label>From (Time)</label>
            <input type='time' name='from' required />
          </div>
          <div className='input-group'>
            <label>To (Time)</label>
            <input type='time' name='to' required />
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
          <button type='submit'>Add</button>
        </Form>
      </div>
    </section>
  );
};

export default AddClass;
