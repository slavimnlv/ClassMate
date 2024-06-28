import { toast } from 'react-toastify';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import { IoAddOutline } from 'react-icons/io5';
import customRequest from '../utils/customRequest';
import { useLoaderData } from 'react-router-dom';
import Class from '../components/Class';

export const loader = async () => {
  try {
    const data = await customRequest.get('/classes');
    return data?.data;
  } catch (error) {
    toast.error(error?.response?.data?.message);
    return [];
  }
};

const daysOfWeek = [
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
  'Sunday',
];

const AllClasses = () => {
  const [classes, setClasses] = useState(useLoaderData());

  const getDayOfWeek = (dateString) => {
    const date = new Date(dateString);
    let dayIndex = date.getDay() - 1;
    if (dayIndex < 0) {
      dayIndex = 6;
    }
    return daysOfWeek[dayIndex];
  };

  const classesByDay = classes?.reduce((acc, classData) => {
    const day = getDayOfWeek(classData.startDate);
    if (!acc[day]) acc[day] = [];
    acc[day].push(classData);
    return acc;
  }, {});

  Object.keys(classesByDay).forEach((day) => {
    classesByDay[day].sort(
      (a, b) => new Date(a.startDate) - new Date(b.startDate)
    );
  });

  const daysWithClasses = daysOfWeek.filter(
    (day) => classesByDay[day] && classesByDay[day].length > 0
  );
  const gridTemplateColumns = `repeat(${daysWithClasses.length}, 430px)`;

  const handleDelete = (id) => {
    setClasses((prevClasses) => prevClasses.filter((clas) => clas.id !== id));
  };

  return (
    <div>
      <div className='add-actions'>
        <Link to={'add'} className='add-new-link'>
          <button>
            <IoAddOutline />
          </button>
        </Link>
      </div>
      {classes && classes.length === 0 ? (
        <p className='centered' style={{ marginTop: '220px' }}>
          No classes found
        </p>
      ) : (
        <div className='all-container-classes' style={{ gridTemplateColumns }}>
          {daysWithClasses.map((day) => (
            <div key={day} className='day-column'>
              <h3 className='day-title'>{day}</h3>
              <div className='day-classes'>
                {classesByDay[day].map((classData) => (
                  <Class
                    key={classData.id}
                    {...classData}
                    onDelete={handleDelete}
                  />
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default AllClasses;
