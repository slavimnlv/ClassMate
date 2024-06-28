import { useState, useEffect } from 'react';
import { Link, Form, redirect } from 'react-router-dom';
import { CiEdit } from 'react-icons/ci';
import { MdDelete } from 'react-icons/md';
import { SiGooglecalendar } from 'react-icons/si';
import { handleCalendarSync } from '../utils/googleSync';
import { toast } from 'react-toastify';
import customRequest from '../utils/customRequest';

const formatTime = (datetime) => {
  const date = new Date(datetime);
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};

const formatDate = (datetime) => {
  const date = new Date(datetime);
  return date.toLocaleDateString('en-GB', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  });
};

const Class = ({
  id,
  title,
  description,
  startDate,
  weekRepetition,
  endDate,
  repeatUntil,
  role,
  onDelete,
}) => {
  //
  const [loading, setLoading] = useState(false);

  const AddToCalendadr = async () => {
    setLoading(true);
    await handleCalendarSync(
      '/classes/sync',
      { resourceId: id, platform: 0 },
      '/dashboard/classes'
    );
    setLoading(false);
  };

  const Delete = async () => {
    try {
      await customRequest.delete(`/classes/${id}`);
      toast.success('Deleted class successful');
      onDelete(id);
    } catch (error) {
      toast.error(error?.response?.data?.message);
    }
    return redirect('/dashboard/classes');
  };

  return (
    <div className='single-container'>
      <h2>{title}</h2>
      <div className='details'>
        <span>
          <span>
            <strong>Details: </strong>
            {description}
          </span>
        </span>
        <span>
          <strong>Start: </strong>
          {formatDate(startDate)}
        </span>
        <span>
          <strong>From:</strong> {formatTime(startDate)}
        </span>
        <span>
          <strong>To:</strong> {formatTime(endDate)}
        </span>
        {repeatUntil && (
          <span>
            <strong>Until:</strong> {formatDate(repeatUntil)}
          </span>
        )}
        {weekRepetition && (
          <span>
            <strong>Repeat:</strong> Every{' '}
            {weekRepetition > 1 ? ` ${weekRepetition} weeks` : 'week'}
          </span>
        )}
      </div>
      <div className='role'>
        <span>{role === 0 ? '' : 'shared with me'}</span>
      </div>
      <div className='actions'>
        <Link to={`edit/${id}`}>
          <button>
            <CiEdit />
          </button>
        </Link>
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
  );
};

export default Class;
