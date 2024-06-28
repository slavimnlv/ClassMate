import { Link, Form } from 'react-router-dom';
import { CiEdit } from 'react-icons/ci';
import { MdDelete } from 'react-icons/md';
import { GrShareOption } from 'react-icons/gr';
import { toast } from 'react-toastify';
import { redirect } from 'react-router-dom';
import customRequest from '../utils/customRequest';
import { useState } from 'react';
import Popup from './Popup';

const Note = ({ id, title, role, onDelete }) => {
  //
  const [showPopup, setShowPopup] = useState(false);

  const togglePopup = () => {
    setShowPopup(!showPopup);
  };

  const Delete = async () => {
    try {
      await customRequest.delete(`/notes/${id}`);
      toast.success('Deleted note successful');
      onDelete(id);
    } catch (error) {
      toast.error(error?.response?.data?.message);
    }
    return redirect('/dashboard/notes');
  };

  return (
    <>
      {showPopup && <Popup onClose={togglePopup} id={id} route={'notes'} />}

      <div className='single-container-small'>
        <h2>{title}</h2>
        {
          // <div className='role'>
          //<span>{role === 0 ? '' : 'shared with me'}</span>
          //</div>
        }
        <div className='actions'>
          <Link to={`edit/${id}`}>
            <button>
              {' '}
              <CiEdit />
            </button>
          </Link>
          {role === 2 || (
            <Link>
              <button className='white-stroke-path' onClick={togglePopup}>
                <GrShareOption />
              </button>
            </Link>
          )}
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

export default Note;
