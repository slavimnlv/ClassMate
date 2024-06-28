import React, { useState } from 'react';
import { Link, Form } from 'react-router-dom';
import customRequest from '../utils/customRequest';
import { toast } from 'react-toastify';
import { MdFileDownloadDone } from 'react-icons/md';
import { CiEdit } from 'react-icons/ci';
import { MdDelete } from 'react-icons/md';
import { SiGooglecalendar } from 'react-icons/si';
import { handleCalendarSync } from '../utils/googleSync';
import { MdOutlineDoNotDisturb } from 'react-icons/md';
import { GrShareOption } from 'react-icons/gr';
import Popup from './Popup';

const ToDo = ({
  id,
  title,
  description,
  deadline,
  done: initialDone,
  parentToDo,
  role,
  onDelete,
}) => {
  //
  const [done, setDone] = useState(initialDone);
  const [loading, setLoading] = useState(false);

  const AddToCalendadr = async () => {
    setLoading(true);
    await handleCalendarSync(
      '/todos/sync',
      { resourceId: id, platform: 0 },
      '/dashboard/todos'
    );
    setLoading(false);
  };

  const Delete = async () => {
    try {
      await customRequest.delete(`/todos/${id}`);
      toast.success('Deleted todo successful');
      onDelete(id);
    } catch (error) {
      toast.error(error?.response?.data?.message);
    }
    return redirect('/dashboard/todos');
  };

  const [showPopup, setShowPopup] = useState(false);

  const togglePopup = () => {
    setShowPopup(!showPopup);
  };

  const toogleDone = async () => {
    const newDone = !done;
    setDone(newDone);

    try {
      const data = {
        id,
        title,
        description,
        deadline,
        done: newDone,
        parentToDo,
      };

      await customRequest.put('/todos', data);
      toast.success(
        `Task status set to ${newDone ? '"done"' : '"in progress"'}`
      );
    } catch (error) {
      toast.error('Failed to update task status');
    }
  };

  const formatDeadline = (dateString) => {
    const options = {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    };
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', options).format(date);
  };

  const hasPassedDeadline = new Date(deadline) < new Date() && !done;

  return (
    <>
      {showPopup && <Popup onClose={togglePopup} id={id} route={'todos'} />}

      <div className='single-container-small mid'>
        <h2
          style={{
            textDecoration: done ? 'line-through' : 'none',
          }}
        >
          {title}
        </h2>
        <p style={{ color: hasPassedDeadline ? 'red' : 'inherit' }}>
          due {formatDeadline(deadline)}
        </p>
        <p>{description}</p>
        <div className='role'>
          <span>{role === 0 ? '' : 'shared with me'}</span>
        </div>
        <div className='actions'>
          {role === 2 || (
            <button type='button' onClick={toogleDone}>
              {done ? <MdOutlineDoNotDisturb /> : <MdFileDownloadDone />}
            </button>
          )}
          {role === 2 || (
            <Link to={`edit/${id}`}>
              <button>
                <CiEdit />
              </button>
            </Link>
          )}
          {role === 2 || (
            <Link>
              <button className='white-stroke-path' onClick={togglePopup}>
                <GrShareOption />
              </button>
            </Link>
          )}
          <button onClick={AddToCalendadr} disabled={loading}>
            <SiGooglecalendar />
          </button>
          <div>
            <button onClick={Delete}>
              <MdDelete />
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default ToDo;
